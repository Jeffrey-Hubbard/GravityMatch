using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image currentScoreBar;
    public int playerScore = 0;
    public int maxScore;
    public int displayedScore;

    public Text playerScoreText;

	// Use this for initialization
	void Start () {
        UpdateScore();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerScore == 0)
        {
            displayedScore = playerScore;
            playerScoreText.text = displayedScore.ToString();
            float ratio = (float)displayedScore / (float)maxScore;
            currentScoreBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        }
		if (playerScore > displayedScore)
        {
            displayedScore += 1;
            playerScoreText.text = displayedScore.ToString();
            float ratio = (float)displayedScore / (float)maxScore;
            currentScoreBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        }
	}

    private void UpdateScore ()
    {
        
    }

    public void ChangeScore(int scoreDelta)
    {
        playerScore += scoreDelta;
    }
}
