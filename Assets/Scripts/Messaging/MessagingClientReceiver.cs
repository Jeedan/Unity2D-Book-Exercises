using UnityEngine;
using System.Collections;

public class MessagingClientReceiver : MonoBehaviour
{
    public bool playingDialogue = false;
    public Conversation conversation;

    // Use this for initialization
    void Start()
    {
        MessagingManager.Instance.Subscribe(ThePlayerIsTryingToLeave);
    }

    void ThePlayerIsTryingToLeave()
    {
        playingDialogue = false;
        Debug.Log("Oi Don't Leave me!! - " + tag.ToString());
        var dialog = GetComponent<ConversationComponent>();
        if (dialog != null)
        {
            if (dialog.Conversations != null && dialog.Conversations.Length > 0)
            {
                conversation = dialog.Conversations[0];

                if (conversation != null)
                {
                    playingDialogue = true;
                    ConversationManager.Instance.StartConversation(conversation);
                }
            }
        }
    }
}
