using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenuScript : MonoBehaviour
{
    public void Retry(){
        SceneManager.LoadScene("TESTCREATENEWSCENE");
    }
    public void Quit()
    {
        Debug.Log("Quit function called");
        Application.Quit();

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
