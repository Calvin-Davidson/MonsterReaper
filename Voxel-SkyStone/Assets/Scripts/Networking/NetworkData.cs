using System;
using System.Collections;
using SimpleJSON;
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
        public event Action OnConnectionSuccessful;
        public event Action OnConnectionFail;
        public NetworkEventContainer networkEvents;

        private void Awake()
        {
            networkEvents = new NetworkEventContainer();
        }

        public WebSocket Client
        {
            get => _client;
            set => _client = value;
        }


        public int MyId => _myId;
        public bool Localhost => localhost;


        public bool IsMyTurn()
        {
            return _myId == _playerTurnId;
        }

        public void InitEvents()
        {
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

            _client.OnMessage += (sender, args) => { HandleMessage(args); };
        }

        private void HandleMessage(MessageEventArgs args)
        {
            Debug.Log(args.Data);
            JSONNode jsonNode = JSONNode.Parse(args.Data);
            string actionName = jsonNode["action"];

            if (actionName == NetworkAction.Connected.ToString()) networkEvents.onConnected?.Invoke(jsonNode);
            if (actionName == NetworkAction.EndGame.ToString()) networkEvents.onGameEnd?.Invoke(jsonNode);
            if (actionName == NetworkAction.StartGame.ToString()) networkEvents.onGameStart?.Invoke(jsonNode);
            if (actionName == NetworkAction.TurnSwitch.ToString()) networkEvents.onTurnSwitch?.Invoke(jsonNode);
            if (actionName == NetworkAction.PlaceStone.ToString()) networkEvents.onStonePlace?.Invoke(jsonNode);
        }
    }
}