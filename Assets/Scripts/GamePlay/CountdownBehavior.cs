using UnityEngine;
using System.Collections;

public class CountdownBehavior : MonoBehaviour {
    public AudioClip sfxTick;
    public AudioClip sfxJang;

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = sfxTick;
        GetComponent<AudioSource>().loop = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void playSFXTick()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = sfxTick;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
    }

    void playSFXJang()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = sfxJang;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
    }
}
