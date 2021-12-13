using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Networking;
using UnityEngine;
using UnityEngine.Events;
using NativeWebSocket;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(NetworkReceiveHandler))]
public class NetworkClient : MonoBehaviour
{
    public NetworkData networkData;

    public UnityEvent onConnectionFail = new UnityEvent();
    public UnityEvent onConnectionSuccessful = new UnityEvent();

    private WebSocket _client;
    private NetworkReceiveHandler _receiveHandler;
    private NetworkEventContainer _events;
    private static NetworkClient _instance;

    private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        _events = new NetworkEventContainer();
        DontDestroyOnLoad(this.gameObject);

        _receiveHandler = GetComponent<NetworkReceiveHandler>();
    }

    public void Connect()
    {
        Debug.Log("starting to connect");
        TryConnect();
    }

    public void Send(string json)
    {
        _client.SendText(json);
    }

    public NetworkEventContainer Events => _events;

    private async void TryConnect()
    {
        Debug.Log("trying to connect");
        if (networkData.Wss)
            _client = networkData.Localhost
                ? new WebSocket("wsw://localhost:80")
                : new WebSocket("wss://voxel-relay.herokuapp.com/");
        else
            _client = networkData.Localhost
                ? new WebSocket("ws://localhost:80")
                : new WebSocket("ws://voxel-relay.herokuapp.com/");

        InitEvents();

        await _client.Connect();

        if (_client.State == WebSocketState.Open || _client.State == WebSocketState.Connecting)
            onConnectionSuccessful?.Invoke();
        else
            onConnectionFail?.Invoke();
    }

    private void InitEvents()
    {
        _client.OnOpen += () =>
        {
            onConnectionSuccessful?.Invoke();
            Debug.Log("connection opened");
        };

        _client.OnClose += code =>
        {
            onConnectionFail?.Invoke();
            Debug.Log("connection closed");
            SceneManager.LoadScene("ConnectionLost");
        };

        _client.OnMessage += data =>
        {
            string message = System.Text.Encoding.UTF8.GetString(data);
            Debug.Log(message);
            _actions.Enqueue(() => _receiveHandler.OnPacketReceive(message, networkData));
        };
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _client?.DispatchMessageQueue();
#endif

        while (_actions.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }

    private async void OnApplicationQuit()
    {
        if (_client == null) return;
        await _client.Close();
    }

    public static NetworkClient Instance => _instance;
}