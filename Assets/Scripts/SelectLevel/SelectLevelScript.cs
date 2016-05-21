using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectLevelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void selectLevel(int level)
    {
        Data.level = level;
        SceneManager.LoadScene("gameplay");
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }
}
