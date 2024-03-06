using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    Ray sightRay;
    RaycastHit hit;
    public float sightRange =  5;
    // Start is called before the first frame update
    void Start()
    {
        // Vector3 direction = new Vector3(0, 0, 1);
        // sightRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward * sightRange));
        // Debug.DrawRay(transform.position, transform.TransformDirection(direction * sightRange), Color.green);
        // // CheckForPlayer1();
    }

    void Update()
    {
        
        CheckForPlayer1();
    }
    void CheckForPlayer1()
    {
        Vector3 currentDirection = transform.TransformDirection(Vector3.forward) * sightRange;
        if (Physics.Raycast(transform.position, currentDirection, out hit, sightRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("I see you");
            }
        }
        // Optionally, to visualize the ray in the Scene view while playing
        Debug.DrawRay(transform.position, currentDirection, Color.green);
    }


}
