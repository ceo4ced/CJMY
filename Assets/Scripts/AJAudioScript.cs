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
                audioSource.clip = groundClip;
            else if (layer == LayerMask.NameToLayer("Grass"))
                audioSource.clip = grassClip;
            else if (layer == LayerMask.NameToLayer("Sand"))
                audioSource.clip = sandClip;
            else if (layer == LayerMask.NameToLayer("Water"))
                audioSource.clip = waterClip;

            audioSource.Play();
        }
    }
}
