using UnityEngine;
using System.Collections;

public class EventTemplate
{
    public delegate void LogMessage(string message);
    public static event LogMessage Log;

    void OnLog(string message)
    {
        if (Log != null)
        {
            Log(message);
        }
    }
}
