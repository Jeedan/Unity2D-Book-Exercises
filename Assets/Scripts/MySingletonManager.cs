using UnityEngine;
using System.Collections;

public class MySingletonManager : MonoBehaviour
{
    public static MySingletonManager Instance { get; private set; }

    public string MyTextProperty = "Hello World";

    // Use this for initialization
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void DoSomethingAwesome() { }
}
