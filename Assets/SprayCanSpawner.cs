using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayCanSpawner : MonoBehaviour
{
    public GameObject redCanPrefab;
    public GameObject blueCanPrefab;
    public GameObject greenCanPrefab;
    public LayerMask groundLayerMask;
    public float minDistanceBetweenCans = 5f; // Adjust this value as needed

    // Configuration for number of cans to spawn
    private int numGreenCans = 25;
    private int numBlueCans = 10;
    private int numRedCans = 5;

    private int totalCansSpawned = 0; // Counter for total cans spawned
    private List<Vector3> occupiedPositions = new List<Vector3>();

    void Start()
    {
        // Calculate map dimensions
        float minX = -205f;
        float maxX = 92f;
        float minZ = -70f;
        float maxZ = 155f;

        // Spawn cans
        SpawnCans(greenCanPrefab, numGreenCans, minX, maxX, minZ, maxZ);
        SpawnCans(blueCanPrefab, numBlueCans, minX, maxX, minZ, maxZ);
        SpawnCans(redCanPrefab, numRedCans, minX, maxX, minZ, maxZ);

        Debug.Log("Total cans spawned: " + totalCansSpawned);
    }

    void SpawnCans(GameObject canPrefab, int quantity, float minX, float maxX, float minZ, float maxZ)
    {
        for (int i = 0; i < quantity; i++)
        {
            Vector3 canPosition = FindPositionForCan(minX, maxX, minZ, maxZ);
            if (canPosition != Vector3.zero) // A suitable position was found
            {
                Instantiate(canPrefab, canPosition, Quaternion.identity);
                totalCansSpawned++; // Increment the total cans spawned counter
            }
            else
            {
                Debug.LogWarning("Could not find a suitable position for more cans.");
                break; // Exit the loop if no suitable position is found
            }
        }
    }

    Vector3 FindPositionForCan(float minX, float maxX, float minZ, float maxZ)
    {
        const int maxAttempts = 200; // Increased number of attempts
        int attempts = 0;
        while (attempts < maxAttempts)
        {
            Vector3 potentialPosition = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
            if (IsPositionValid(potentialPosition) && !IsTooCloseToOthers(potentialPosition))
            {
                occupiedPositions.Add(potentialPosition);
                return potentialPosition;
            }
            attempts++;
        }
        return Vector3.zero; // Return an invalid position if none is found
    }

    bool IsPositionValid(Vector3 position)
    {
        // Here you might check if the position is on the ground, not overlapping with buildings, etc.
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 50f, Vector3.down, out hit, 100f, groundLayerMask))
        {
            return true; // This simplification assumes all ground hits are valid
        }
        return false;
    }

    bool IsTooCloseToOthers(Vector3 position)
    {
        foreach (Vector3 otherPosition in occupiedPositions)
        {
            if (Vector3.Distance(position, otherPosition) < minDistanceBetweenCans)
            {
                return true;
            }
        }
        return false;
    }
}