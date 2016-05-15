using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Thalmic.Myo;

public class AlphabetInputListener : MonoBehaviour {
    
    public int maxStringLength;
    public bool isUseKeyboard = true;
    public bool isUseMyo = false;
    public GameObject textGen;

    public bool isControllerEnabled;

    private DancematController danceMat;
    private ThalmicMyo myo = null;
    private Text inputText;
    private TextGeneratorScript textGenerator;

    private const float MAX_TIME = 0.65f;

    private string lastChar = "";
    private float inputTimeLeft = MAX_TIME;

    private Pose lastPose = Pose.Unknown;
    private float deleteTimeLeft = MAX_TIME;

    float delayInputTransition = 0.3f;
    float delayTimer = 0;
    bool isOnInputDelay = false;

    void Start () 
    {
        //isControllerEnabled = true;
        if (!isUseKeyboard)
        {
            danceMat = GetComponent<DancematController>();
        }

        if (isUseMyo)
        {
            myo = GameObject.Find("Myo").GetComponent<ThalmicMyo>();
        }


        inputText = GetComponent<Text>();
        textGenerator = textGen.GetComponent<TextGeneratorScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (transform.parent.GetComponent<GameStateManager>().isGameplay)
            {
                transform.parent.GetComponent<GameStateManager>().isPaused = !transform.parent.GetComponent<GameStateManager>().isPaused;
                transform.parent.GetComponent<GameStateManager>().setPauseState(transform.parent.GetComponent<GameStateManager>().isPaused);
            } else if (transform.parent.GetComponent<GameStateManager>().isIntro)
            {
                //fungsi button pas lagi intro disini
            }
        }
        if (isControllerEnabled)
        {
            if (!isUseKeyboard)
            {
                danceMatAction();
            }
            else
            {
                keyboardAction();
            }

            if (isUseMyo)
            {
                myoAction();
            }
        }
    }

    private void danceMatAction()
    {
        float inputtimebarMaxWidth = 510;
        float widthAdder = inputtimebarMaxWidth / MAX_TIME * Time.deltaTime;
        
        // input huruf dari Dancemat        
        if (danceMat.huruf == "")
        {
            inputTimeLeft = MAX_TIME;
            lastChar = danceMat.huruf;
            transform.FindChild("InputTime").GetComponent<RectTransform>().sizeDelta = new Vector2(
                0,
                transform.FindChild("InputTime").GetComponent<RectTransform>().rect.height);
        }
        else
        {
            if (inputText.text.Length < maxStringLength && !isOnInputDelay && transform.parent.GetComponent<GameStateManager>().isGameplay)
            {
                inputTimeLeft -= Time.deltaTime;
                transform.FindChild("InputTime").GetComponent<RectTransform>().sizeDelta = new Vector2(
                    transform.FindChild("InputTime").GetComponent<RectTransform>().rect.width + widthAdder,
                    transform.FindChild("InputTime").GetComponent<RectTransform>().rect.height);
                if (danceMat.huruf != lastChar)
                {
                    inputTimeLeft = MAX_TIME;
                    transform.FindChild("InputTime").GetComponent<RectTransform>().sizeDelta = new Vector2(
                    0,
                    transform.FindChild("InputTime").GetComponent<RectTransform>().rect.height);
                    lastChar = danceMat.huruf;
                }
                else
                {
                    if (inputTimeLeft < 0)
                    {
                        inputText.text += lastChar;
                        inputTimeLeft = MAX_TIME;
                        transform.FindChild("InputTime").GetComponent<RectTransform>().sizeDelta = new Vector2(
                        0,
                        transform.FindChild("InputTime").GetComponent<RectTransform>().rect.height);
                        isOnInputDelay = true;
                    }
                }
            }
            else
            {
                delayTimer += Time.deltaTime;
                if (delayTimer > delayInputTransition)
                {
                    isOnInputDelay = false;
                    delayTimer = 0;
                }
            }
        } //> input huruf dari Dancemat
    }

    private void myoAction()
    {
        // hapus karakter menggunakan pose WaveIn
        if (myo.pose == Pose.Unknown || myo.pose == Pose.Rest)
        {
            deleteTimeLeft = MAX_TIME;
            lastPose = myo.pose;
        }
        else
        {
            deleteTimeLeft -= Time.deltaTime;

            if (myo.pose != lastPose)
            {
                deleteTimeLeft = MAX_TIME;
                lastPose = myo.pose;
            }
            else
            {
                if (deleteTimeLeft < 0)
                {
                    if (lastPose == Pose.WaveIn)
                    {
                        if (inputText.text.Length > 0)
                        {
                            inputText.text = inputText.text.Substring(0, inputText.text.Length - 1);
                        }
                    }
                    else if (lastPose == Pose.Fist)
                    {
                        textGenerator.cek();
                    }

                    deleteTimeLeft = MAX_TIME;
                }
            }
        } //> hapus karakter menggunakan pose WaveIn
    }

    private void keyboardAction()
    {
        //listen input keyboard trus ngubah string di input sesuai dengan inputan
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // delete or backspace
            {
                //delete char
                if (GetComponent<Text>().text.Length > 0)
                {
                    inputText.text = inputText.text.Substring(0, inputText.text.Length - 1);
                }
            }
            else if (c == '\n' || c == '\r') // enter
            {
                //trigger action
                //Debug.Log("Hai");
                textGenerator.cek();
                //inputText.text = "";
            }

            // append char
            if (inputText.text.Length < maxStringLength)
            {
                if (c >= 'a' && c <= 'z') 
                {
                    inputText.text += c;
                    inputText.text = inputText.text.ToUpper();
                }
            }
        }
    }
}
