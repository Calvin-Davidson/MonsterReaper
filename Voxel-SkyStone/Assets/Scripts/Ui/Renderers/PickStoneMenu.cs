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
    [SerializeField] private GameObject kitItemContainer;
    [SerializeField] private TileSelection tileSelection;

    private GameObject _selectedItem;
    private List<string> _placedStones = new List<string>();
    private Dictionary<string, GameObject> _items = new Dictionary<string, GameObject>();
    private void Awake()
    {
        kit = Instantiate(kit);
        foreach (var stone in kit.GetStones())
        {
            GameObject item = new GameObject(stone, typeof(RawImage), typeof(UIRaycastEvents));
            item.transform.parent = kitItemContainer.transform;
            item.GetComponent<RawImage>().texture = stonesContainer.GetStoneByName(stone).Texture;
            _items.Add(stone, item);

            item.GetComponent<UIRaycastEvents>().MouseClick.AddListener(() => SelectStone(stone, item));
        }
        tileSelection.onStonePlace.AddListener((placedStone) => UseSelectedItem());
    }

    private void SelectStone(string stone, GameObject item)
    {
        if (!NetworkClient.Instance.networkData.IsMyTurn()) return;
        tileSelection.SelectedStone = stonesContainer.GetStoneByName(stone);
        _selectedItem = item;
    }

    private void UseSelectedItem()
    {
        _selectedItem.RemoveComponent<UIRaycastEvents>();
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
