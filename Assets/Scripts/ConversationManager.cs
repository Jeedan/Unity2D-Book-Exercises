using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConversationManager : Singleton<ConversationManager>
{
    protected ConversationManager() { } // can't use the constructor

    // is there a convo going on?
    bool talking = false;

    // the current line of text being displayed
    ConversationEntry currentConversationLine;

    // estimated width of characters in the font
    int fontSpacing = 7;

    // How wide does the dialog window need to be 
    int conversationTextWidth;

    // How high does the dialog window need to be
    int dialogHeight = 70;

    // display Texture offset
    public int displayTextureOffset = 70;

    // scaled image for displaying character image
    Rect scaledTextureRect;

    public bool stopConvo = true;

    // my GUI 4.6 implementation of this
    public GameObject convoContainer;
    public RectTransform panel;
    public Text crntSpeakerText;
    public Image crntSpeakerImage;
    public Text crntText;

    public IEnumerator DisplayConversation(Conversation conversation)
    {
        talking = true;
        foreach (var conversationLine in conversation.ConversationLines)
        {
            currentConversationLine = conversationLine;
            conversationTextWidth = currentConversationLine.ConversationText.Length * fontSpacing;

            // we might need to change this later too
            scaledTextureRect = new Rect(currentConversationLine.DisplayPic.textureRect.x / currentConversationLine.DisplayPic.texture.width,
                currentConversationLine.DisplayPic.textureRect.y / currentConversationLine.DisplayPic.texture.height,
                currentConversationLine.DisplayPic.textureRect.width / currentConversationLine.DisplayPic.texture.width,
                currentConversationLine.DisplayPic.textureRect.height / currentConversationLine.DisplayPic.texture.height);

            yield return new WaitForSeconds(3);
        }

        talking = false;
        yield return null;
    }
    
    void Update()
    {
        if (talking)
        {
            convoContainer.SetActive(true);
            crntSpeakerText.text = currentConversationLine.SpeakingCharacterName;
            crntText.text = currentConversationLine.ConversationText;
            crntSpeakerImage.sprite = currentConversationLine.DisplayPic;
        }
        else
        {
            convoContainer.SetActive(false);
        }
    }

    // replace this with 4.6 stuff
    //void OnGUI()
    //{
    //    if (talking)
    //    {
    //        // layout start
    //        GUI.BeginGroup(new Rect(Screen.width / 2 - conversationTextWidth / 2, 50,
    //                                conversationTextWidth + displayTextureOffset + 10, dialogHeight));
    //        // the background box
    //        GUI.Box(new Rect(0, 0, conversationTextWidth + displayTextureOffset + 10, dialogHeight), "");

    //        // the character name
    //        GUI.Label(new Rect(displayTextureOffset, 10, conversationTextWidth + 30, 20), currentConversationLine.SpeakingCharacterName);

    //        // the conversationText
    //        GUI.Label(new Rect(displayTextureOffset, 30, conversationTextWidth + 30, 20), currentConversationLine.ConversationText);

    //        // the character image
    //        GUI.DrawTextureWithTexCoords(new Rect(10, 10, 50, 50), currentConversationLine.DisplayPic.texture, scaledTextureRect);

    //        // layout end
    //        GUI.EndGroup();
    //    }
    //}

    public void StartConversation(Conversation conversation)
    {
        if (!talking)
        {
            StartCoroutine(DisplayConversation(conversation));
        }
    }

    public void StopConversation(Conversation conversation)
    {
        StopCoroutine(DisplayConversation(conversation));
        talking = false;
    }
}
