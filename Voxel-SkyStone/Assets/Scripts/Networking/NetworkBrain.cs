using System;
using UnityEngine;
using WebSocketSharp;

public class NetworkBrain : MonoBehaviour
{
    private WebSocket _client;

    private void Start()
    {
        _client = new WebSocket("ws://voxel-relay.herokuapp.com/");
        _client.OnMessage += (sender, e) =>
        {
            string jsonString = e.Data;
            Debug.Log("message received from " + ((WebSocket)sender).Url + ", data : " + e.Data);
        };
        _client.OnOpen += (sender, args) =>
        {
            Debug.Log("connected opened");
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
