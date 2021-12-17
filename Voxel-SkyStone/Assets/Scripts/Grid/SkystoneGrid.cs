using System.Collections.Generic;
using System.Linq;
using Grid;
using Toolbox.MethodExtensions;
using UnityEngine;

public class SkystoneGrid : MonoBehaviour
{
    [SerializeField] private List<Stone> stones = new List<Stone>();

    public List<Stone> Stones => stones;

    [SerializeField] private int gridSize = 3;
    [SerializeField] private float tileSize = 1;
    [SerializeField] private float tileGap = 0;

    [SerializeField] private GameObject tileGameObject;
    [SerializeField] private GameObject overlayGameObject;

    [SerializeField] private GameObject[] arrowObjects;
    
    private void Start()
    {
        // if (Camera.main is { })
        // {
        //     Transform cam = Camera.main.gameObject.transform;
        //     cam.position = new Vector3(gridSize / 2, gridSize, -gridSize);
        //     cam.Rotate(new Vector3(65, 0, 0));
        // }

        // InstantiateTiles();
        // InstantiateOverlay();
    }
    private void InstantiateTiles()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject stoneObject = Instantiate(tileGameObject, new Vector3(x * (1 + tileGap), 0, -y * (1 + tileGap)), Quaternion.identity);
                Stone stone = stoneObject.GetOrAddComponent<Stone>();
                stone.GridIndex = x + y*gridSize;
                stoneObject.transform.parent = gameObject.transform;
                var tileCollider = stoneObject.AddComponent<BoxCollider>();
                tileCollider.size = new Vector3(tileSize, 0.1f, tileSize);
                
                stones.Add(stone);
            }
        } 
    }
    private void InstantiateOverlay()
    {
        for (int y = 0; y < gridSize + 1; y++)
        {
            for (int x = 0; x < gridSize + 1; x++)
            {
                GameObject betweenTile = Instantiate(overlayGameObject, new Vector3((x - tileSize/2) * (1 + tileGap), 0, (-y + tileSize/2) * (1 + tileGap)), Quaternion.identity);
                betweenTile.transform.parent = gameObject.transform;
            }
        }
    }

    public bool CheckGameEnd()
    {
        if (stones.Exists(stone => stone.StoneData == null)) return false;
        return true;
    }

    public VictoryState GetWinner()
    {
        int opponentStonesCount = (stones.Count(stone => stone.TeamSide != NetworkClient.Instance.networkData.MyId));
        int myStonesCount = (stones.Count(stone => stone.TeamSide == NetworkClient.Instance.networkData.MyId));
        if (myStonesCount > opponentStonesCount) return VictoryState.Winner;
        if (myStonesCount < opponentStonesCount) return VictoryState.Loser;
        return VictoryState.Draw;
    }

    public Stone GetStoneAbove(Stone stone)
    {
        int index = stones.IndexOf(stone) - gridSize;
        return index < 0 || index >= stones.Count ? null : stones[index];
    }

    public Stone GetStoneUnder(Stone stone)
    {
        int index = stones.IndexOf(stone) + gridSize;
        return index < 0 || index >= stones.Count ? null : stones[index];
    }

    public Stone GetStoneRight(Stone stone)
    {
        int index = stones.IndexOf(stone) + 1;
        return index < 0 || index >= stones.Count || (index) % 3 == 0 ? null : stones[index];
    }

    public Stone GetStoneLeft(Stone stone)
    {
        int index = stones.IndexOf(stone) - 1;
        return index < 0 || index >= stones.Count || stones.IndexOf(stone) % 3 == 0 ? null : stones[index];
    }
}
