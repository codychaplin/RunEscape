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

    public SkinnedMeshRenderer targetMesh; // player mesh
    public Equipment[] defaultItems; // array of clothes to wear on start

    Equipment[] currentEquipment; // array of current equipment
    SkinnedMeshRenderer[] currentMeshes; // array of meshes for current equipment
    Inventory inventory;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        int numOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length; // gets length of equipment enum
        currentEquipment = new Equipment[numOfSlots]; // equipment array with same length as equipment slots
        currentMeshes = new SkinnedMeshRenderer[numOfSlots]; // mesh array

        EquipDefaultItems(); // gives player default clothes on start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            UnequipAll();
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipmentSlot; // slotIndex = index in enum
        Equipment oldItem = Unequip(slotIndex); // unequips item and assigns it to oldItem

        if (onEquipmentChanged != null)
            onEquipmentChanged.Invoke(newItem, oldItem);

        SetBlendShapes(newItem, 100);

        currentEquipment[slotIndex] = newItem; // add new item to slot
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;
        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }

    public Equipment Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null) // if equipment slot isn't empty
        {
            if (currentMeshes[slotIndex] != null)
                Destroy(currentMeshes[slotIndex].gameObject);

            Equipment oldItem = currentEquipment[slotIndex];
            SetBlendShapes(oldItem, 0);
            inventory.Add(oldItem); // add equipment currently in slot to inventory
            currentEquipment[slotIndex] = null; // remove equipment from slot

            if (onEquipmentChanged != null)
                onEquipmentChanged.Invoke(null, oldItem);

            return oldItem;
        }

        return null;
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++) // loops through each equipment slot and removes them
            Unequip(i);

        EquipDefaultItems();
    }

    void SetBlendShapes(Equipment item, int weight)
    {
        foreach (EquipmentMeshRegion blendShapes in item.coveredRegions)
        {
            targetMesh.SetBlendShapeWeight((int)blendShapes, weight);
        }
    }

    void EquipDefaultItems()
    {
        foreach (Equipment item in defaultItems)
            Equip(item);
    }
}