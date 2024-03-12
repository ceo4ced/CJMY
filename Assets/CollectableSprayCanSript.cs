using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSprayCanSript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sprayCan;
    void OnTriggerEnter(Collider c)
    {
     
        Debug.Log("Spray Can Pickup");
        
        if (c.CompareTag("Player"))
        {
            sprayCanCollector cc = c.gameObject.GetComponent<sprayCanCollector>();
            Debug.Log("Spray Can Pickup");
            cc.ReceiveCan();
                
            Destroy(this.gameObject);

     
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
