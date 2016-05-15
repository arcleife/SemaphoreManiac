using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class InstructionManager : MonoBehaviour {
    public Sprite[] InstructionImage;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }

    public void setInstructionImage(char c)
    {
        c = Char.ToUpper(c);
        transform.FindChild("InstructionR").FindChild("InstructionImage").GetComponent<Image>().sprite = InstructionImage[c - 'A'];
        transform.FindChild("InstructionR").FindChild("InstructionImage").FindChild("letter").GetComponent<Text>().text = c.ToString();
    }
}
