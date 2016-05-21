using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public GameObject textGen;
    public bool isIntro;
    public bool isGameover;
    public bool isClear;
    public bool isPaused;
    public bool isGameplay;

    public AudioClip bgmMain;
    public AudioClip bgmStagePass;
    public AudioClip bgmGameOver;

    float ortoHeight;
    float ortoWidth;

    float countdown;

    bool isOutroDone;

    bool isFirstClear = true;

	// Use this for initialization
	void Start ()
    {
        countdown = 0;
        isIntro = true;
        isGameplay = false;
        isGameover = false;
        isClear = false;
        isPaused = false;
        isOutroDone = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //intro
        if (isIntro)
        {
            if (transform.FindChild("Countdown").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                transform.FindChild("CountdownTime").GetComponent<Animator>().SetBool("isIdle", false);
            }
            if (!transform.FindChild("CountdownTime").GetComponent<Animator>().GetBool("isIdle"))
            {
                countdown += Time.deltaTime;
                if (countdown < 2.99f)
                {
                    transform.FindChild("CountdownTime").GetComponent<Text>().text = (3 - Mathf.FloorToInt(countdown)).ToString();
                }
                else
                {
                    transform.FindChild("Countdown").GetComponent<Text>().text = "";
                    transform.FindChild("CountdownTime").GetComponent<Text>().text = "Go";
                    transform.FindChild("CountdownTime").GetComponent<Animator>().SetBool("isCountingdown", false);
                    if (countdown > 3.5f)
                    {
                        isIntro = false;
                        transform.FindChild("Countdown").gameObject.SetActive(false);
                        isGameplay = true;
                        transform.FindChild("InputText").GetComponent<AlphabetInputListener>().isControllerEnabled = true;
                    }
                }
            }
        }

        //masuk gameplay
        if (isGameplay)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().loop = true;
            }
            //cek gameover apa ngga
            if (HeartManager.getHeart() < 1)
            {
                isGameover = true;
                isGameplay = false;
                GetComponent<AudioSource>().Stop();
            } else if (transform.FindChild("TextGenerator").GetComponent<TextGeneratorScript>().wordNumToFinish <= 0)
            {
                isClear = true;
                isGameplay = false;
                GetComponent<AudioSource>().Stop();
            }
        }

        if (isClear)
        {
            transform.FindChild("GameOverCaption").FindChild("Mask").gameObject.SetActive(true);
            transform.FindChild("GameOverCaption").FindChild("BtnBackToMenu").gameObject.SetActive(true);
            transform.FindChild("GameOverCaption").FindChild("BtnRetry").gameObject.SetActive(true);
            transform.FindChild("GameOverCaption").FindChild("BtnRetry").transform.position = new Vector3(0, -2, 80);
            transform.FindChild("GameOverCaption").FindChild("BtnNextStage").gameObject.SetActive(true);

            if (!GetComponent<AudioSource>().isPlaying && !isOutroDone)
            {
                GetComponent<AudioSource>().clip = bgmStagePass;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().Play();
                isOutroDone = true;
            }
            for (int i = 0; i < textGen.transform.childCount; i++)
            {
                textGen.transform.GetChild(i).GetComponent<TextBehavior>().destroy();
            }
            textGen.GetComponent<TextGeneratorScript>().pauseGame(true);
            transform.FindChild("InputText").GetComponent<AlphabetInputListener>().isControllerEnabled = false;
            transform.FindChild("GameOverCaption").gameObject.SetActive(true);

            // random caption hanya sekali
            if (isFirstClear)
            {
                // transform.FindChild("GameOverCaption").GetComponent<Text>().text = randomWinCaption();
                transform.FindChild("GameOverCaption").FindChild("Text").gameObject.SetActive(true);
                transform.FindChild("GameOverCaption").FindChild("Text").GetComponent<Text>().text = randomWinCaption();
                isFirstClear = false;
            }


            transform.FindChild("InputText").transform.FindChild("InputTime").GetComponent<RectTransform>().sizeDelta = new Vector2(
                0,
                transform.FindChild("InputText").transform.FindChild("InputTime").GetComponent<RectTransform>().rect.height
            );
        }

        if (isGameover)
        {
            transform.FindChild("GameOverCaption").FindChild("Text").gameObject.SetActive(true);
            transform.FindChild("GameOverCaption").FindChild("Text").GetComponent<Text>().text = "Game Over";

            transform.FindChild("GameOverCaption").FindChild("Mask").gameObject.SetActive(true);
            transform.FindChild("GameOverCaption").FindChild("BtnBackToMenu").gameObject.SetActive(true);
            transform.FindChild("GameOverCaption").FindChild("BtnRetry").transform.position = transform.FindChild("GameOverCaption").FindChild("BtnNextStage").transform.position;
            transform.FindChild("GameOverCaption").FindChild("BtnRetry").gameObject.SetActive(true);
            transform.FindChild("GameOverCaption").FindChild("BtnNextStage").gameObject.SetActive(false);

            if (!GetComponent<AudioSource>().isPlaying && !isOutroDone)
            {
                GetComponent<AudioSource>().clip = bgmGameOver;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().Play();
                isOutroDone = true;
            }
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
        transform.FindChild("PauseMask").FindChild("PauseText").GetComponent<Text>().enabled = state;
    }

    public void startGame()
    {
        isGameplay = true;
    }

    public string randomWinCaption()
    {
        string[] captions = {"Good Job!", "Nice Job!", "Excellent!", "Great!"};
        return captions[Random.Range(0, captions.Length)];
    }
}
