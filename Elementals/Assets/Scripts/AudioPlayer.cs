using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
	public AudioSource animAudio;
	public AudioClip auAttack;
	//public AudioClip auKill;

	void Awake() {
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("music");
		if (objs.Length > 1)
			Destroy (this.gameObject);
		DontDestroyOnLoad(this.gameObject);
	}

//	void Update() {
//		Debug.Log ("Audio's playing");
//		animAudio.clip = auAttack;
//		animAudio.Play ();
//	}
}
