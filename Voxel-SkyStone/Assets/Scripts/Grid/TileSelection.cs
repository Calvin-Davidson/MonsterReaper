using Grid;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    private SkystoneGrid _skyStoneGrid;

    [SerializeField] private int playerTeam;
    [SerializeField] private StoneData selectedTile;

    private void Awake()
    {
        _skyStoneGrid = FindObjectOfType<SkystoneGrid>();
    }

    private void Update()
    {
        // if (NetworkClient.Instance == null) return; // temp
        // if (!NetworkClient.Instance.networkData.IsMyTurn()) return;
        //
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.gameObject.TryGetComponent<Stone>(out Stone clickedStone);

                clickedStone.TeamSide = playerTeam;
                clickedStone.StoneData = selectedTile;
        //        FindObjectOfType<NetworkSendHandler>().PlaceStone(selectedTile.Name, _skyStoneGrid.Stones.IndexOf(clickedStone));
                
                clickedStone.UpdateStoneData();
            }
        }
    }
}
