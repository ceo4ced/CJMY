using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopAudioScript : MonoBehaviour
{
    private AudioSource footstepAudioSource;
    private AudioSource sirenAudioSource;
    private Transform playerTransform;
    private PoliceAIWaypoint policeAI;
    public AudioClip footstepSound;
    public AudioClip sirenSound;
    public float maxHearingDistance = 20f; // Maximum distance at which footsteps can be heard

    void Start()
    {
        // Initialize both AudioSources
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            Debug.LogError("Expected at least two AudioSource components on the GameObject");
            return;
        }
        footstepAudioSource = audioSources[0];
        sirenAudioSource = audioSources[1];

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        policeAI = GetComponentInParent<PoliceAIWaypoint>(); // Assuming this script is a child of the police object
    }

    void Update()
    {
        if ((policeAI.currentState == PoliceAIWaypoint.CopState.Chase ||
             policeAI.currentState == PoliceAIWaypoint.CopState.Attack) && !sirenAudioSource.isPlaying)
        {
            PlaySiren();
        }
        else if (policeAI.currentState != PoliceAIWaypoint.CopState.Chase &&
                 policeAI.currentState != PoliceAIWaypoint.CopState.Attack && sirenAudioSource.isPlaying)
        {
            StopSiren();
        }
    }

    public void PlayFootstep()
    {
        if (playerTransform != null)
        {
            float volume = CalculateVolumeBasedOnDistance();
            if (volume > 0)
            {
                footstepAudioSource.volume = volume;
                footstepAudioSource.PlayOneShot(footstepSound);
            }
        }
    }

    private float CalculateVolumeBasedOnDistance()
    {
        if (playerTransform == null) return 0f;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        return Mathf.Clamp01(1 - (distance / maxHearingDistance));
    }

    private void PlaySiren()
    {
        sirenAudioSource.clip = sirenSound;
        sirenAudioSource.loop = true;
        sirenAudioSource.volume = 1.0f; // Adjust as necessary
        sirenAudioSource.Play();
    }

    private void StopSiren()
    {
        sirenAudioSource.Stop();
        // It is not necessary to clear the clip here unless you need to
    }
}
