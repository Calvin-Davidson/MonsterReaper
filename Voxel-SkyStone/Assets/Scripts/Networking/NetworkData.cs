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
        
        public bool Localhost => localhost;


        public bool IsMyTurn()
        {
            return _myId == _playerTurnId;
        }
    }
}