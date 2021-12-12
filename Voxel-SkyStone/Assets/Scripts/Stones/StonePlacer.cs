using SimpleJSON;
using UnityEngine;

namespace Stones
{
    public class StonePlacer : MonoBehaviour
    {
        [SerializeField] private SkystoneGrid grid;
        [SerializeField] private StonesContainer stones;
    
    
        public void PlaceStone(JSONNode jsonNode)
        {
            string placedStoneName = jsonNode["placedStone"];
            int tileId = jsonNode["tileId"];
            int placedBy = jsonNode["playerBy"];

            grid.Stones[tileId].StoneData = stones.GetStoneByName(placedStoneName);
            grid.Stones[tileId].TeamSide = placedBy;
        }
    }
}
