using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    private static Text scoreText;
    private static int score = 0;

	// Use this for initialization
	void Start ()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
	}

    public static void reset()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    public static void increment(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public static void decrement(int value)
    {
        score -= value;
        scoreText.text = score.ToString();
    }
}
