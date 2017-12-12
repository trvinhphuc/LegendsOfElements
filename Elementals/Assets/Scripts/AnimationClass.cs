using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClass {

	public Vector3 atkLocation, killLocation;
	public int eatAnim;
	public bool isPriority;
	public int angleAnim;
	public string killedName;
	public Vector2 blowRate = new Vector2();

	public void setInfo(Vector3 _atkLocation, Vector3 _killLocation,
		bool _isPriority, int _eatAnim, int _angleAnim, string _killedName) {

		this.killedName = _killedName;
		this.atkLocation = _atkLocation;
		this.killLocation = _killLocation;
		this.angleAnim = _angleAnim;
		this.isPriority = _isPriority;
		this.eatAnim = _eatAnim;
	}
		
	//For wind animation
	public Vector2 blowAngleRate() {
		Vector2 k = new Vector2();
		switch (this.angleAnim) {
		case 0:
			k = new Vector2 (1.0f, 0.0f);
			break;
		case 1:
			k = new Vector2 (1.0f, -1.0f);
			break;
		case 2:
			k = new Vector2 (0.0f, -1.0f);
			break;
		case 3:
			k = new Vector2 (-1.0f, -1.0f);
			break;
		case 4:
			k = new Vector2 (-1.0f, 0.0f);
			break;
		case 5:
			k = new Vector2 (-1.0f, 1.0f);
			break;
		case 6:
			k = new Vector2 (0.0f, 1.0f);
			break;
		case 7:
			k = new Vector2 (1.0f, 1.0f);
			break;
		}
		this.blowRate = k;
		return this.blowRate;
	}
}
