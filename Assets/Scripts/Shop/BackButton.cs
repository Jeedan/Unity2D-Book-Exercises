using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour
{
    void OnMouseDown()
    {
        NavigationManager.GoBack();
    }
}
