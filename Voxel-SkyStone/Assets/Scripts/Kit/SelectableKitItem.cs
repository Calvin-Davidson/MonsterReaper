using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MouseEvents))]
public class SelectableKitItem : MonoBehaviour
{
    [SerializeField] private MeshRenderer outlineObject;
    [SerializeField] private MeshRenderer textureObject;
    [SerializeField] private TextMesh priceText;
    
    [SerializeField] private GameObject topArrowContainer;
    [SerializeField] private GameObject rightArrowContainer;
    [SerializeField] private GameObject bottomArrowContainer;
    [SerializeField] private GameObject leftArrowContainer;

    [SerializeField] private GameObject[] arrows;
    
    private bool _isSelected = false;

    private Color _defaultColor;
    private static readonly int EmissionMap = Shader.PropertyToID("_EmissionMap");

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
        Material material = textureObject.material;
        
        material.mainTexture = data.Texture;
        priceText.text = data.Price.ToString();
        
        material.EnableKeyword("_EMISSION");
        material.SetTexture (EmissionMap, data.Texture);
        
        SpawnArrows(data);
    }
    
    private void SpawnArrows(StoneData stoneData)
    {
        if (stoneData.TopDamage > 0) SpawnArrow(topArrowContainer, arrows[stoneData.TopDamage-1]);
        if (stoneData.RightDamage > 0) SpawnArrow(rightArrowContainer, arrows[stoneData.RightDamage-1]);
        if (stoneData.BottomDamage > 0) SpawnArrow(bottomArrowContainer, arrows[stoneData.BottomDamage-1]);
        if (stoneData.LeftDamage > 0) SpawnArrow(leftArrowContainer, arrows[stoneData.LeftDamage-1]);
    }
    
    private void SpawnArrow(GameObject container, GameObject arrow)
    {
        Instantiate(arrow, container.transform, false);
    }



    public bool IsSelected => _isSelected;
}
