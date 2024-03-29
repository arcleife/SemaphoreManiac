﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextBehavior : MonoBehaviour {
    List<string> textDict; //dictionary text yang digenerate
    string text; //text keseluruhan
    string text2; //debug purpose
    public string textHighlighted; //text yang bener
    public string textRemaining; //text sisa
    public float fallSpeed;
    public Text inputText;
    bool isPaused;
    GameObject dictLoader; //gameobject buat load dictionary

	// Use this for initialization
	void Start () {

        //SEMENTARA//
        //input semua text yang ada di dictionary ke textDict trus dirandom isinya
        //entar bikin aja gameobject baru buat load dictionary biar gampang, jadi load dictionary baru kalo ganti level aja

        isPaused = false;
        inputText = this.transform.parent.transform.parent.FindChild("InputText").GetComponent<Text>();
        textHighlighted = "";
        textRemaining = text;
    }
	
	// Update is called once per frame
	void Update () {
        //assign highlighted text sama remaining text
        transform.FindChild("Highlighted").GetComponent<Text>().text = textHighlighted;
        transform.FindChild("Highlighted").transform.FindChild("Remaining").GetComponent<Text>().text = textRemaining;
        
        if (!isPaused)
        {
            //jatoh trus ancur kalo lewat kamera
            transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, 80);
            if (transform.position.y < TextGeneratorScript.ortoHeight * -0.5)
            {
                // Debug.Log("health minus!");
                HeartManager.decrement();
                TextGeneratorScript.shakeCamera();
                destroy();
            }
        }

        //ngatur highlight
        highlightCorrectSubstring();
	}

    void highlightCorrectSubstring()
    {
        if (text.Length < inputText.text.Length || inputText.text.Length == 0)
        {
            textHighlighted = "";
            transform.FindChild("Highlighted").GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.MinSize;
            textRemaining = text;
        }
        else
        {
            transform.FindChild("Highlighted").GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            int i = 0;
            bool isEnd = false;
            bool isUnmatch = false;
            while (i < inputText.text.Length && !isEnd && !isUnmatch)
            {
                if (text[i] == inputText.text[i])
                {
                    isUnmatch = false;
                    i++;
                }
                else if (text[i] != inputText.text[i])
                {
                    isUnmatch = true;
                }
                else
                {
                    isEnd = true;
                    isUnmatch = false;
                }
            }
            if (isUnmatch)
            {
                textHighlighted = "";
                transform.FindChild("Highlighted").GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.MinSize;
                textRemaining = text;
            }
            else
            {
                textHighlighted = text.Substring(0, i);
                textRemaining = text.Substring(i);
            }
        }
    }
    
    public void destroy()
    {
        Destroy(gameObject);
    }

    public void pause(bool i)
    {
        isPaused = i;
    }

    public void setText(string text)
    {
        this.text = text;
    }
}
