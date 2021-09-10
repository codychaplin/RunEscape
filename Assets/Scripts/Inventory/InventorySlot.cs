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
        parent.AddComponent<BoxCollider>(); // adds box collider
        parent.AddComponent<ItemPickup>(); // adds itemPickup script

        BoxCollider boxCollider = parent.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(boxCollider.size.x, 0.01f, boxCollider.size.z); // sets the box collider size
        ItemPickup pickup = parent.GetComponent<ItemPickup>();
        pickup.item = item; // sets the item to the item in the inventory slot

        parent.transform.position = PlayerManager.instance.player.transform.position; // sets the position to the players position
        parent.transform.position = new Vector3(Mathf.FloorToInt(
            parent.transform.position.x) + 0.5f, parent.transform.position.y, Mathf.FloorToInt(parent.transform.position.z) + 0.5f); // sets y to 0 and rounds position to int

        Instantiate(item.prefab, parent.transform); // instantiates prefab
    }
}
