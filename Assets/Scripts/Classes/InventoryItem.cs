using UnityEngine;
using System.Collections;

public class InventoryItem : ScriptableObject
{
    public Sprite Sprite;
    public Vector3 Scale;
    public string ItemName;
    public int Cost;
    public int Strength;
    public int Defense;


    /////////////////////////////////////// new section end
    public ItemType type;

    public enum ItemType
    {
        SWORD,
        AXE,
        SPEAR,
        BOW,
        POTION
    }
}
