﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    #region Singleton
    public static InventoryController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory Found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 8;
    public List<Item> items = new List<Item>();

    public bool Add(Item _item)
    {
        if (!_item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough space in inventory.");
                return false;
            }
            items.Add(_item);

            ConsoleUI.instance.UpdateConsoleUI("Added " + _item.name + " to inventory");

            if (onItemChangedCallback != null)
            {
                // Debug.Log("Invoking onItemChangedCallback");
                onItemChangedCallback.Invoke();
            }
        }
        return true;
    }

    public void Remove(Item _item)
    {
        items.Remove(_item);

        ConsoleUI.instance.UpdateConsoleUI("Removed " + _item.name + " from inventory");

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
