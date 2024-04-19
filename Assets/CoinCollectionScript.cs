using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollectionScript : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private const int scorePerCoin = 50;
    private AudioSource coinSound;
    private bool markForDeletion;
    private bool collided;
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player") && !collided)
        {
            int score = int.Parse(scoreText.text.Substring(7)); // ignore "score: " for parse
            score += scorePerCoin;
            scoreText.text = "Score: " + score.ToString();
            coinSound.Play(0);
            GetComponent<MeshRenderer>().enabled = false;
            collided = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        coinSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, 300.0f * Time.deltaTime, Space.Self);
        if(coinSound.isPlaying)
        {
            markForDeletion = true;
        }
        if(!coinSound.isPlaying && markForDeletion)
        {
            this.gameObject.SetActive(false);
        }
    }
}
