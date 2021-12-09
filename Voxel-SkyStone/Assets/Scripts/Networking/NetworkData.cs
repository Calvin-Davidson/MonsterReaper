using System;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

//[CreateAssetMenu(fileName = "NetworkData", menuName = "ScriptableObjects/NetworkData")]
namespace Networking
{
    public class NetworkData : ScriptableObject
    {
        [SerializeField] private bool localhost;
        
        private int _myId;
        private int _playerTurnId;
        private WebSocket _client;

        public WebSocket Client
        {
            get => _client;
            set => _client = value;
        }


        public bool Localhost => localhost;
        public event Action OnConnectionSuccessful;
        public event Action OnConnectionFail;
        
        public bool IsMyTurn()
        {
            return _myId == _playerTurnId;
        }
        
        public void Connect()
        {
            _client = localhost ? new WebSocket("ws://localhost:80") : new WebSocket("ws://voxel-relay.herokuapp.com/");
            _client.WaitTime = TimeSpan.FromSeconds(10);
            _client.Connect();
            
            _client.OnOpen += (sender, args) =>
            {
                OnConnectionSuccessful?.Invoke();
                Debug.Log("connection opened");
            };
            
            _client.OnClose += (sender, args) =>
            {
                OnConnectionFail?.Invoke();
                Debug.Log("connection closed");
            };

            _client.OnError += (sender, args) =>
            {
                Debug.Log(args.Exception);
                Debug.Log(args.Message);
                Debug.Log("error");
            };
        }
    }
}
