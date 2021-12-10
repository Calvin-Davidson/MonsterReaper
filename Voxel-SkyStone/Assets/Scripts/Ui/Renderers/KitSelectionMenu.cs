using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.UI;

public class KitSelectionMenu : MonoBehaviour
{
    [SerializeField] private StonesContainer stonesContainer;
    [SerializeField] private GameObject content;
    [SerializeField] private RawImage[] selected;

    private int _selectedSlot = 0;

    private void Awake()
    {
        for (var i = 0; i < selected.Length; i++)
        {
            var slot = i;
            selected[i].GetOrAddComponent<UIRaycastEvents>().MouseClick.AddListener(() => _selectedSlot = slot);
        }
    }

    private void Start()
    {
        string[] stoneNames = stonesContainer.GetStoneNames();
        foreach (var stoneName in stoneNames)
        {
            GameObject image = new GameObject(stoneName, typeof(CanvasRenderer), typeof(RawImage), typeof(UIRaycastEvents));
            image.GetComponent<RawImage>().texture = stonesContainer.GetStoneByName(stoneName).Texture;
            image.GetComponent<UIRaycastEvents>().MouseClick.AddListener(() => Select(image.GetComponent<RawImage>().texture, _selectedSlot));
    
            image.transform.parent = content.transform;
        }
    }

    
    private void Select(Texture texture, int slot)
    {
        selected[slot].texture = texture;
    }
}
