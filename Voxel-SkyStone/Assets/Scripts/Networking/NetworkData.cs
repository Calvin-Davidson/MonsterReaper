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
        
        public bool IsMyTurn()
        {
            return _myId == _playerTurnId;
        }
    
        public int MyId
        {
            get => _myId;
            set => _myId = value;
        }

        public int PlayerTurnId
        {
            get => _playerTurnId;
            set => _playerTurnId = value;
        }

        public WebSocket Client
        {
            get => _client;
            set => _client = value;
        }


        private void Connect()
        {
            var tryConnect = TryConnect();
        }
            
        private IEnumerator TryConnect()
        {
            Client = localhost ? new WebSocket("ws://localhost:80") : new WebSocket("ws://voxel-relay.herokuapp.com/");
            Client.ConnectAsync();

            float waitedTime = 0;
            while (waitedTime < 10)
            {
                waitedTime += Time.deltaTime;
                if (Client.IsAlive) break;
                yield return null;
            }

            if (Client.IsAlive)
            {
                onConnectionSuccessful?.Invoke();
            }
            else
            {
                onConnectionFail?.Invoke();
            }
        }
    }
}
