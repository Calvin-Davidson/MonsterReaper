using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class StonePlacer : MonoBehaviour
{
    [SerializeField] private SkystoneGrid grid;
    [SerializeField] private StonesContainer stones;
    
    
    public void PlaceStone(JSONNode jsonNode)
    {
        string placedStoneName = jsonNode["placedStone"];
        int tileId = jsonNode["tileId"];

        grid.Stones[tileId].StoneData = stones.GetStoneByName(placedStoneName);
    }
}
