using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Networking;
using Toolbox.MethodExtensions;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    private SkystoneGrid _skystoneGrid;

    [SerializeField] private int playerTeam;
    [SerializeField] private TileScriptableObject selectedTile;
    [SerializeField] private NetworkData networkData;
    
    private void Awake()
    {
        _skystoneGrid = FindObjectOfType<SkystoneGrid>();
    }

    private void Update()
    {
        if (!networkData.IsMyTurn()) return;
        if (!Input.GetMouseButtonDown(0)) return;
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (!hit.transform.gameObject.TryGetComponent(out Stone clickedStone)) return;

            clickedStone.TeamSide = networkData.MyId;
            clickedStone.StoneData = selectedTile;
        }
    }
}
