using UnityEngine;

//[CreateAssetMenu(fileName = "NetworkData", menuName = "ScriptableObjects/NetworkData")]
namespace Networking
{
    public class NetworkData : ScriptableObject
    {
        private int _myId;
        private int _playerTurnId;

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
    }
}
