using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class StoneData : ScriptableObject
{
    [SerializeReference] private new string name;
    [SerializeReference] private GameObject tileObject;
    [SerializeReference] private Texture texture;
    [SerializeReference] private int price;
    [SerializeReference] private int topDamage;
    [SerializeReference] private int rightDamage;
    [SerializeReference] private int bottomDamage;
    [SerializeReference] private int leftDamage;

    public string Name => name;

    public GameObject TileObject => tileObject;

    public int TopDamage => topDamage;

    public int RightDamage => rightDamage;

    public int BottomDamage => bottomDamage;

    public int LeftDamage => leftDamage;

    public Texture Texture => texture;

    public int Price => price;
#if UNITY_EDITOR
    public void SetName(string newName)
    {
        this.name = newName;
        EditorUtility.SetDirty(this);
    }

    public void SetImage(GameObject newImage)
    {
        this.tileObject = newImage;
        EditorUtility.SetDirty(this);
    }

    public void SetTopDamage(int newDamage)
    {
        this.topDamage = newDamage;
        EditorUtility.SetDirty(this);
    }

    public void SetBottomDamage(int newDamage)
    {
        this.bottomDamage = newDamage;
        EditorUtility.SetDirty(this);
    }

    public void SetLeftDamage(int newDamage)
    {
        this.leftDamage = newDamage;
        EditorUtility.SetDirty(this);
    }

    public void SetRightDamage(int newDamage)
    {
        this.rightDamage = newDamage;
        EditorUtility.SetDirty(this);
    }

    public void SetTexture(Texture newTexture)
    {
        this.texture = newTexture;
        EditorUtility.SetDirty(this);
    }
    public void SetPrice(int newPrice)
    {
        this.price = newPrice;
        EditorUtility.SetDirty(this);
    }
    
#endif
}
