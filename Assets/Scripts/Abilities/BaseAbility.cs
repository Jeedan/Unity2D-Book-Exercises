using UnityEngine;
using System.Collections;

public class BaseAbility : ScriptableObject
{
    public virtual void UseAbility()
    {
        Debug.Log("Used Base Ability");
    }

    public virtual void UseAbility(GameObject go)
    {
    }
}
