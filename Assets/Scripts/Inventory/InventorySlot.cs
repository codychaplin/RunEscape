using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Item item { get; private set; }
    public Image icon;

    public int index;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        if (eventData.pointerDrag != null)
        {
            Inventory.instance.Swap(
                eventData.pointerDrag.GetComponentInParent<InventorySlot>().item,
                eventData.pointerDrag.GetComponentInParent<InventorySlot>().index, index);
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

    public void DropItem()
    {
        if (item != null)
        {
            BuildPrefab();
            Inventory.instance.Remove(item);
        }
    }

    void BuildPrefab()
    {
        GameObject parent = new GameObject();
        parent.transform.SetParent(GameObject.FindGameObjectWithTag("Drops").transform);
        parent.name = item.name + "Pickup";
        parent.AddComponent<BoxCollider>();
        parent.AddComponent<ItemPickup>();

        ItemPickup pickup = parent.GetComponent<ItemPickup>();
        pickup.item = item;

        parent.transform.position = PlayerManager.instance.player.transform.position;
        parent.transform.position = new Vector3(parent.transform.position.x, 0f, parent.transform.position.z);

        Instantiate(item.prefab, parent.transform);
    }
}
