using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class CommandButton : MonoBehaviour
{
    private CommandBar commandBar;
    public InventoryItem Item;
    private bool selected;

    public void Init(CommandBar commandBar)
    {
        this.commandBar = commandBar;
        gameObject.layer = commandBar.Layer;

        var collider = (BoxCollider2D)gameObject.GetComponent(typeof(BoxCollider2D));
        collider.size = new Vector2(1f, 1f);

        var renderer = (SpriteRenderer)gameObject.GetComponent(typeof(SpriteRenderer));
        renderer.sprite = commandBar.DefaultButtonImage;
        renderer.sortingLayerName = "GUI";
        renderer.sortingOrder = 5;
    }

    public void AddInventoryItem(InventoryItem item)
    {
        this.Item = item;
        var childGO = new GameObject("InventoryItemDisplayImage");
        var renderer = childGO.AddComponent<SpriteRenderer>();
        renderer.sprite = item.Sprite;
        renderer.sortingLayerName = "GUI";
        renderer.sortingOrder = 10;
        renderer.transform.parent = this.transform;
        renderer.transform.localScale *= 4;
    }

    

    private void UpdateSelection()
    {
        var renderer = (SpriteRenderer)gameObject.GetComponent(typeof(SpriteRenderer));
        renderer.sprite = selected ? commandBar.SelectedButtonImage : commandBar.DefaultButtonImage;
    }

    public void ClearSelection()
    {
        selected = false;
        UpdateSelection();
    }

    void OnMouseDown()
    {
        if (commandBar.CanSelectButton)
        {
            selected = !selected;
            UpdateSelection();
            commandBar.SelectButton(selected ? this : null);
        }
    }
}
