using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{

    public int SCORE = 0;

    public Text ScoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        ScoreDisplay.text = SCORE.ToString();
    }

    public void Score(int points)
    {
        SCORE += points;
        ScoreDisplay.text = SCORE.ToString();
    }
}
