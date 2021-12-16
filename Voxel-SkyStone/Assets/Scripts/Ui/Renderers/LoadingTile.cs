using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTile : MonoBehaviour
{
    [SerializeField] private MeshRenderer frontRenderer;
    [SerializeField] private MeshRenderer backRenderer;
    [SerializeField] private StonesContainer stones;
    private static readonly int EmissionMap = Shader.PropertyToID("_EmissionMap");

    private void Awake()
    {
        ChangeFrontTexture();
        ChangeBackTexture();
    }

    private void ChangeFrontTexture()
    {
        Texture texture = stones.GetRandom().Texture;
        frontRenderer.material.mainTexture = texture;
        frontRenderer.material.SetTexture (EmissionMap, texture);
    }

    private void ChangeBackTexture()
    {
        Texture texture = stones.GetRandom().Texture;
        backRenderer.material.mainTexture = texture;
        backRenderer.material.SetTexture (EmissionMap, texture);
    }
}
