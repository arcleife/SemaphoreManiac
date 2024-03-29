﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TextGeneratorScript : MonoBehaviour
{
    public int maxTextOnScreen;
    public GameObject textGameObject;
    public GameObject gameStateManager;
    public GameObject scoreOutCaps;

    public float generateDelay;
    int wordIDToSpawn;
    float curTime;
    float randomAdditionalTime = 1f;

    public static float ortoWidth;
    public static float ortoHeight;

    public int wordNumToFinish; // berapa kata yang harus diancurin biar lolos stage
    int tutorialID; // tutorial mana yang dimunculin (0 buat tanpa tutorial)
    int tutorialWordCounter; // jumlah kata yang udah keluar pas tutorial
    float textFallSpeed;

    bool isPaused;

    List<string> textDict; //dictionary text yang digenerate
    List<string> rightTextDict; //dictionary IDtext yang udah bener jawabnya, gunanya pas tutorial aja

    public AudioClip RightAnswerSFX;
    public AudioClip WrongAnswerSFX;

    public bool isEndless = false;

    private static Transform localTransform;
    public static AudioClip localWrongAnswerSFX;

    // Use this for initialization
    void Start ()
    {
        if (Data.level == 0) isEndless = true;

        Debug.Log("Anda memilih level = " + Data.level);

        localTransform = transform;
        localWrongAnswerSFX = WrongAnswerSFX;

        isPaused = false;
        wordIDToSpawn = 0;
        tutorialWordCounter = 0;

        ortoHeight = 2 * Camera.main.orthographicSize;
        ortoWidth = ortoHeight * Camera.main.aspect;
        // Debug.Log("ortoHeight = " + ortoHeight);
        // Debug.Log("ortoWidth = " + ortoWidth);

        // load soal
        // nanti disesuaikan sama level
        setLevel(Data.level);

        curTime = generateDelay;
        rightTextDict = new List<string>();
        if (tutorialID > 0)
        {
            transform.parent.FindChild("Instruction").gameObject.SetActive(true);
        } else
        {
            transform.parent.FindChild("Instruction").gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(wordIDToSpawn);
        if (!isPaused && gameStateManager.GetComponent<GameStateManager>().isGameplay)
        {
            curTime += Time.deltaTime;
        }

        if (curTime > generateDelay)
        {
            if (transform.childCount < maxTextOnScreen)
            {
                string text;
                // tutorial apa ngga, kalo tutorial selalu jamin minimal tiap word keluar
                if (tutorialID < 1)
                {
                    wordIDToSpawn = Random.Range(0, textDict.Count);
                    text = textDict[Random.Range(0, textDict.Count)];
                } else
                {
                    tutorialWordCounter++;
                    text = textDict[wordIDToSpawn % textDict.Count];
                    wordIDToSpawn++;
                    wordIDToSpawn %= textDict.Count;
                    if (tutorialWordCounter / textDict.Count >= 1)
                    {
                        // berarti udah 1 loop, cek kalo yang keluar kata yang udah bener langsung dinext
                        while (rightTextDict.Exists(x => x == textDict[wordIDToSpawn]))
                        {
                            wordIDToSpawn++;
                            wordIDToSpawn %= textDict.Count;
                        }
                    }
                    if (transform.parent.FindChild("Instruction") != null)
                    {
                        transform.parent.FindChild("Instruction").GetComponent<InstructionManager>().setInstructionImage(text[0]);
                    }
                }
                // bikin object
                GameObject newText = (GameObject) Instantiate(textGameObject);
                newText.transform.SetParent(transform);
                newText.transform.localScale = new Vector3 (1, 1, 1);
                newText.transform.position = new Vector3(Random.Range(-ortoWidth/2+3.3f, ortoWidth / 2 - 5f), ortoHeight/2, 0);
                newText.GetComponent<TextBehavior>().setText(text);
                newText.GetComponent<TextBehavior>().fallSpeed = textFallSpeed;
                newText.transform.FindChild("Highlighted").FindChild("Remaining").GetComponent<ContentSizeFitter>().enabled = true;
                if (text.Length == 3)
                {
                    newText.transform.FindChild("Frame").transform.position = new Vector3(newText.transform.FindChild("Frame").transform.position.x - 4, newText.transform.FindChild("Frame").transform.position.y);
                } else if (text.Length == 5)
                {
                    newText.transform.FindChild("Frame").transform.position = new Vector3(newText.transform.FindChild("Frame").transform.position.x + 3, newText.transform.FindChild("Frame").transform.position.y);
                }
            }
            curTime = 0 - Random.Range(0, randomAdditionalTime);
        }

        //biar kalo di layar teks nya 0 langsung spawn yang baru
        if (transform.childCount <= 0 && transform.parent.gameObject.GetComponent<GameStateManager>().isGameplay)
        {
            curTime = generateDelay + 0.1f;
        }

        //reset animation
        
        if (transform.parent.FindChild("Score").FindChild("ScoreText").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            transform.parent.FindChild("Score").FindChild("ScoreText").GetComponent<Animator>().SetBool("isScoreUp", false);
        }

        updateNextLetter();
    }

    public void cek()
    {
        //destroy kalo ada kata di layar yang bener
        bool isMatch = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<TextBehavior>().textRemaining == "")
            {
                transform.parent.FindChild("Score").FindChild("ScoreText").GetComponent<Animator>().SetBool("isScoreUp", true);
                int scoreGet = 10 * transform.parent.FindChild("InputText").GetComponent<Text>().text.Length + HeartManager.getHeart();
                showScoreOutAnim(scoreGet, transform.GetChild(i).transform.position.x + 0.5f, transform.GetChild(i).transform.position.y, 80);
                ScoreManager.increment(scoreGet);
                transform.GetChild(i).GetComponent<TextBehavior>().destroy();
                if (tutorialID > 0)
                {
                    rightTextDict.Add(transform.parent.FindChild("InputText").GetComponent<Text>().text);
                }
                transform.parent.FindChild("InputText").GetComponent<Text>().text = "";
                isMatch = true;
                wordNumToFinish--;
                AudioSource.PlayClipAtPoint(RightAnswerSFX, new Vector3(0, 0, 0));
                break;
            }
        }
        if (!isMatch)
        {
            transform.parent.GetComponent<CameraController>().ShakeCamera(0.05f, 0.2f);
            HeartManager.decrement();
            AudioSource.PlayClipAtPoint(WrongAnswerSFX, new Vector3(0, 0, 0));
        }
    }

    public static void shakeCamera()
    {
        localTransform.parent.GetComponent<CameraController>().ShakeCamera(0.05f, 0.2f);
        AudioSource.PlayClipAtPoint(localWrongAnswerSFX, new Vector3(0, 0, 0));
    }

    public void setLevel(int level)
    {
        if (isEndless)
            textDict = LevelLoader.getDict(1);
        else
            textDict = LevelLoader.getDict(level);

        //ngatur difficulty
        switch (level)
        {
            case 0:
                // mode endless
                maxTextOnScreen = 3;
                generateDelay = 4;
                tutorialID = 0;
                wordNumToFinish = 3;
                textFallSpeed = 0.01f;
                break;

            case 1:
                // tutorial 1
                // pengenalan 5 huruf pertama
                maxTextOnScreen = 1;
                generateDelay = 2;
                tutorialID = 1;
                wordNumToFinish = textDict.Count;
                textFallSpeed = 0.01f;
                break;

            case 2:
                maxTextOnScreen = 2;
                generateDelay = 7;
                wordNumToFinish = 8;
                tutorialID = 0;
                textFallSpeed = 0.005f;
                break;

            case 3:
                maxTextOnScreen = 5;
                generateDelay = 5;
                wordNumToFinish = 5;
                tutorialID = 1;
                textFallSpeed = 0.01f;
                break;

            case 4:
                maxTextOnScreen = 2;
                generateDelay = 7;
                wordNumToFinish = 10;
                tutorialID = 0;
                textFallSpeed = 0.02f;
                break;

            case 5:
                maxTextOnScreen = 3;
                generateDelay = 7;
                wordNumToFinish = 15;
                tutorialID = 0;
                textFallSpeed = 0.02f;
                break;

            case 6:
                maxTextOnScreen = 5;
                generateDelay = 5;
                wordNumToFinish = 5;
                tutorialID = 1;
                textFallSpeed = 0.01f;
                break;

            case 9:
                maxTextOnScreen = 5;
                generateDelay = 5;
                wordNumToFinish = 5;
                tutorialID = 1;
                textFallSpeed = 0.01f;
                break;

            case 12:
                maxTextOnScreen = 5;
                generateDelay = 5;
                wordNumToFinish = 5;
                tutorialID = 1;
                textFallSpeed = 0.01f;
                break;

            default:
                maxTextOnScreen = 2;
                generateDelay = 7;
                wordNumToFinish = 10;
                tutorialID = 0;
                textFallSpeed = 0.002f;
                break;
        }

        if (tutorialID > 0)
        {
            wordNumToFinish = textDict.Count;
            maxTextOnScreen = 1;
        }
    }

    public void pauseGame(bool pause)
    {
        isPaused = pause;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<TextBehavior>().pause(pause);
        }
    }

    void updateNextLetter()
    {
        string huruf = transform.parent.FindChild("InputText").GetComponent<DancematController>().huruf;
        transform.parent.FindChild("InputText").transform.FindChild("NextLetter").GetComponent<Text>().text = huruf;

        if (huruf != "")
            transform.parent.FindChild("Input").transform.FindChild("Image").GetComponent<CharacterManager>().setCharacter(huruf[0]);
        else
            transform.parent.FindChild("Input").transform.FindChild("Image").GetComponent<CharacterManager>().setIdle();
    }

    void showScoreOutAnim(int score, float x, float y, float z)
    {
        GameObject scoreCaps = (GameObject)Instantiate(scoreOutCaps);
        scoreCaps.transform.SetParent(transform.parent);
        scoreCaps.transform.localScale = new Vector3(1, 1, 1);
        scoreCaps.transform.position = new Vector3(x, y, z);
        scoreCaps.GetComponent<Text>().text = score.ToString();
    }
}
