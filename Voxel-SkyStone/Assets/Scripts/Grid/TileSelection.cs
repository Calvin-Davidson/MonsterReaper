using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox.MethodExtensions;
using UnityEngine;

public class TileSelection : MonoBehaviour
{
    private SkystoneGrid _skystoneGrid;

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
                
                Debug.Log(clickedStone.GridIndex);
            }
        }
    }
}
