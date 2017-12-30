using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMove : MonoBehaviour {
	private bool m = false;
	private GameManager _GameManager;
	private GameObject selected;
	private Vector2 _coordPiece;
	private Vector2 _coordToMove;
	private int activeplayer = 0;
	private float moveSpeed = 3.0f;
	// Use this for initialization
	void Start () {
		_GameManager = gameObject.GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update () {
		if (m) {
			if (!(Mathf.Abs(selected.transform.position.x - _coordToMove.x)<0.02f && Mathf.Abs(selected.transform.position.y - _coordToMove.y)<0.02f)) {
				selected.transform.Translate (new Vector2 ((_coordToMove.x - _coordPiece.x) * Time.deltaTime * moveSpeed, (_coordToMove.y - _coordPiece.y) * Time.deltaTime * moveSpeed));
				//Debug.Log (selected.transform.position.y);
			} else {
				selected.transform.position = new Vector2 ( _coordToMove.x , _coordToMove.y );
				Eat (selected, activeplayer);
				m = false;
			}
		}
	}

	public void Move( GameObject _selected, Vector2 _move, int _activeplayer){

		selected = _selected;
		_coordPiece = new Vector2 (_selected.transform.position.x, _selected.transform.position.y);
		_coordToMove = _move;
		activeplayer = _activeplayer;
		m = true;
	}
	public void Eat(GameObject _selected, int activeplayer){
		_GameManager.EatPiece (_selected, activeplayer);
	}
	public int a(){
		return 1;
	}
}
