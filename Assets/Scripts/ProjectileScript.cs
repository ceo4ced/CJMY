using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileScript : MonoBehaviour
{
    public float projectileSpeed = 20f;
    public Vector3 direction = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(direction * projectileSpeed, ForceMode.Impulse);
        Destroy(gameObject, 5f);
    }
    public void Setup(Vector3 dir)
    {
        Debug.Log(dir);
        direction = dir;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.CompareTag("Cop")){
            //Ignore collision with cop (self)
        }else{ 
        if (c.CompareTag("Player"))
        {
            Debug.Log("Collided");
            ReceiveDamageScript damageReceiver = c.GetComponent<ReceiveDamageScript>();
            damageReceiver.ReceiveDamage(direction);
            Destroy(gameObject);
        }
        }
    }
}
