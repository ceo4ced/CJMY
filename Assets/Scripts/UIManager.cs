using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    // public GameObject characterMenu;

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

    // Call this to show Character Menu and hide Main Menu
    public void ShowCharacterMenu()
    {
        mainMenu.SetActive(false);
        // characterMenu.SetActive(true);
    }

    // Call this to quit the game (works in built games, not in the editor)
    public void ExitGame()
    {
        Application.Quit();
        // UnityEditor.EditorApplication.isPlaying = false; // Uncomment if you want to use it in the editor
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("SimpleTown_DemoScene");
    }

}
