using System.Collections;
using System.Collections.Generic;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIRaycastEvents))]
public class SelectableKitItem : MonoBehaviour
{
    [SerializeField] private GameObject kitObject;
    [SerializeField] private GameObject selectedObject;

    private bool _isSelected = false;

    public void Select()
    {
        _isSelected = true;
        selectedObject.SetActive(true);   
    }

    public void Deselect()
    {
        _isSelected = false;
        selectedObject.SetActive(false);
    }

    public void Render(StoneData data)
    {
        kitObject.GetOrAddComponent<RawImage>().texture = data.Texture;
    }

    public bool IsSelected => _isSelected;
}
