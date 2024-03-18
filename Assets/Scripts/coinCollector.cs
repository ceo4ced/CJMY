using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coinCollector : MonoBehaviour
{
    public bool hasCoin = false;
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
        public void ReceiveCoin() { hasCoin = true;
        count = count + 1;
        SetCountText();
    }
}
