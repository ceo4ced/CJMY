using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public float idleTime = 2.0f;  // Time in seconds after which the cursor will be hidden
    private float timer;
    private Vector3 lastMousePosition;

    private void Update()
    {
        // Check if the mouse has moved
        if (lastMousePosition != Input.mousePosition)
        {
            // If the mouse has moved, reset the timer and make sure the cursor is visible
            timer = 0;
            Cursor.visible = true;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            // If the mouse hasn't moved, increment the timer
            timer += Time.deltaTime;

            // If the timer exceeds the idle time, hide the cursor
            if (timer > idleTime)
            {
                Cursor.visible = false;
            }
        }
    }
}
