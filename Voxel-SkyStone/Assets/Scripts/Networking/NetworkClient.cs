using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Networking;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

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
        StartCoroutine(TryConnect());
    }
    
    public void Send(string json)
    {
        _client.Send(json);
    }

    public NetworkEventContainer Events => _events;

    private IEnumerator TryConnect()
    {
        Debug.Log("trying to connect");
        _client = networkData.Localhost
            ? new WebSocket("ws://localhost:80")
            : new WebSocket("ws://voxel-relay.herokuapp.com/");

        InitEvents();

        _client.ConnectAsync();

        float waitedTime = 0;
        while (waitedTime < 10)
        {
            waitedTime += Time.deltaTime;
            if (_client.IsAlive) break;
            yield return null;
        }

        if (_client.IsAlive)
            onConnectionSuccessful?.Invoke();
        else
            onConnectionFail?.Invoke();
    }

    private void InitEvents()
    {
        _client.OnOpen += (sender, args) =>
        {
            onConnectionSuccessful?.Invoke();
            Debug.Log("connection opened");
        };

        _client.OnClose += (sender, args) =>
        {
            onConnectionFail?.Invoke();
            Debug.Log("connection closed");
        };

        _client.OnMessage += (sender, args) =>
        { 
            _actions.Enqueue(() => _receiveHandler.OnPacketReceive(args, networkData));
        };
    }

    private void Update()
    {
        while(_actions.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }

    private void OnApplicationQuit()
    {
        _client?.Close();
    }
    

    public static NetworkClient Instance => _instance;
}