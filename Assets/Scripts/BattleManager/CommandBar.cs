using UnityEngine;
using System.Collections;

public class CommandBar : MonoBehaviour
{
    public InventoryItem debugItem; // debug purposes
    public InventoryItem debugItem2;

    private CommandButton[] commandButtons;
    public float buttonSize = 0.5f;
    public float buttonRows = 1;
    public float buttonColumns = 6;
    public float buttonRowSpacing = 0;
    public float buttonColumnSpacing = 0;

    public Sprite DefaultButtonImage;
    public Sprite SelectedButtonImage;

    public bool anchor = true;
    public Vector2 anchorOffset = Vector2.zero;
    public ScreenPositionAnchorPoint anchorPoint = ScreenPositionAnchorPoint.BottomCenter;

    private float ScreenHeight;
    private float ScreenWidth;

    public int Layer { get { return gameObject.layer; } }
    private float Width { get { return (buttonSize * buttonColumns) + Mathf.Clamp((buttonColumnSpacing * (buttonColumns - 1)), 0, int.MaxValue); } }
    private float Height { get { return (buttonSize * buttonRows) + Mathf.Clamp((buttonRowSpacing * (buttonRows - 1)), 0, int.MaxValue); } }

    private bool canSelectButton = true;
    private CommandButton selectedButton;

    public bool CanSelectButton { get { return canSelectButton; } }

    public enum ScreenPositionAnchorPoint
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    void Awake()
    {
//#if UNITY_EDITOR
//#endif
        GameState.CurrentPlayer.AddinventoryItem(debugItem);
        GameState.CurrentPlayer.AddinventoryItem(debugItem2);
        ScreenHeight = Camera.main.orthographicSize * 2;
        ScreenWidth = ScreenHeight * Screen.width / Screen.height;
    }

    void Start()
    {
        InitCommandButtons();
        MessagingManager.Instance.SubscribeUIEvent(SetCanSelectButton);
    }

    void Update()
    {
        Vector2 position = Vector2.zero;
        if (anchor)
        {
            position = CalculateAnchorScreenPosition();
        }
        else
        {
            position = transform.position;
        }

        SetPosition(position.x, position.y);
    }

    private void InitCommandButtons()
    {
        commandButtons = new CommandButton[(int)buttonRows * (int)buttonColumns];
        for (int i = 0; i < commandButtons.Length; i++)
        {
            var newButton = CreateButton();
            if (i < GameState.CurrentPlayer.Inventory.Count)
            {
                newButton.AddInventoryItem(GameState.CurrentPlayer.Inventory[i]);
            }

            commandButtons[i] = newButton;
        }
        InitButtonPositions();
    }

    private void InitButtonPositions()
    {
        int i = 0;
        float xPos = 0;
        float yPos = 0;

        for (int r = 0; r < buttonRows; ++r)
        {
            xPos = 0;
            for (int c = 0; c < buttonColumns; ++c)
            {
                commandButtons[i].transform.localScale = new Vector3(buttonSize, buttonSize, 0);
                commandButtons[i].transform.localPosition = new Vector3(xPos, yPos, 0);

                i++;
                xPos += buttonSize + buttonColumnSpacing;
            }

            yPos -= buttonSize + buttonRowSpacing;
        }
    }

    private CommandButton CreateButton()
    {
        GameObject go = new GameObject("CommandButton");

        // add components
        go.AddComponent<SpriteRenderer>();
        go.AddComponent<BoxCollider2D>();
        go.transform.parent = transform;

        // Init
        CommandButton button = go.AddComponent<CommandButton>();
        button.Init(this);

        return button;
    }


    public void SelectButton(CommandButton button)
    {
        ResetSelection(button);
        if (selectedButton != null)
        {
            MessagingManager.Instance.BroadcastInventoryEvent(selectedButton.Item);
        }
        else
        {
            MessagingManager.Instance.BroadcastInventoryEvent(null);
        }
    }


    public void ResetSelection(CommandButton button)
    {
        if (selectedButton != null)
        {
            selectedButton.ClearSelection();
        }
        selectedButton = button;
    }

    Vector2 CalculateAnchorScreenPosition()
    {
        Vector2 position = Vector2.zero;

        switch (anchorPoint)
        {
            case ScreenPositionAnchorPoint.BottomLeft:
                position.y = -(ScreenHeight / 2) + Height;
                position.x = -(ScreenWidth / 2) + buttonSize;
                break;

            case ScreenPositionAnchorPoint.BottomCenter:
                position.y = -(ScreenHeight / 2) + Height;
                position.x = -(Width / 2);
                break;

            case ScreenPositionAnchorPoint.BottomRight:
                position.y = -(ScreenHeight / 2) + Height;
                position.x = (ScreenWidth / 2) - Width;
                break;

            case ScreenPositionAnchorPoint.MiddleLeft:
                position.y = (Height / 2);
                position.x = -(ScreenWidth / 2) + buttonSize;
                break;

            case ScreenPositionAnchorPoint.MiddleCenter:
                position.y = (Height / 2);
                position.x = -(Width / 2);
                break;

            case ScreenPositionAnchorPoint.MiddleRight:
                position.y = (Height / 2);
                position.x = (ScreenWidth / 2) - Width;
                break;
            case ScreenPositionAnchorPoint.TopLeft:
                position.y = (ScreenHeight / 2) - Height;
                position.x = -(ScreenWidth / 2) + buttonSize;
                break;

            case ScreenPositionAnchorPoint.TopCenter:
                position.y = (ScreenHeight / 2) - Height;
                position.x = -(Width / 2);
                break;

            case ScreenPositionAnchorPoint.TopRight:
                position.y = (ScreenHeight / 2) - Height;
                position.x = (ScreenWidth / 2) - Width;
                break;
        }
        return anchorOffset + position;
    }


    void SetCanSelectButton(bool state)
    {
        canSelectButton = !state;

        // reset selection every time we freeze UIevents
        if (!canSelectButton)
        {
            ResetSelection(selectedButton);
            selectedButton = null;
        }
    }

    void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }

    void OnDestroy()
    {
        if (MessagingManager.Instance != null)
        {
            MessagingManager.Instance.UnSubscribeUIEvent(SetCanSelectButton);
        }
    }
}
