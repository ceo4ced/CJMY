using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CollectableSprayCanSript : MonoBehaviour
{
    // Start is called before the first frame update
    // public GameObject sprayCan;
    // void OnTriggerEnter(Collider c)
    // {
     
    //     Debug.Log("Spray Can Pickup");
        
    //     // if (c.CompareTag("Player"))
    //     // {
    //     //     Debug.Log("HelloCedric");
    //     //     sprayCanCollector cc = c.gameObject.GetComponent<sprayCanCollector>();
    //     //     Debug.Log("Spray Can Pickup");
    //     //     cc.ReceiveCan();
                
    //     //     Destroy(this.gameObject);

     
    //     // }
        

    //     if (c.CompareTag("Player"))
    //     {
    //         EventManager.instance.onSprayCanCollected.Invoke();
    //         // TriggerEvent("SprayCanPickup");
            
    //         Debug.Log("Spray Can Collected");

    //         Destroy(gameObject);

     
    //     }
    // }

        // using UnityEngine;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Spray Can Pickup");

                // Check if the EventManager instance exists and invoke the event
                if (EventManagerScript.instance != null)
                {
                    EventManagerScript.instance.onSprayCanCollected.Invoke();
                    Debug.Log("Spray Can Collected");
                }
                else
                {
                    Debug.LogError("EventManager instance not found.");
                }

                Destroy(gameObject);
            }
        }
}

 

