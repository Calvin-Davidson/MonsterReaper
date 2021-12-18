using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private int _selectedItemSlot = 0;
    private List<SelectableKitItem> _items = new List<SelectableKitItem>();
    private Vector2 _menuStartPos;

    private void Awake()
    {
        _menuStartPos = menuContainer.transform.position;

        kit = Instantiate(kit);
        
        string[] stoneNames = kit.GetStones();

        _items = menuContainer.GetComponentsInChildren<SelectableKitItem>().ToList();
        for (var i = 0; i < Mathf.Min(_items.Count, stoneNames.Length); i++)
        {
            string stoneName = stoneNames[i];
            _items[i].Render(stonesContainer.GetStoneByName(stoneNames[i]));

            int index = i;
            _items[index].GetOrAddComponent<MouseEvents>().onMouseClick.AddListener(() =>
            {
                if (!_items[index].IsSelected)
                {
                    if (_selectedItem != null) _selectedItem.Deselect();
                    SelectStone(stoneName, _items[index]);
                    _items[index].Select();
                }
            });
        }
        
        tileSelection.onStonePlace.AddListener((placedStone) => UseSelectedItem());
    }

    public void NextItem()
    {
        _selectedItemSlot += 1;
        _selectedItemSlot = Mathf.Clamp(_selectedItemSlot, 0, _items.Count);
        _selectedItem = _items[_selectedItemSlot];
    }

    public void PreviousItem()
    {
        _selectedItemSlot -= 1;
        _selectedItemSlot = Mathf.Clamp(_selectedItemSlot, 0, _items.Count);
        _selectedItem = _items[_selectedItemSlot];
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
        tileSelection.SelectedStone = stonesContainer.GetStoneByName(stone);
        _selectedItem = item;
    }

    private void UseSelectedItem()
    {
        _selectedItem.Deselect();
        _items.Remove(_selectedItem);
        Destroy(_selectedItem.gameObject);
        PreviousItem();
        Render();
    }

    private void Start()
    {
        tileSelection.onStonePlace.AddListener((stoneName) =>
        {
            Render();
        });
    }

    private void Render()
    {
        // todo rotation table rendering magic.
        
        
    }
}
