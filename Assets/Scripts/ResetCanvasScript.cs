using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCanvasScript : MonoBehaviour
{
    // Start is called before the first frame update
    private CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        Debug.Log("Reset Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
