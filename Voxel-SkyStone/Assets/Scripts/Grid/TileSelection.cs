using Grid;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    private SkystoneGrid _skystoneGrid;

    [SerializeField] private int playerTeam;
    [SerializeField] private TileScriptableObject selectedTile;

    private void Awake()
    {
        _skystoneGrid = FindObjectOfType<SkystoneGrid>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                hit.transform.gameObject.TryGetComponent<Stone>(out Stone clickedStone);

                clickedStone.TeamSide = playerTeam;
                clickedStone.StoneData = selectedTile;
                
                clickedStone.UpdateStoneData();
            }
        }
    }
}
