using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    private string itemTag; // Variable to store the tag of the item being picked up
    private bool timerActivated;
    private float respawnTimer;

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
                //this.gameObject.SetActive(false);
                foreach (Transform child in transform)
                {
                    child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    child.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                }
                this.gameObject.GetComponent<Collider>().enabled = false;
                // start timer for respawn
                this.timerActivated = true;
                this.respawnTimer = 30.0f;
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
        if (timerActivated)
        {
            this.respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0.0f)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    child.gameObject.GetComponent<CapsuleCollider>().enabled = true;
                }
                this.gameObject.GetComponent<Collider>().enabled = true;
                this.timerActivated = false;
            } 
        }
    }
}
