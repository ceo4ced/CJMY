using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour
{
    // Drag all your sprites here in the Unity Editor
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Randomly select a sprite and assign it
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}