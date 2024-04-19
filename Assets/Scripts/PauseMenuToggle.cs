using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuToggle : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public GameObject pauseMenu;
    private bool paused;
    public GameObject gameOverCanvas;
    private GameOverMenuScript gameOverMenuScript;

    // Start is called before the first frame update
    void Start()
    {
        gameOverMenuScript = gameOverCanvas.GetComponent<GameOverMenuScript>();
    }
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        paused = false;
        canvasGroup.interactable = false; canvasGroup.blocksRaycasts = false; canvasGroup.alpha = 0f; Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        paused = true;
        canvasGroup.interactable = true; canvasGroup.blocksRaycasts = true; canvasGroup.alpha = 1f; Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false; // Uncomment if you want to use it in the editor
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("TESTCREATENEWSCENE");
        Unpause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !gameOverMenuScript.GetIsGameOver())
        {
            if (paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

    }
}
