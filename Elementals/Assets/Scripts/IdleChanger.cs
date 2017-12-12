﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class IdleChanger : MonoBehaviour
{
	GameManager _animGame;
	AnimationClass lightEnd;

	void Start() {
		_animGame = GetComponent<GameManager> ();
		lightEnd = new AnimationClass ();
		lightEnd.atkLocation = new Vector3 (0.0f, 0.0f, 0.5f);
		lightEnd.eatAnim = 6;
	}

	void OnGUI() {
		StartCoroutine (multiProcessAnim ());
		//StartCoroutine (test ());
	}

	IEnumerator multiProcessAnim() {
		//Play animation in game
		if (_animGame.animList != null && _animGame.isEnd == 0) {
			//Play animation with high priority first
			foreach (AnimationClass index in _animGame.animList.ToArray()) {
				if (index.isPriority) {
					Debug.Log (index.eatAnim);
					//_animGame.multiAnimPlaying = true;
					_animGame.AnimLocate (index);
					StartCoroutine (_animGame.AnimProcess (index));
					yield return new WaitForSeconds (_animGame.duration);
					//_animGame.multiAnimPlaying = false;
				}
			}

			//Play animation with low priority
			foreach (AnimationClass index in _animGame.animList.ToArray()) {
				if (!index.isPriority ) {
					Debug.Log (index.eatAnim);
					//_animGame.multiAnimPlaying = true;
					_animGame.AnimLocate (index);
					StartCoroutine (_animGame.AnimProcess (index));
					yield return new WaitForSeconds (_animGame.duration);
					//_animGame.multiAnimPlaying = false;
				}
			}
			_animGame.animList.Clear ();
		}
		//Play animation when game end
		else {
			if (_animGame.isEnd != 0) {
				_animGame.AnimLocate (lightEnd);
				StartCoroutine (_animGame.AnimProcess (lightEnd));
			}
		}
		yield return null;
	}

}

