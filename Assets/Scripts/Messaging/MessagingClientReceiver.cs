using UnityEngine;
using System.Collections;

public class MessagingClientReceiver : MonoBehaviour
{
    public Transform player;
    public Transform wizard;
    public bool playingDialoge = false;
    Conversation conversation;
    // Use this for initialization
    void Start()
    {
        MessagingManager.Instance.Subscribe(ThePlayerIsTryingToLeave);

        player = GameObject.Find("Player").transform;
        wizard = GameObject.Find("Greybeard").transform;
    }

    void ThePlayerIsTryingToLeave()
    {
        playingDialoge = false;
        Debug.Log("Oi Don't Leave me!! - " + tag.ToString());
        var dialog = GetComponent<ConversationComponent>();
        if (dialog != null)
        {
            if (dialog.Conversations != null && dialog.Conversations.Length > 0)
            {
                 conversation = dialog.Conversations[0];

                playingDialoge = true;
                if (conversation != null && playingDialoge)
                {
                    ConversationManager.Instance.StartConversation(conversation);
                }
            }
        }
    }

    void Update()
    {
        if (playingDialoge)
        {
            Vector3 playerPos = player.position;
            Vector3 wizardPos = wizard.position;

            float distance = (playerPos - wizardPos).magnitude;
            if (distance > 2)
            {
                Debug.Log("out of range");
                ConversationManager.Instance.StopConversation(conversation);
            }
        }
    }
}
