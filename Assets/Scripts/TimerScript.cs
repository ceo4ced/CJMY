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

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    private float timePassed = 0.0f;
    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 1f)
        {
            //do something
            if (--seconds < 0)
            {
                if (--minutes < 0)
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
                seconds = 59;
            }
            updateTimerText();
            timePassed = 0f;
        }
    }

    private void updateTimerText()
    {
        timerText.text = minutes.ToString().PadLeft(2, '\u0030') + ":" + seconds.ToString().PadLeft(2, '\u0030');
    }
}
