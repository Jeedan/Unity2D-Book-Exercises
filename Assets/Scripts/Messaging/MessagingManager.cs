using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MessagingManager : MonoBehaviour
{
    public static MessagingManager Instance { get; private set; }

    private List<Action> subscribers = new List<Action>();
    private List<Action<bool>> uiEventSubscribers = new List<Action<bool>>(); 

    void Awake()
    {
        Debug.Log("Messaging Manager Started");
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    // the subscribe method for manager
    public void Subscribe(Action subscriber)
    {
        Debug.Log("Subscriber registered");
        subscribers.Add(subscriber);
    }

    // unsibscribe method
    public void Unsubscribe(Action subscriber)
    {
        Debug.Log("subscriber unregistered");
        subscribers.Remove(subscriber);
    }

    // clear them all
    public void ClearAllSubscribers()
    {
        subscribers.Clear();
    }

    public void Broadcast()
    {
        Debug.Log("broadcast requested, No of Subscribers = " + subscribers.Count);
        foreach (var subscriber in subscribers)
        {
            subscriber();
        }
    }

    public void UISubscribeEvent(Action<bool> subscriber)
    {
        uiEventSubscribers.Add(subscriber);
    }

    public void BroadcastUIEvent(bool uIVisible)
    {
        foreach (var subscriber in uiEventSubscribers.ToArray())
        {
            subscriber(uIVisible);
        }
    }

    public void UnSubscribeUIEvent(Action<bool> subscriber)
    {
        uiEventSubscribers.Remove(subscriber);
    }

    public void ClearAllUIEventSubscribers()
    {
        uiEventSubscribers.Clear();
    }
}
