using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

namespace Networking
{
    [System.Serializable]
    public class NetworkEventContainer
    {
        public UnityEvent<JSONNode> onConnected = new UnityEvent<JSONNode>();
        public UnityEvent<JSONNode> onGameStart = new UnityEvent<JSONNode>();
        public UnityEvent<JSONNode> onGameEnd = new UnityEvent<JSONNode>();
        public UnityEvent<JSONNode> onTurnSwitch = new UnityEvent<JSONNode>();
        public UnityEvent<JSONNode> onStonePlace = new UnityEvent<JSONNode>();
        public UnityEvent<JSONNode> onOpponentLeave = new UnityEvent<JSONNode>();
        public UnityEvent<JSONNode> onGameFound = new UnityEvent<JSONNode>();
    }
}