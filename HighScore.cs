using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public static HighScore S;
    public Text text;
    private static int score;
    public int highScore;

    public static int SCORE
    {
        get { return (score); }
        set
        {
            score = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Punkty: " + SCORE;
    }
}
