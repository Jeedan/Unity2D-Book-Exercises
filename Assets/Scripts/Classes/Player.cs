using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public List<BaseAbility> abilities = new List<BaseAbility>();
    public string[] Skills;
    public int Money;

    public void AddinventoryItem(InventoryItem item)
    {
     //   this.Strength += item.Strength;
       // this.Defense += item.Defense;
        Inventory.Add(item);

        // my stuff
        switch (item.type)
        {
            case InventoryItem.ItemType.SWORD:
                BaseAbility swordSlash = ScriptableObject.CreateInstance<SwordSlash>();
                abilities.Add(swordSlash);
                break;
            case InventoryItem.ItemType.AXE:
                break;
            case InventoryItem.ItemType.SPEAR:
                break;
            case InventoryItem.ItemType.BOW:
                BaseAbility bowShot = ScriptableObject.CreateInstance<BaseAbility>();
                abilities.Add(bowShot);
                break;
            case InventoryItem.ItemType.POTION:
                break;
            default:
                break;
        }
    }
}
