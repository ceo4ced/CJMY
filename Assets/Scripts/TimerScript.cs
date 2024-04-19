using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    // Reference: https://stackoverflow.com/questions/61439740/how-can-i-make-an-action-repeat-every-x-seconds-with-timer-in-c

    public int minutes;
    public int seconds;
    TMP_Text timerText;
    public GameObject gameOverCanvas;
    public GameObject mainCharacterReference;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        timerCanvas.SetActive(true); // Show the timer canvas initially
        gameOverCanvas.SetActive(false); // Initially hide the game over canvas
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= 1f)
        {
            seconds--;
            if (seconds < 0)
            {
                if (minutes > 0)
                {
                    //timer is done
                    seconds = 0;
                    minutes = 0;

                    //Open game over menu
                    AJ_controller_Script ajController = mainCharacterReference.GetComponent<AJ_controller_Script>();
                    GameOverMenuScript gameOverMenuScript = gameOverCanvas.GetComponent<GameOverMenuScript>();
                    gameOverMenuScript.InitialiazeGameOverMenu("You Survived!", ajController.GetScore());
                    ajController.GameOver(true);

                    return;
                }
            }
            UpdateTimerText();
            timePassed = 0f;
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void GameOver()
    {
        timerCanvas.SetActive(false);  // Hide the timer canvas

        // Call GameManager to handle the game over process
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver(gameController.score, "You Survived!");
        }
    }
}
