using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public GameObject textGen;
    public bool isIntro;
    public bool isGameover;
    public bool isPaused;
    public bool isGameplay;

    float ortoHeight;
    float ortoWidth;

    float countdown;

	// Use this for initialization
	void Start () {
        countdown = 0;
        isIntro = true;
        isGameplay = false;
        isGameover = false;
        isPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(transform.FindChild("Score").FindChild("ScoreText").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
        //intro
        if (isIntro)
        {
            countdown += Time.deltaTime;
            if (countdown < 2.99f)
            {
                transform.FindChild("Countdown").FindChild("CountdownTime").GetComponent<Text>().text = (3 - Mathf.FloorToInt(countdown)).ToString();
            }
            else
            {
                transform.FindChild("Countdown").GetComponent<Text>().text = "";
                transform.FindChild("Countdown").FindChild("CountdownTime").GetComponent<Text>().text = "Go";
                transform.FindChild("Countdown").FindChild("CountdownTime").GetComponent<Animator>().SetBool("isCountingdown", false);
                if (countdown > 3.5f)
                {
                    isIntro = false;
                    transform.FindChild("Countdown").gameObject.SetActive(false);
                    isGameplay = true;
                }
            }
        }

        //masuk gameplay
        if (isGameplay)
        {
            //cek gameover apa ngga
            if (HeartManager.getHeart() <= 0)
            {
                isGameover = true;
                isGameplay = false;
            }
        }

        if (isGameover)
        {
            for (int i = 0; i < textGen.transform.childCount; i++)
            {
                textGen.transform.GetChild(i).GetComponent<TextBehavior>().destroy();
            }
            textGen.GetComponent<TextGeneratorScript>().pauseGame(true);
            transform.FindChild("InputText").GetComponent<AlphabetInputListener>().isControllerEnabled = false;
            transform.FindChild("GameOverCaption").gameObject.SetActive(true);
            transform.FindChild("InputText").transform.FindChild("InputTime").GetComponent<RectTransform>().sizeDelta = new Vector2(
                0,
                transform.FindChild("InputText").transform.FindChild("InputTime").GetComponent<RectTransform>().rect.height);
        }
    }

    public void setPauseState(bool state)
    {
        transform.FindChild("InputText").GetComponent<AlphabetInputListener>().isControllerEnabled = !state;
        isPaused = state;
        textGen.GetComponent<TextGeneratorScript>().pauseGame(state);
        transform.FindChild("PauseMask").GetComponent<Image>().enabled = state;
    }

    public void startGame()
    {
        isGameplay = true;
    }
}
