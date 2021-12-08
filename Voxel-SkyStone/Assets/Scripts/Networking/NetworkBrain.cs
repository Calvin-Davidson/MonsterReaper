using System;
using UnityEngine;
using WebSocketSharp;

public class NetworkBrain : MonoBehaviour
{
    private WebSocket _client;

    private void Start()
    {
        _client = new WebSocket("ws://localhost:80/");
        _client.OnMessage += (sender, e) =>
        {
            string jsonString = e.Data;
            Debug.Log("message received from " + ((WebSocket)sender).Url + ", data : " + e.Data);
        };
        _client.OnError += (sender, e) =>
        {
            string jsonString = e.Message;
            Debug.Log("error from" + ((WebSocket)sender).Url + ", data : " + e.Message);
        };
        _client.Connect();
        
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
}
