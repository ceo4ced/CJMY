using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenuScript : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public TMPro.TextMeshProUGUI gameOverLabel;
    public TMPro.TextMeshProUGUI finalScoreText;
    private bool isGameOver = false;

    public void Retry()
    {
        SceneManager.LoadScene("TESTCREATENEWSCENE");
    }
    public void Quit()
    {
        //exit game
        Application.Quit();
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void InitialiazeGameOverMenu(string labelText, int score)
    {
        isGameOver = true;
        gameOverLabel.text = labelText;
        finalScoreText.text = "" + score;
        CanvasGroup gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
        if (!gameOverCanvasGroup.interactable)
        {
            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;
            gameOverCanvasGroup.alpha = 1f;
        }
    }
    public bool GetIsGameOver()
    {
        return isGameOver;
    }
}
