using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    // Reference: https://stackoverflow.com/questions/61439740/how-can-i-make-an-action-repeat-every-x-seconds-with-timer-in-c

    public int minutes;
    public int seconds;
    public GameObject timerCanvas; // Reference to the Canvas GameObject
    public GameObject gameOverCanvas; // Reference to the Game Over Canvas GameObject
    public TMP_Text finalScoreText; // Reference to the Text that will display the final score
    public int score; // Variable to hold the score, ensure this is updated during the game
    public AJ_controller_Script gameController;
    public TMP_Text gameOverMessageText;

    private TMP_Text timerText;
    private float timePassed = 0.0f;

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
                    minutes--;
                    seconds = 59;
                }
                else
                {
                    // Time's up, show game over
                    GameOver();
                    return; // Stop the timer
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
            GameManager.Instance.TriggerGameOver(gameController.score, "You Survived!", true);
        }
    }
}
