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
                    networkData.MyId = jsonNode["playerId"];
                    Debug.Log("received connected");
                }
                if (jsonNode["action"] == NetworkAction.StartGame.ToString())
                {
                    networkData.PlayerTurnId = jsonNode["StartingPlayerId"];
                    Debug.Log("Changing player's turn");
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
}
