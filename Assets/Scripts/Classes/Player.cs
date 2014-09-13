using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public string[] Skills;
    public int Money;

    public void AddinventoryItem(InventoryItem item)
    {
        this.Strength += item.Strength;
        this.Defense += item.Defense;
        Inventory.Add(item);
    }
}
