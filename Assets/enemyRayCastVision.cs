using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class rayCastVision : MonoBehaviour
{
    Ray sightRay;
    RaycastHit hit;
    public float sightRange =  5;
    // Start is called before the first frame update
    void Start()
    {
        sightRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward * sightRange));
        CheckForPlayer1();
    }

    void CheckForPlayer1()
    {
        if (Physics.Raycast(sightRay, out hit, sightRange))
        {
            if (hit.collider.tag == "Player")
            {
                Debug.Log("I see you");
            }
        }
    }
}

