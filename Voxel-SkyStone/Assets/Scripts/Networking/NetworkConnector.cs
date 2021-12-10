using System;
using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

namespace Networking
{
    public class NetworkConnector : MonoBehaviour
    {
        [SerializeField] private NetworkData networkData;

        public UnityEvent onConnectionFail = new UnityEvent();
        public UnityEvent onConnectionSuccessful = new UnityEvent();

        public void Connect()
        {
            StartCoroutine(TryConnect());
        }

        private IEnumerator TryConnect()
        {
            networkData.Client = networkData.Localhost
                ? new WebSocket("ws://localhost:80")
                : new WebSocket("ws://voxel-relay.herokuapp.com/");

            networkData.InitEvents();
            
            networkData.Client.ConnectAsync();

            float waitedTime = 0;
            while (waitedTime < 10)
            {
                waitedTime += Time.deltaTime;
                if (networkData.Client.IsAlive) break;
                yield return null;
            }

            if (networkData.Client.IsAlive)
                onConnectionSuccessful?.Invoke();
            else
                onConnectionFail?.Invoke();
        }
    }
}