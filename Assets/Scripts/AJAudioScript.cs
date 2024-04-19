using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJAudioScript : MonoBehaviour
{
    private AudioSource audioSource;

    // Audio clips for different surfaces
    public AudioClip groundClip;
    public AudioClip grassClip;
    public AudioClip sandClip;
    public AudioClip waterClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // This function is called by the animation event
    public void PlayFootstep()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1f)) // Added a distance limit to the raycast
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Ground"))
            {
                audioSource.clip = groundClip;
                audioSource.volume = 0.080f; // Set volume for ground
            }
            else if (layer == LayerMask.NameToLayer("Grass"))
            {
                audioSource.clip = grassClip;
                audioSource.volume = 1f; // You can adjust this volume as needed
            }
            else if (layer == LayerMask.NameToLayer("Sand"))
            {
                audioSource.clip = sandClip;
                audioSource.volume = 0.2f; // You can adjust this volume as needed
            }
            else if (layer == LayerMask.NameToLayer("Water"))
            {
                audioSource.clip = waterClip;
                audioSource.volume = 0.5f; // You can adjust this volume as needed
            }

            audioSource.Play();
        }
    }
}
