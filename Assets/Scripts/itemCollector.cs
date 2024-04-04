using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class itemCollector : MonoBehaviour
{
    private bool hasItem = false; // Flag to track if the player has an item
    //private int itemCount;
    private string currentColor;
    public TextMeshProUGUI countText;

    // Start is called before the first frame update
    void Start()
    {
        //itemCount = 0;
        currentColor = "None";
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "Spraycan: " + currentColor;
    }
    public void ReceiveItem(string itemTag)
    {
        if (!hasItem)
        {
            hasItem = true;
            if (itemTag == "redCan")
            {
                currentColor = "Red";
            }
            else if (itemTag == "blueCan")
            {
                currentColor = "Blue";
            }
            else if (itemTag == "greenCan")
            {
                currentColor = "Green";
            }
            SetCountText();
        }
    }

//public void RecieveItem(string itemTag)
//   {
//        if (!hasItem) // Check if the player doesn't already have an item
//        {
//            hasItem = true; // Mark the player as having an item
//            //itemCount++;
//            SetCountText();
//        }
//    }

    // Method to reset the hasItem flag
    public void ResetItem()
    {
        hasItem = false;
        currentColor = "None";
        SetCountText();
    }

    // Method to check if the player has an item
    public bool HasItem()
    {
        return hasItem;
    }
}
