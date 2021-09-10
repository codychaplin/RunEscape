using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform invPanel;
    public GameObject inventoryUI;

    Inventory inventory;
    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI; // UI update is subscribed to item change event

        slots = invPanel.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].index = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
    
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // if item is at index, update UI slot, else clear slot
            if (inventory.items[i] != null)
                slots[i].AddItem(inventory.items[i]);
            else
                slots[i].ClearSlot();
        }
    }
}
