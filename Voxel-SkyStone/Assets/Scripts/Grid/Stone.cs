using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.Events;

public class Stone : MonoBehaviour
{
    [SerializeField] private int teamSide;
    [SerializeField] private TileScriptableObject stoneData;

    private int gridIndex;

    public int GridIndex
    {
        get => gridIndex;
        set => gridIndex = value;
    }
}
