using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDonutTest : MonoBehaviour
{
    public GameObject heldDonut;

    public Transform donutProjectile;

    private Transform Spawner;

    // Start is called before the first frame update
    void Start()
    {
        Spawner = GetComponent<Transform>();
        heldDonut.SetActive(true); //ADDED BY CEDRIC
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowDonut()
    {
        Debug.Log("Throw Donut Function called");
        // if (!heldDonut.activeSelf == false)
        if(true)
        {
            Debug.Log("Grab Donut");
            // heldDonut.SetActive(false);
            Transform thrownDonut = Instantiate(donutProjectile, heldDonut.GetComponent<Transform>().position, Quaternion.identity);
            
            Debug.Log("Aim Donut");
            ProjectileScript thrownDonutProjectileScript = thrownDonut.GetComponent<ProjectileScript>();
            thrownDonutProjectileScript.Setup(Spawner.forward);
            // heldDonut.SetActive(true); //ADDED BY CEDRIC
            Debug.Log("Threw Donut");
        }
        // else 
        // {
        //     Debug.Log("Donut not thrown");
        // }
    }

    public void StopThrowingDonut()  //ADDED BY CEDRIC
    {
        Debug.Log("Stopped Throwing Donut");
        heldDonut.SetActive(true);
    }

}
////////////////////
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ThrowDonutTest : MonoBehaviour
// {
//     public GameObject heldDonut;
//     public Transform donutProjectile;
//     private Transform Spawner;
//     public float throwDistance = 10f; // Maximum distance to throw donuts

//     // Start is called before the first frame update
//     void Start()
//     {
//         Spawner = GetComponent<Transform>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     public void ThrowDonut()
//     {
//         GameObject player = GameObject.FindWithTag("Player"); // Find the player GameObject
//         float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position); // Calculate the distance to the player

//         // Check if the distance is within the throw distance and the held donut is active
//         if (distanceToPlayer <= throwDistance && heldDonut.activeSelf)
//         {
//             Debug.Log("Throw Donut");
//             Transform thrownDonut = Instantiate(donutProjectile, heldDonut.GetComponent<Transform>().position, Quaternion.identity);
//             ProjectileScript thrownDonutProjectileScript = thrownDonut.GetComponent<ProjectileScript>();
//             thrownDonutProjectileScript.Setup(Spawner.forward);
//         }
//         else
//         {
//             // Optionally, log when the donut is not thrown due to being too far away
//             Debug.Log("Too far to throw donut");
//         }
//     }
// }

