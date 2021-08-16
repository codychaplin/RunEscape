using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region
    public static EquipmentManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    Equipment[] currentEquipment;
    Inventory inventory;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        int numOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length; // gets length of equipment enum
        currentEquipment = new Equipment[numOfSlots]; // equipment array with same length as equipment slots
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipmentSlot; // slotIndex = index in enum
        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null) // if equipment slot isn't empty
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem); // add equipment currently in slot to inventory
        }

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke(newItem, oldItem);

        currentEquipment[slotIndex] = newItem; // add new item to slot
    }

    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null) // if equipment slot isn't empty
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem); // add equipment currently in slot to inventory
            currentEquipment[slotIndex] = null; // remove equipment from slot

            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++) // loops through each equipment slot and removes them
            Unequip(i);
    }
}