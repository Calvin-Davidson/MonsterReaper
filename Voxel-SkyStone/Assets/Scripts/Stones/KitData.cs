using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KitData : ScriptableObject
{
    [SerializeField] private List<string> stones = new List<string>();

    public bool AddStone(string stoneName)
    {
        if (stones.Contains(stoneName)) return false;
        stones.Add(stoneName);
        return true;
    }

    public bool RemoveStone(string stoneName)
    {
        if (!stones.Contains(stoneName)) return false;
        stones.Remove(stoneName);
        return true;
    }

    public string[] GetStones()
    {
        return stones.ToArray();
    }

    public bool HasStone(string stoneName)
    {
        return stones.Contains(stoneName);
    }
}
