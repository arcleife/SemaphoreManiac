using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public AudioClip bgm;

	// Use this for initialization
	void Start () {
        AudioSource.PlayClipAtPoint(bgm, new Vector3(0, 0, 0));
        DontDestroyOnLoad(bgm);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
