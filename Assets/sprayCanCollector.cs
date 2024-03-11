using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sprayCanCollector : MonoBehaviour
{
    public bool hasCan = false;
    private int count;
    public TextMeshProUGUI countText;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        SetCountText();

    }
    void SetCountText()
    {
        countText.text = "" + count.ToString();


    }
        public void ReceiveCan() { hasCan = true;
        count = count + 1;
        SetCountText();
    }
}
