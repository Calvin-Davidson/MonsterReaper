using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitSelectionMenuRenderer : MonoBehaviour
{
    [SerializeField] private StonesContainer stonesContainer;
    [SerializeField] private GameObject content;
    private void Start()
    {
        string[] stoneNames = stonesContainer.GetStoneNames();
        foreach (var stoneName in stoneNames)
        {
            GameObject image = new GameObject(stoneName, typeof(CanvasRenderer), typeof(RawImage));
            image.GetComponent<RawImage>().texture = stonesContainer.GetStoneByName(stoneName).Texture;
            image.transform.parent = content.transform;
        }
    }
}
