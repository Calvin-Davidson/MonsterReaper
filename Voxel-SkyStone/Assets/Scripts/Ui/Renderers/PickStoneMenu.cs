using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.UI;

public class PickStoneMenu : MonoBehaviour
{
    [SerializeField] private KitData kit;
    [SerializeField] private StonesContainer stonesContainer;
    [SerializeField] private TileSelection tileSelection;
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float columnSize;

    private SelectableKitItem _selectedItem;
    private List<string> _placedStones = new List<string>();
    private Dictionary<string, GameObject> _items = new Dictionary<string, GameObject>();
    private Vector2 _menuStartPos;

    private void Awake()
    {
        _menuStartPos = menuContainer.transform.position;

        kit = Instantiate(kit);
        
        string[] stoneNames = kit.GetStones();

        SelectableKitItem[] items = menuContainer.GetComponentsInChildren<SelectableKitItem>();
        for (var i = 0; i < Mathf.Min(items.Length, stoneNames.Length); i++)
        {
            string stoneName = stoneNames[i];
            items[i].Render(stonesContainer.GetStoneByName(stoneNames[i]));

            int index = i;
            items[index].GetOrAddComponent<MouseEvents>().onMouseClick.AddListener(() =>
            {
                if (!items[index].IsSelected)
                {
                    if (_selectedItem != null) _selectedItem.Deselect();
                    SelectStone(stoneName, items[index]);
                    items[index].Select();
                }
            });
        }
        
        tileSelection.onStonePlace.AddListener((placedStone) => UseSelectedItem());
    }
    
    private void Update()
    {
        Vector3 currentPos = menuContainer.transform.position;
        int columns = Mathf.CeilToInt((kit.GetStones().Length - 2));
        if (columns <= 0) return;
        
        currentPos.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        currentPos.y = Mathf.Clamp(currentPos.y, _menuStartPos.y, _menuStartPos.y + columnSize * columns);
        menuContainer.transform.position = currentPos;
    }
    
    private void SelectStone(string stone, SelectableKitItem item)
    {
        if (!NetworkClient.Instance.networkData.IsMyTurn()) return;
        tileSelection.SelectedStone = stonesContainer.GetStoneByName(stone);
        _selectedItem = item;
    }

    private void UseSelectedItem()
    {
        _selectedItem.gameObject.RemoveComponent<MouseEvents>();
    }

    private void Start()
    {
        tileSelection.onStonePlace.AddListener((stoneName) =>
        {
            Remove(stoneName);
            Render();
        });
    }

    private void Render()
    {
        foreach (var keyValuePair in _items)
        {
            if (!_placedStones.Contains(keyValuePair.Key)) continue;
            ColorUtility.TryParseHtmlString("#4D4D4D", out var color);
            keyValuePair.Value.GetComponent<RawImage>().color = color;
        }
    }

    public void Remove(String stoneName)
    {
        _placedStones.Add(stoneName);
    }
}
