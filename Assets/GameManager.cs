using UnityEngine;
using TMPro; // Make sure you have this namespace to use TextMeshPro classes

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverCanvas;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI gameOverMessageText; // Add this line if not already added

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void TriggerGameOver(int finalScore, string message)
    {
        gameOverCanvas.SetActive(true);
        CanvasGroup gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;
            gameOverCanvasGroup.alpha = 1f;
            finalScoreText.text = "Final Score: " + finalScore.ToString();
            gameOverMessageText.text = message;  // Set the custom message
        }
    }
}