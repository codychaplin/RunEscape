using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact(); // inherited interact function from Interactable.cs

        Pickup();
    }

    void Pickup()
    {
        if(Inventory.instance.Add(item)) // if item is successfully picked up
            Destroy(gameObject); // delete from scene
    }
}
