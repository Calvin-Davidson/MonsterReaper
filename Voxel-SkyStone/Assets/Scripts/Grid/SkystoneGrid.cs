using System.Collections.Generic;
using UnityEngine;

public class SkystoneGrid : MonoBehaviour
{
    [SerializeField] private int gridSize = 3;
    [SerializeField] private float tileSize = 1;
    [SerializeField] private float tileGap = 0;

    [SerializeField] private GameObject tileGameObject;
    [SerializeField] private GameObject overlayGameObject;

    private BoxCollider _tileCollider;

    private void Awake()
    {
        Transform cam = Camera.main.gameObject.transform;
        cam.position = new Vector3(gridSize / 2, gridSize, -gridSize);
        cam.Rotate(new Vector3(65, 0, 0)); 
        
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject gridCollider = Instantiate(tileGameObject, new Vector3(x * (1 + tileGap), 0, -y * (1 + tileGap)), Quaternion.identity);
                gridCollider.transform.parent = gameObject.transform;
                _tileCollider = gridCollider.AddComponent<BoxCollider>();
                _tileCollider.size = new Vector3(tileSize, 0.3f, tileSize);
            }
        }

        for (int y = 0; y < gridSize + 1; y++)
        {
            for (int x = 0; x < gridSize + 1; x++)
            {
                Instantiate(overlayGameObject, new Vector3((x - tileSize/2) * (1 + tileGap), 0, (-y + tileSize/2) * (1 + tileGap)), Quaternion.identity);
            }
        }
    }
}
