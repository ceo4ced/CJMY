using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {       
     
        
        if (c.CompareTag("Player"))
        {
            itemCollector cc = c.gameObject.GetComponent<itemCollector>();
                cc.RecieveItem();
                
                Destroy(this.gameObject);

     
        }
    }
    void Update()
    {
        transform.Rotate(0f, 300.0f * Time.deltaTime, 0f, Space.Self);
    }

}