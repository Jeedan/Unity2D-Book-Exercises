using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MessagingManager : MonoBehaviour
{
    public static MessagingManager Instance { get; private set; }

    private List<Action> subscribers = new List<Action>();
    private List<Action<bool>> uiEventSubscribers = new List<Action<bool>>();

    private List<Action<InventoryItem>> inventorySubscribers = new List<Action<InventoryItem>>();

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

    public void SubscribeUIEvent(Action<bool> subscriber)
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

    // sub for inventory manager
    public void SubscribeInventoryEvent(Action<InventoryItem> subscriber)
    {
        if (inventorySubscribers != null)
        {
            inventorySubscribers.Add(subscriber);
        }
    }

    public void BroadcastInventoryEvent(InventoryItem itemInUse)
    {
        foreach (var subscriber in inventorySubscribers)
        {
            subscriber(itemInUse);
        }
    }

    // unsub
    public void UnSubscribeInventoryEvent(Action<InventoryItem> subscriber)
    {
        if (inventorySubscribers != null)
        {
            inventorySubscribers.Remove(subscriber);
        }
    }

    public void ClearAllInventoryEventSubscribers()
    {
        if (inventorySubscribers != null)
        {
            inventorySubscribers.Clear();
        }
    }
}
