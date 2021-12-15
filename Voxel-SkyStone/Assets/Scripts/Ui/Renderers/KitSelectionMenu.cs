using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class KitSelectionMenu : MonoBehaviour
{
    [SerializeField] private StonesContainer stonesContainer;
    [SerializeField] private KitData kit;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject selectablePrefab;
    [SerializeField] private Text priceText;
    [SerializeField] private float columnSize = 2.166637F;
    [SerializeField] private GameObject menu;
    
    public UnityEvent onKitValid = new UnityEvent();
    public UnityEvent onKitInvalid = new UnityEvent();

    private Vector3 _menuStartPos;
    private bool _locked;
    private bool _isValid = false;
    private Dictionary<string, SelectableKitItem> _selectableKitItems = new Dictionary<string, SelectableKitItem>();
    
    private void Awake()
    {
        if (!selectablePrefab.HasComponent<SelectableKitItem>())
        {
            throw new Exception("The selectable needs to have the SelectableKitItem");
        }

        _menuStartPos = menu.transform.position;
    }

    private void Start()
    {
        string[] stoneNames = stonesContainer.GetStoneNames();
        foreach (var stoneName in stoneNames)
        {
            GameObject image = Instantiate(selectablePrefab, content.transform, false);
            SelectableKitItem item = image.GetComponent<SelectableKitItem>();
            _selectableKitItems.Add(stoneName, item);

            item.Render(stonesContainer.GetStoneByName(stoneName));
            if (kit.GetStones().Contains(stoneName)) item.Select();

            image.GetOrAddComponent<UIRaycastEvents>().MouseClick.AddListener(() =>
            {
                if (_locked) return;
                if (item.IsSelected)
                {
                    item.Deselect();
                    kit.RemoveStone(stoneName);
                }
                else
                {
                    if (!CanSelect(stonesContainer.GetStoneByName(stoneName))) return;
                    item.Select();
                    kit.AddStone(stoneName);
                }

                Validate();
                RenderPoints();
            });
        }

        Validate();
        RenderPoints();
    }

    private void Update()
    {
        Vector3 currentPos = menu.transform.position;
        currentPos.y -= Input.GetAxis("Mouse ScrollWheel");
        currentPos.y = Mathf.Clamp(currentPos.y, _menuStartPos.y, _menuStartPos.y + columnSize * (stonesContainer.GetStoneNames().Length-4f)/4);
        menu.transform.position = currentPos;
    }

    public void Ready()
    {
        NetworkSendHandler sendHandler = FindObjectOfType<NetworkSendHandler>();
        sendHandler.SendReady(kit.GetStones());
    }

    public void Unready()
    {
        NetworkSendHandler sendHandler = FindObjectOfType<NetworkSendHandler>();
        sendHandler.SendUnready();
    }

    public void SelectRandomKit()
    {
        if (_locked) return;
        Validate();
        kit.Clear();
        List<string> stones = new List<string>(stonesContainer.GetStoneNames());
        while (stones.Count > 0)
        {
            int randomIndex = Random.Range(0, stones.Count);
            kit.AddStone(stones[randomIndex]);
            _selectableKitItems[stones[randomIndex]].Select();
            stones.RemoveAt(randomIndex);
            stones.RemoveAll(stone => !CanSelect(stonesContainer.GetStoneByName(stone)));
        }

        RenderPoints();
        Validate();
    }

    private void RenderPoints()
    {
        int price = 0;
        foreach (var stone in kit.GetStones()) price += stonesContainer.GetStoneByName(stone).Price;
        priceText.text = (20 - price).ToString() + "/20";
    }

    private bool CanSelect(StoneData stoneData)
    {
        int pointsLeft = 20;
        foreach (var stone in kit.GetStones()) pointsLeft -= stonesContainer.GetStoneByName(stone).Price;
        Debug.Log(pointsLeft);
        Debug.Log(stoneData.Price);
        return ((pointsLeft) >= stoneData.Price);
    }

    private void Validate()
    {
        if (kit.GetStones().Length < 5)
        {
            if (_isValid) onKitInvalid?.Invoke();
            _isValid = false;
        }
        else
        {
            if (!_isValid) onKitValid?.Invoke();
            _isValid = true;
        }
    }

    public bool Locked
    {
        get => _locked;
        set => _locked = value;
    }
}