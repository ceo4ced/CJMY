using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        Debug.Log("CoinPickup2");
       
     
            Debug.Log("CoinPickup");
        
        if (c.CompareTag("Player"))
        {
            coinCollector cc = c.gameObject.GetComponent<coinCollector>();
            Debug.Log("CoinPickup");
                cc.ReceiveCoin();
                
                Destroy(this.gameObject);

     
        }
    }
    void Update()
    {
        transform.Rotate(0f, 300.0f * Time.deltaTime, 0f, Space.Self);
    }

}