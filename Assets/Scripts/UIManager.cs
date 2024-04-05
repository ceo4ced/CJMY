using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject manualMenu;

    void Start()
    {
        ShowMainMenu();
    }
    // Call this to show Main Menu and hide Character Menu
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        // characterMenu.SetActive(false);
    }

    // Call this to show Manual Menu and hide Main Menu
    public void ShowManualMenu()
    {
        mainMenu.SetActive(false);
        manualMenu.SetActive(true);
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
