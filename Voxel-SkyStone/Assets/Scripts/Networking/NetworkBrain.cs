using System.Collections;
using UnityEngine;
using WebSocketSharp;
using SimpleJSON;
using UnityEngine.Events;

public class NetworkBrain : MonoBehaviour
{
    [SerializeField] private bool localHost;
    [SerializeField] private bool connectOnAwake = false;
    
    private WebSocket _client;

    public UnityEvent onConnectionOpen = new UnityEvent();

    private void Awake()
    {
        if (connectOnAwake) Connect();
    }
    
    public void Connect()
    {
        StartCoroutine(TryConnect());
    }
    
    private IEnumerator TryConnect()
    {
        _client = localHost ? new WebSocket("ws://localhost:80") : new WebSocket("ws://voxel-relay.herokuapp.com/");
        _client.OnMessage += (sender, e) =>
        {
            JSONNode jsonNode = JSONNode.Parse(e.Data);
            if (jsonNode["action"] == NetworkAction.Connected.ToString())
            {
                Debug.Log("received connected");
            }
        };
        _client.OnOpen += (sender, args) =>
        {
            Debug.Log("connected opened");
            onConnectionOpen?.Invoke();
        };
        _client.ConnectAsync();

        float waitedTime = 0;
        while (waitedTime < 10)
        {
            waitedTime += Time.deltaTime;
            if (_client.IsAlive) break;
            yield return null;
        }

        print(_client.IsAlive ? "successfully connected" : "something went wrong while connecting");
    }
}
