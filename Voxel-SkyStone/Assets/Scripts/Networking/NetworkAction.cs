using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Networking
{
    public enum NetworkAction
    {
        Connected,
        StartGame,
        EndGame,
        TurnSwitch,
        PlaceStone,
        OpponentLeave,
        Ready,
        Unready
    }
}