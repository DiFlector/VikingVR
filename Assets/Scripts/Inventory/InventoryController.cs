using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public Inventory inventory;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<ItemController>().getType() == "log")
        {
            if (inventory.logCount != inventory.maxLog && inventory.logCount + collision.GetComponent<ItemController>().count <= 10)
            {
                inventory.logCount += collision.GetComponent<ItemController>().count;
                Destroy(collision.gameObject);
            }
        }
    }
}
