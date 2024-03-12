using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class itemCollector : MonoBehaviour
{
    private int itemCount;
    public TextMeshProUGUI countText;
    // Start is called before the first frame update
    void Start()
    {
        itemCount = 0;
        SetCountText();

    }
    void SetCountText()
    {
        countText.text = "" + itemCount.ToString();


    }
        public void RecieveItem() {
        itemCount = itemCount + 1;
        SetCountText();
    }
}
