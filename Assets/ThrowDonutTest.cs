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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowDonut()
    {
        Debug.Log("Threw Donut");
        heldDonut.SetActive(false);
        Transform thrownDonut = Instantiate(donutProjectile, heldDonut.GetComponent<Transform>().position, Quaternion.identity);
        ProjectileScript thrownDonutProjectileScript = thrownDonut.GetComponent<ProjectileScript>();
        thrownDonutProjectileScript.Setup(Spawner.forward);
    }
}
