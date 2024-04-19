using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenuScript : MonoBehaviour
{
    public void Retry(){
        SceneManager.LoadScene("TESTCREATENEWSCENE");
    }
    public void Quit(){
        //exit game
    }
}
