using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    

    public Dictionary<string, int> TileScores = new Dictionary<string, int>()
    {
        { "green", 10 },
        { "blue", 10 },
        { "grey", 1 },
        { "brown", 1 },
        { "orange", 1 },
        { "black", 5 }
    };

    public void ScoreMatches(Dictionary<string, int> countMatches)
    {
        foreach (KeyValuePair<string, int> pair in countMatches)
        {

            //get the score value for the color in question
            Debug.Log("Key is " + pair.Key + " and value is " + pair.Value);
            int scoreValue = TileScores[pair.Key];
            //get number of matches
            int matchCount = pair.Value;
            //assign proportionally more (or less) points as the number of matches rises above 3
            int addScore = (matchCount * scoreValue) + Mathf.Max(0, (matchCount - 3) * 20) + Mathf.Max(0, (matchCount - 4) * 50);
            Debug.Log("Changing the score by " + addScore);
            this.GetComponent<HealthBar>().ChangeScore(addScore);
        }
    }


}
