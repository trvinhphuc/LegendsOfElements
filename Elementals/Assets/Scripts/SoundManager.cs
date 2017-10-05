using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource efxsound;
    public AudioSource music;
    public static SoundManager instance = null;

    public float lowRange = .95f, highRange = 1.05f;

  
	// Use this for initialization
	void Awake () {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
	}

    public void Sliderchange(float newvalue)
    {
       music.volume = newvalue;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
