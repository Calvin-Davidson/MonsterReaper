using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//[CreateAssetMenu]  // Only 1 instance should be made!
public class StonesContainer : ScriptableObject
{
  [SerializeReference] private List<StoneData> stones = new List<StoneData>();
  
  public string[] GetStoneNames()
  {
    return stones.Select(data => data.Name).ToArray();
  }

  public StoneData GetStoneByName(string stoneName)
  {
    return stones.FirstOrDefault(data => data.Name == stoneName);
  }



#if UNITY_EDITOR
  public void CreateStone()
  {
    StoneData instance = ScriptableObject.CreateInstance<StoneData>();

    instance.SetName(GUID.Generate().ToString());

    stones.Add(instance);

    AssetDatabase.AddObjectToAsset(instance, this);
    AssetDatabase.SaveAssets();
  }

  public void DeleteStone(string stoneName)
  {
    var stone = stones.FirstOrDefault(data => data.Name == stoneName);
    if (stone == null) return;

    stones.Remove(stone);
    AssetDatabase.RemoveObjectFromAsset(stone);
    AssetDatabase.SaveAssets();
  }
#endif
}
