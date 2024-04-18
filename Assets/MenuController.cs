using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; // Assign this in the inspector

    public void SkipButtonClicked()
    {
        menuPanel.SetActive(false); // Hide the menu panel when Skip is clicked
    }
}
