using UnityEngine;
using System.Collections;

public class ShopSlot : MonoBehaviour
{
    public InventoryItem Item;
    public ShopManager Manager;

    public void AddShopItem(InventoryItem item)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.Sprite;
        spriteRenderer.transform.localScale = item.Scale;
        Item = item;
    }

    public void PurchaseItem()
    {
        GameState.CurrentPlayer.Inventory.Add(Item);
        Item = null;
        var spriterenderer = GetComponent<SpriteRenderer>();
        spriterenderer.sprite = null;
        Manager.ClearSelectedItem();
    }

    void OnMouseDown()
    {
        if (Item != null)
        {
            Manager.SetShopSelecteditem(this);
        }
    }
}
