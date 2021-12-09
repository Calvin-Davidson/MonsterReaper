using System.Collections.Generic;
using Toolbox.MethodExtensions;
using UnityEngine;

public class SkystoneGrid : MonoBehaviour
{
    private List<Stone> _stones = new List<Stone>();

    public List<Stone> Stones => _stones;

    [SerializeField] private int gridSize = 3;
    [SerializeField] private float tileSize = 1;
    [SerializeField] private float tileGap = 0;

    [SerializeField] private GameObject tileGameObject;
    [SerializeField] private GameObject overlayGameObject;
    
    private void Start()
    {
        if (Camera.main is { })
        {
            Transform cam = Camera.main.gameObject.transform;
            cam.position = new Vector3(gridSize / 2, gridSize, -gridSize);
            cam.Rotate(new Vector3(65, 0, 0));
        }

        InstantiateTiles();
        InstantiateOverlay();
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
                
                _stones.Add(stone);
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
}
