using UnityEngine;
public class Npc : MonoBehaviour
{
    Transform player;
    Transform wizard;
    public bool playingDialogue = false;
    MessagingClientReceiver messagingClientReceiver;

    public string Name;
    public int Age;
    public string Faction;
    public string Occupation;
    public int Level = 1;
    public int Health = 2;
    public int Strength = 1;
    public int Magic = 0;
    public int Defense = 0;
    public int Speed = 1;
    public int Damage = 1;
    public int Armor = 0;
    public int NoOfAttacks = 1;
    public string Weapon;
    public Vector2 Position;

    public void TakeDamage(int Amount)
    {
        Health -= Mathf.Clamp((Amount - Armor), 0, int.MaxValue);
    }

    public void Attack(Entity Entity)
    {
        Entity.TakeDamage(Strength);
    }

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
