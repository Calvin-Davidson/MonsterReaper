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
    [SerializeField] private Text priceText;
    [SerializeField] private float columnSize = 2.166637F;
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private GameObject menuSectionPrefab;
    [SerializeField] private List<GameObject> kitContainers = new List<GameObject>();

    public UnityEvent onKitValid = new UnityEvent();
    public UnityEvent onKitInvalid = new UnityEvent();

    private Vector3 _menuStartPos;
    private bool _locked;
    private bool _isValid = false;
    private Dictionary<string, SelectableKitItem> _selectableKitItems = new Dictionary<string, SelectableKitItem>();
    
    private void Awake()
    {
        _menuStartPos = menuContainer.transform.position;
    }

    private void Start()
    {
        SpawnSections();
        string[] stoneNames = stonesContainer.GetStoneNames();

        SelectableKitItem[] items = menuContainer.GetComponentsInChildren<SelectableKitItem>();
        Debug.Log(Mathf.Min(items.Length, stoneNames.Length));
        for (var i = 0; i < Mathf.Min(items.Length, stoneNames.Length); i++)
        {
            string stoneName = stoneNames[i];
            _selectableKitItems.Add(stoneNames[i], items[i]);
            items[i].Render(stonesContainer.GetStoneByName(stoneNames[i]));
            if (kit.GetStones().Contains(stoneNames[i])) items[i].Select();

            int index = i;
            items[index].GetOrAddComponent<MouseEvents>().onMouseClick.AddListener(() =>
            {
                if (_locked) return;
                if (items[index].IsSelected)
                {
                    items[index].Deselect();
                    kit.RemoveStone(stoneName);
                }
                else
                {
                    if (!CanSelect(stonesContainer.GetStoneByName(stoneName))) return;
                    items[index].Select();
                    kit.AddStone(stoneName);
                }

                Validate();
                RenderPoints();
            });
        }

        for (var i = items.Length - 1; i >= Math.Abs(items.Length - stoneNames.Length - 2); i--)
        {
            Destroy(items[i].gameObject);
        }

        Validate();
        RenderPoints();
    }

    private void SpawnSections()
    {
        int columns = Mathf.CeilToInt((stonesContainer.GetStoneNames().Length - 8) / 4f);
        for (int i = 0; i < columns; i++)
        {
            GameObject section = Instantiate(menuSectionPrefab, menuContainer.transform, false);
            Vector3 currentPos = section.transform.position;
            currentPos.y = -71.93261f - 14.39739f * (i + 1) + menuContainer.transform.position.y;
            section.transform.position = currentPos;
            kitContainers.Add(section);
        }
    }

    private void Update()
    {
        Vector3 currentPos = menuContainer.transform.position;
        int columns = Mathf.CeilToInt((stonesContainer.GetStoneNames().Length - 8) / 4f);
        currentPos.y -= Input.GetAxis("Mouse ScrollWheel") * 3;
        currentPos.y = Mathf.Clamp(currentPos.y, _menuStartPos.y, _menuStartPos.y + columnSize * columns);
        menuContainer.transform.position = currentPos;
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