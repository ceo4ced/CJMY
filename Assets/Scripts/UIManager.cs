using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject manualMenu;
    public GameObject prestartMenu;
    public GameObject creditsMenu;

    void Start()
    {
        ShowMainMenu();
    }
    // Call this to show Main Menu and hide Character Menu
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        manualMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    // Call this to show Manual Menu and hide Main Menu
    public void ShowManualMenu()
    {
        mainMenu.SetActive(false);
        manualMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    // Call this to show Credits Menu and hide Main Menu
    public void ShowCreditsMenu()
    {
        mainMenu.SetActive(false);
        manualMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    // Call this to show Manual Menu and hide Main Menu
    public void ShowPrestartMenu()
    {
        mainMenu.SetActive(false);
        manualMenu.SetActive(false);
        creditsMenu.SetActive(false);
        prestartMenu.SetActive(true);
    }


    // Call this to quit the game (works in built games, not in the editor)
    public void ExitGame()
    {
        Application.Quit();
        // UnityEditor.EditorApplication.isPlaying = false; // Uncomment if you want to use it in the editor
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("TESTCREATENEWSCENE");
    }

}
