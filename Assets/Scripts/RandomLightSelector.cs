using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLightSelector : MonoBehaviour
{
    public int numberOfLightsToDisplay = 30; // Number of lights to display

    private void Start()
    {
        SelectRandomLights();
    }

    private void SelectRandomLights()
    {
        // Get the "Markers" GameObject
        GameObject markers = GameObject.Find("Markers");

        if (markers != null)
        {
            List<Transform> lights = new List<Transform>();

            // Get all child objects of the "Markers" GameObject
            foreach (Transform child in markers.transform)
            {
                lights.Add(child);
            }

            //Debug.Log("Total lights: " + lights.Count);

            // Shuffle the list of lights
            Shuffle(lights);

            // Enable the first numberOfLightsToDisplay lights
            for (int i = 0; i < numberOfLightsToDisplay; i++)
            {
                if (i < lights.Count)
                {
                    lights[i].gameObject.SetActive(true);
                    //Debug.Log("Light " + i + " enabled: " + lights[i].name);
                }
            }

            // Disable the lights that were not selected
            for (int i = numberOfLightsToDisplay; i < lights.Count; i++)
            {
                lights[i].gameObject.SetActive(false);
                //Debug.Log("Light " + i + " disabled: " + lights[i].name);
            }
        }
        else
        {
            //Debug.LogError("Markers GameObject not found.");
        }
    }

    // Fisher-Yates shuffle algorithm
    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}