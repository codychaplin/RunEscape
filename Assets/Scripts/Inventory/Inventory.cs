using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback; // inventory event

    public List<Item> items = new List<Item>(); // inventory list
    public int size = 28; // inventory size

    public bool Add(Item item)
    {
        if (!item.isDefaultItem) // if item is not a default item
        {
            if (items.Count >= size) // if inventory is full
            {
                Debug.Log("Inventory full");
                return false; // if false, don't destroy game object
            }
            
            items.Add(item); // add item to inventory

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke(); // trigger event
        }

        return true; // if true, destroy gameobject
    }

    public void Remove(Item item)
    {
        Debug.Log("Removed " + item.name);

        items.Remove(item); // remove item from list

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke(); // trigger event
    }
}
