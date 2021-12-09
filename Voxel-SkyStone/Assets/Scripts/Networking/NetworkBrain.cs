using System;
using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

namespace Networking
{
    public class NetworkBrain : MonoBehaviour
    {
        [SerializeField] private bool localHost;
        [SerializeField] private bool connectOnAwake = false;
        [SerializeField] private NetworkData networkData;

        public UnityEvent onConnectionOpen = new UnityEvent();
        public UnityEvent onConnectionFail = new UnityEvent();
        public UnityEvent onConnectionSuccessful = new UnityEvent();

        public UnityEvent onGameStart = new UnityEvent();

        private void Awake()
        {
            if (connectOnAwake) Connect();

            if (networkData.Client != null)
            {
                networkData.Client.OnOpen += OnConnectionOpen;
                networkData.Client.OnMessage += OnMessage;
            }
        }

        private void OnDisable()
        {
            networkData.Client.OnOpen -= OnConnectionOpen;
            networkData.Client.OnMessage -= OnMessage;
        }

        public void Connect()
        {
            StartCoroutine(TryConnect());
        }

        private IEnumerator TryConnect()
        {
            networkData.Client = localHost ? new WebSocket("ws://localhost:80") : new WebSocket("ws://voxel-relay.herokuapp.com/");
            
            networkData.Client.OnOpen += OnConnectionOpen;
            networkData.Client.OnMessage += OnMessage;
            
            networkData.Client.ConnectAsync();

            float waitedTime = 0;
            while (waitedTime < 10)
            {
                waitedTime += Time.deltaTime;
                if (networkData.Client.IsAlive) break;
                yield return null;
            }

            if (networkData.Client.IsAlive)
            {
                print("successfully connected");
                onConnectionSuccessful?.Invoke();
            }
            else
            {
                print("something went wrong while connecting");
                onConnectionFail?.Invoke();
            }
        }

        private void OnConnectionOpen(object sender, EventArgs args)
        {
            Debug.Log("connection opened");
        }

        private void OnMessage(object sender, MessageEventArgs args)
        {
            JSONNode jsonNode = JSONNode.Parse(args.Data);
            if (jsonNode["action"] == NetworkAction.Connected.ToString())
            {
                networkData.MyId = jsonNode["playerId"];
            }

            if (jsonNode["action"] == NetworkAction.StartGame.ToString())
            {
                networkData.PlayerTurnId = jsonNode["StartingPlayerId"];
                onGameStart?.Invoke();
            }
        }
    }
}