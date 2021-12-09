using System;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

public class NetworkBrain : MonoBehaviour
{
    [SerializeField] private bool localHost;
    [SerializeField] private bool connectOnAwake = false;
    
    private WebSocket _client;
    
    private void Awake()
    {
        if (connectOnAwake) Connect();
    }
    
    private void Update()
    {
        if (_client == null) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string message = "hello server!";
            _client.Send(message);
        }
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
            string jsonString = e.Data;
            Debug.Log("message received from " + ((WebSocket)sender).Url + ", data : " + e.Data);
        };
        _client.OnOpen += (sender, args) =>
        {
            Debug.Log("connected opened");
        };
        _client.ConnectAsync();

        float waitedTime = 0;
        while (waitedTime < 10)
        {
            waitedTime += Time.deltaTime;
            if (_client.IsAlive) break;
            yield return null;
        }

        if (_client.IsAlive) print("succesfully connected");
        else print("something went wrong while connecting");
    }
}
