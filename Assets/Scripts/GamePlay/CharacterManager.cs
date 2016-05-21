using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
    public Sprite[] characters;

    public void setCharacter(char c)
    {
        c = Char.ToUpper(c);
        Debug.Log("CHAR = " + c);

        transform.GetComponent<Image>().sprite = characters[c - 'A' + 1];
    }

    public void setIdle()
    {
        transform.GetComponent<Image>().sprite = characters[0];
    }
}
