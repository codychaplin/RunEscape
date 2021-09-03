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

    const int size = 28; // inventory size
    public Item[] items = new Item[size]; // inventory array

    int itemCount = 0; // current item count

    public bool Add(Item item)
    {
        if (!item.isDefaultItem) // if item is not a default item
        {
            if (itemCount >= size) // if inventory is full
            {
                Debug.Log("Inventory full");
                return false; // if false, don't destroy game object
            }

            for (int i = 0; i < items.Length; i++)
                if (items[i] == null)
                {
                    items[i] = item; // add item to inventory slot
                    itemCount++;
                    break; // breaks at first empty slot
                }

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke(); // trigger event
        }

        return true; // if true, destroy gameobject
    }

    public bool Swap(Item item, int oldIndex, int newIndex)
    {
        if (!item.isDefaultItem) // if item is not a default item
        {
            if (items[newIndex] == null)
            {
                Remove(items[oldIndex]); // removes old item;
                items[newIndex] = item; // add item to inventory slot
                itemCount++;
            }
            else if (items[newIndex] != null)
            {
                // swaps items at indicies
                Item tempItem = items[newIndex];
                items[newIndex] = items[oldIndex];
                items[oldIndex] = tempItem;
            }

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke(); // trigger event
        }

        return true; // if true, destroy gameobject
    }

    public void Remove(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null; // remove item from list
                itemCount--;
                break; // break once found
            }
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke(); // trigger event
    }
}
