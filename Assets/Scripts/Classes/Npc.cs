using UnityEngine;
public class Npc : Entity
{
    Transform player;
    Transform wizard;
    public bool playingDialogue = false;
    MessagingClientReceiver messagingClientReceiver;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").transform;
        wizard = GameObject.Find("Greybeard").transform;

        if (messagingClientReceiver == null)
        {
            messagingClientReceiver = (MessagingClientReceiver)GetComponent(typeof(MessagingClientReceiver));
        }

        if (!player)
        {
            Debug.LogError("Player not found in " + this.name + " Npc script");
        }

        if (!wizard)
        {
            Debug.LogError("Wizard not found in " + this.name + " Npc script");
        }
    }

    void Update()
    {
        if (messagingClientReceiver != null && messagingClientReceiver.playingDialogue)
        {
            Vector3 playerPos = player.position;
            Vector3 wizardPos = wizard.position;

            float distance = (playerPos - wizardPos).magnitude;
            if (distance > 2)
            {
                ConversationManager.Instance.StopConversation(messagingClientReceiver.conversation);
                messagingClientReceiver.playingDialogue = false;
            }
        }
    }
}
