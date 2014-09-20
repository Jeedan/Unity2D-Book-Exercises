using UnityEngine;
using System.Collections;

public class SwordSlash : BaseAbility
{
    public override void UseAbility(GameObject go)
    {
        GameObject newGO = GameObject.Instantiate(go) as GameObject;
        Debug.Log("Sword Slash");
    }

    public override void UseAbility()
    {
        Debug.Log("Sword Slash");
    }
}
