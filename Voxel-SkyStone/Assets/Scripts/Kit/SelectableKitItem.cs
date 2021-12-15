using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIRaycastEvents))]
public class SelectableKitItem : MonoBehaviour
{
    [SerializeField] private MeshRenderer outlineObject;
    [SerializeField] private MeshRenderer textureObject;
    [SerializeField] private TextMesh priceText;

    private bool _isSelected = false;

    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = outlineObject.material.color;
    }

    public void Select()
    {
        outlineObject.material.color = Color.red;
        _isSelected = true;
    }

    public void Deselect()
    {
        outlineObject.material.color = _defaultColor;
        _isSelected = false;
    }

    public void Render(StoneData data)
    {
        textureObject.material.mainTexture = data.Texture;
        priceText.text = data.Price.ToString();
    }

    public bool IsSelected => _isSelected;
}
