using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTile : MonoBehaviour
{
    [SerializeField] private MeshRenderer frontRenderer;
    [SerializeField] private MeshRenderer backRenderer;
    [SerializeField] private StonesContainer stones;

    private void Awake()
    {
        ChangeFrontTexture();
        ChangeBackTexture();
    }

    private void ChangeFrontTexture()
    {
        Debug.Log("front texture");
        frontRenderer.material.mainTexture = stones.GetRandom().Texture;
    }

    private void ChangeBackTexture()
    {
        Debug.Log("back texture");
        backRenderer.material.mainTexture = stones.GetRandom().Texture;
    }
}
