using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TextGeneratorScript : MonoBehaviour {
    public int maxTextOnScreen;
    public GameObject textGameObject;
    public GameObject gameStateManager;

    public float generateDelay;
    float curTime;

    float ortoWidth;
    float ortoHeight;

    bool isPaused;

    List<string> textDict; //dictionary text yang digenerate

    // Use this for initialization
    void Start () {
        isPaused = false;
        ortoHeight = 2 * Camera.main.orthographicSize;
        ortoWidth = ortoHeight * Camera.main.aspect;
        //Debug.Log("ortoHeight = " + ortoHeight);
        //Debug.Log("ortoWidth = " + ortoWidth);
        // load soal
        // nanti disesuaikan sama level
        setLevel(1);
        curTime = generateDelay;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isPaused && gameStateManager.GetComponent<GameStateManager>().isGameplay)
        {
            curTime += Time.deltaTime;
        }

        if (curTime > generateDelay)
        {
            if (transform.childCount < maxTextOnScreen)
            {
                string text;
                // random soal
                text = textDict[Random.Range(0, textDict.Count)];
                // bikin object
                GameObject newText = (GameObject) Instantiate(textGameObject);
                newText.transform.SetParent(transform);
                newText.transform.localScale = new Vector3 (1, 1, 1);
                newText.transform.position = new Vector3(Random.Range(10, Screen.width - 150), Screen.height + 5);
                newText.GetComponent<TextBehavior>().setText(text);
                newText.transform.FindChild("Highlighted").FindChild("Remaining").GetComponent<ContentSizeFitter>().enabled = true;
                //Debug.Log (newText.transform.FindChild("Highlighted").FindChild("Remaining").GetComponent<RectTransform>().rect.width);
                if (text.Length == 3)
                {
                    newText.transform.FindChild("Frame").transform.position = new Vector3(newText.transform.FindChild("Frame").transform.position.x - 4, newText.transform.FindChild("Frame").transform.position.y);
                } else if (text.Length == 5)
                {
                    newText.transform.FindChild("Frame").transform.position = new Vector3(newText.transform.FindChild("Frame").transform.position.x + 3, newText.transform.FindChild("Frame").transform.position.y);
                }
            }
            curTime = 0;
        }

        //biar kalo di layar teks nya 0 langsung spawn yang baru
        if (transform.childCount <= 0 && transform.parent.gameObject.GetComponent<GameStateManager>().isGameplay)
        {
            curTime = generateDelay + 0.1f;
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
                //Debug.Log("beneeeer");
                ScoreManager.increment(10);
                transform.GetChild(i).GetComponent<TextBehavior>().destroy();
                transform.parent.FindChild("InputText").GetComponent<Text>().text = "";
                isMatch = true;
                break;
            }
        }
        if (!isMatch)
        {
            //Debug.Log("salaaaah");
            HeartManager.decrement();
        }
    }

    public void setLevel(int level)
    {
        textDict = LevelLoader.getDict(level);
        //ngatur difficulty
        switch (level)
        {
            case 1:
                maxTextOnScreen = 5;
                generateDelay = 5;
                break;
            case 2:
                maxTextOnScreen = 5;
                generateDelay = 5;
                break;
            default:
                break;
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
        transform.parent.FindChild("InputText").transform.FindChild("NextLetter").GetComponent<Text>().text = transform.parent.FindChild("InputText").GetComponent<DancematController>().huruf;
    }
}
