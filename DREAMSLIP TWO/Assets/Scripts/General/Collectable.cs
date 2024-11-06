using UnityEngine;

public class Collectable : Item
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            Inventory.AddItemToInventory(this);
            Destroy(gameObject);
        }
    }
}
