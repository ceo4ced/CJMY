using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDonutTest : MonoBehaviour
{
    public GameObject heldDonut;
    public Transform donutProjectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowDonut()
    {
        Debug.Log("Threw Donut");
        heldDonut.SetActive(false);
        Instantiate(donutProjectile, heldDonut.GetComponent<Transform>().position, Quaternion.identity);
    }
}
