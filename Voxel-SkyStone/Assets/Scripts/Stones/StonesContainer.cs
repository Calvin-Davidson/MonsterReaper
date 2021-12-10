using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]  // Only 1 instance should be made!
public class StonesContainer : ScriptableObject
{
  private List<StoneData> _stones = new List<StoneData>();
  
  public string[] GetStoneNames()
  {
    return _stones.Select(data => data.Name).ToArray();
  }



#if UNITY_EDITOR
  public void CreateStone()
  {
    StoneData instance = ScriptableObject.CreateInstance<StoneData>();

    instance.SetName(GUID.Generate().ToString());

    _stones.Add(instance);

    AssetDatabase.AddObjectToAsset(instance, this);
    AssetDatabase.SaveAssets();
  }

  public void DeleteStone(string stoneName)
  {
    var stone = _stones.FirstOrDefault(data => data.Name == stoneName);
    if (stone == null) return;

    _stones.Remove(stone);
    AssetDatabase.RemoveObjectFromAsset(stone);
    AssetDatabase.SaveAssets();
  }
#endif
}
