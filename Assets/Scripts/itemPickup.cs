using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    private string itemTag; // Variable to store the tag of the item being picked up

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            itemCollector cc = c.gameObject.GetComponent<itemCollector>();
            if (cc != null && !cc.HasItem()) // Check if the player does not already have an item
            {
                cc.ReceiveItem(itemTag); // Pass the item tag to the itemCollector
                AJ_controller_Script ajController = c.gameObject.GetComponent<AJ_controller_Script>();
                if (ajController != null)
                {
                    ajController.SetColorItem(itemTag); // Pass the item tag to the AJ_controller_script
                }
                else
                {
                    Debug.LogWarning("AJ_controller_Script not found on player object.");
                }
                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogWarning("Player already has an item.");
                // Optionally, you could play a sound or provide feedback to the player indicating they can't pick up another item.
            }
        }
    }

    void Start()
    {
        itemTag = this.gameObject.tag; // Store the tag of the object
    }

    void Update()
    {
        transform.Rotate(0f, 300.0f * Time.deltaTime, 0f, Space.Self);
    }
}
