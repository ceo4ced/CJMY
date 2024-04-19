using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; // Assign this in the inspector
    public GameObject Background;

    void Start()
    {
        // Pause the game when the menu is active
        if (menuPanel.activeSelf)
        {
            Time.timeScale = 0;
        }
    }

    public void SkipButtonClicked()
    {
        // Hide the menu panel when Skip is clicked
        menuPanel.SetActive(false);
        Background.SetActive(false);

        // Resume the game
        Time.timeScale = 1;
    }

    void OnEnable()
    {
        // Ensure the game is paused when the menu is enabled
        Time.timeScale = 0;
    }

    void OnDisable()
    {
        // Ensure the game resumes when the menu is not visible
        Time.timeScale = 1;
    }
}