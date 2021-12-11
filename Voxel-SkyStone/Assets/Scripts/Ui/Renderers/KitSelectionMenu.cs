using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KitSelectionMenu : MonoBehaviour
{
    [SerializeField] private StonesContainer stonesContainer;
    [SerializeField] private GameObject content;
    [SerializeField] private RawImage[] selected;
    [SerializeField] private bool locked;
    
    public UnityEvent onKitValid = new UnityEvent();
    public UnityEvent onKitInvalid = new UnityEvent();

    private int _selectedSlot = 0;
    private List<GameObject> _selectables = new List<GameObject>();
    private StoneData[] _selectedStones = new StoneData[5];
    private bool _isValid = false;
    
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
            GameObject image = new GameObject(stoneName, typeof(CanvasRenderer), typeof(RawImage),
                typeof(UIRaycastEvents));
            image.GetComponent<RawImage>().texture = stonesContainer.GetStoneByName(stoneName).Texture;
            image.GetComponent<UIRaycastEvents>().MouseClick.AddListener(() =>
            {
                if (locked) return;
                _selectedStones[_selectedSlot] = stonesContainer.GetStoneByName(stoneName);
                Select(image.GetComponent<RawImage>().texture, _selectedSlot);
                Validate();
            });

            image.transform.parent = content.transform;
            _selectables.Add(image);
        }
    }


    public void Ready()
    {
        NetworkSendHandler sendHandler = FindObjectOfType<NetworkSendHandler>();
        string item1 = _selectedStones[0] == null || String.IsNullOrEmpty(_selectedStones[0].Name) ? "empty" : _selectedStones[0].Name;
        string item2 = _selectedStones[1] == null || String.IsNullOrEmpty(_selectedStones[1].Name) ? "empty" : _selectedStones[1].Name;
        string item3 = _selectedStones[2] == null || String.IsNullOrEmpty(_selectedStones[2].Name) ? "empty" : _selectedStones[2].Name;
        string item4 = _selectedStones[3] == null || String.IsNullOrEmpty(_selectedStones[3].Name) ? "empty" : _selectedStones[3].Name;
        string item5 = _selectedStones[4] == null || String.IsNullOrEmpty(_selectedStones[4].Name) ? "empty" : _selectedStones[4].Name;
        sendHandler.SendReady(item1, item2, item3, item4, item5);
    }

    public void Unready()
    {
        NetworkSendHandler sendHandler = FindObjectOfType<NetworkSendHandler>();
        sendHandler.SendUnready();
    }
    
    private void Select(Texture texture, int slot)
    {
        selected[slot].texture = texture;
    }

    private void Validate()
    {
        if (_selectedStones.Count(data => data == null) > 0)
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
        get => locked;
        set => locked = value;
    }
}