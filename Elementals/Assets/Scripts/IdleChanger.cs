using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleChanger : MonoBehaviour {
	private Animator anim;						
	private AnimatorStateInfo currentState;		
	private AnimatorStateInfo previousState;
	private GameManager _GameManager;

	void Start () {
		anim = GetComponent<Animator> ();
		currentState = anim.GetCurrentAnimatorStateInfo (0);
		previousState = currentState;
		_GameManager = gameObject.GetComponent<GameManager>();
	}

	// Update is called once per frame
	void OnGUI () {
		
		anim.SetInteger ("eatIndex", _GameManager.eatIndex);
		Debug.Log (_GameManager.eatIndex);
	}
}
