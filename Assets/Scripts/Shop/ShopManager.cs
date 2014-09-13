using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public Sprite ShopOwnerSprite;
    public Vector3 ShopOwnerScale;
    public GameObject ShopOwnerLocation;
    public GameObject PurchasingSection;
    public SpriteRenderer PurchaseItemDisplay;
    public ShopSlot[] ItemSlots;
    public InventoryItem[] ShopItems;
    private static ShopSlot SelectedShopSlot;

    private int nextSlotIndex = 0;

    void Start()
    {
        var OwnerSpriteRenderer = ShopOwnerLocation.GetComponent<SpriteRenderer>();
        OwnerSpriteRenderer.sprite = ShopOwnerSprite;
        OwnerSpriteRenderer.transform.localScale = ShopOwnerScale;

        if (ItemSlots.Length > 0 && ShopItems.Length > 0)
        {
            for (int i = 0; i < ShopItems.Length; i++)
            {
                if (nextSlotIndex > ItemSlots.Length) break;
                ItemSlots[nextSlotIndex].AddShopItem(ShopItems[i]);
                ItemSlots[nextSlotIndex].Manager = this;
                nextSlotIndex++;
            }
        }
    }

    public void SetShopSelecteditem(ShopSlot slot)
    {
        SelectedShopSlot = slot;
        PurchaseItemDisplay.sprite = slot.Item.Sprite;
        PurchasingSection.SetActive(true);
    }

    public void ClearSelectedItem()
    {
        Debug.Log("Clearing Shop Purchase area");
        SelectedShopSlot = null;
        PurchaseItemDisplay.sprite = null;
        PurchasingSection.SetActive(false);
    }

    public static void PurchaseSelectedItem()
    {
        SelectedShopSlot.PurchaseItem();
    }
}
