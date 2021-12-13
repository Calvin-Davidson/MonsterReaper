using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Networking
{
    public enum NetworkAction
    {
        StartGame,
        EndGame,
        TurnSwitch,
        PlaceStone,
        OpponentLeave,
        Ready,
        Unready,
        GameFound,
        JoinLobby,
    }
}