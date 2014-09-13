using UnityEngine;
using System.Collections;

public class BuyButton : MonoBehaviour
{
    void OnMouseDown()
    {
        ShopManager.PurchaseSelectedItem();
    }
}
