using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuToggle : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public GameObject pauseMenu;
    public GameObject gameOverCanvas;
    private GameManager gameManager;
    private bool paused;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameOverCanvas.GetComponent<GameManager>();
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
        if (Input.GetKeyUp(KeyCode.Escape) && !gameManager.isGameOver)
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
