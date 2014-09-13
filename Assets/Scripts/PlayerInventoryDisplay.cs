using UnityEngine;
using System.Collections;

public class PlayerInventoryDisplay : MonoBehaviour
{
    private bool displayInventory = false;
    private Rect inventoryWindowRect;
    private Vector2 inventoryWindowSize = new Vector2(150, 150);
    private Vector2 inventoryItemIconSize = new Vector2(130, 32);
    private float offsetX = 6;
    private float offsetY = 6;

    void Awake()
    {
        inventoryWindowRect = new Rect(Screen.width - inventoryWindowSize.x,
                                       Screen.height - inventoryWindowSize.y,
                                       inventoryWindowSize.x,
                                       inventoryWindowSize.y);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 40, Screen.height - 40, 40, 40), "INV"))
        {
            displayInventory = !displayInventory;
        }
        if (displayInventory)
        {
            inventoryWindowRect = GUI.Window(0, inventoryWindowRect, DisplayInventoryWindow, "Inventory");
            inventoryWindowSize = new Vector2(inventoryWindowRect.width, inventoryWindowRect.height);
        }
    }

    void DisplayInventoryWindow(int windowID)
    {
        var currentX = 0 + offsetX;
        var currentY = 18 + offsetY;
        foreach (var item in GameState.CurrentPlayer.Inventory)
        {
            Rect texcoords = item.Sprite.textureRect;
            texcoords.x /= item.Sprite.texture.width;
            texcoords.y /= item.Sprite.texture.height;
            texcoords.width /= item.Sprite.texture.width;
            texcoords.height /= item.Sprite.texture.height;

            GUI.DrawTextureWithTexCoords(new Rect(currentX, currentY, item.Sprite.textureRect.width, item.Sprite.textureRect.height), item.Sprite.texture, texcoords);

            currentX += inventoryItemIconSize.x;
            if (currentX + inventoryItemIconSize.x + offsetX > inventoryWindowSize.x)
            {
                currentX = offsetX;
                currentY += inventoryItemIconSize.y;
                if (currentY + inventoryItemIconSize.y + offsetY > inventoryWindowSize.y)
                    return;
            }
        }
    }
}
