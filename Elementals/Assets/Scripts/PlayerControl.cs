﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	
	private Camera PlayerCam;			// Camera used by the player
	private GameManager _GameManager; 	// GameObject responsible for the management of the game

	private float zoom = 5;

	private int _activePlayer;
	private bool _player1AI;
	private bool _player2AI;
	private bool minimax;
	private int Depth = 1;
	// Use this for initialization
	void Start () 
	{
		PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // Find the Camera's GameObject from its tag 
		_GameManager = gameObject.GetComponent<GameManager>();
		_player1AI = _GameManager.player1AI;
		_player2AI = true;
		minimax =  true;

		//Debug.Log(_player2AI);
		
	}
	
	// Update is called once per frame
	void Update () {
		// Look for Mouse Inputs
		_activePlayer = _GameManager.activePlayer;
		if ((_activePlayer == 1 && _player1AI == false) || (_activePlayer == -1 && _player2AI == false)) {
			//Debug.Log (_player2AI);
			GetMouseInputs ();
		} else {
			if (!minimax) {
				int r;
				do {
					r = Random.Range (14, 27);
				} while(_GameManager.activePiece [r].transform.position.x > 50);
				_GameManager.SelectPiece (_GameManager.activePiece [r]);
				GameObject[] moves = GameObject.FindGameObjectsWithTag ("clone");
				int m = Random.Range (0, moves.Length - 1);
				_GameManager.MovePiece (moves [m]);
			} else {
				//_GameManager.State = null;
				_GameManager.SetState (_GameManager.activePiece);
				_GameManager.best_move = null;
				//_GameManager.best_score = 0;
				print (_GameManager.Minimax (_GameManager.State,Depth,_activePlayer));
				_GameManager.MovePiece (_GameManager.best_move);
				
				print (_GameManager.best_move.PieceName);
				print (_GameManager.best_move.MoveCoord);
				_activePlayer = -_activePlayer;
			}
		}
	}
	
	// Detect Mouse Inputs
	void GetMouseInputs()
	{	
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			if (zoom > 2)
				zoom -= 1;
			PlayerCam.transform.position.Set(2f,2f,-10f);

		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			if (zoom < 5)
				zoom += 1;
			PlayerCam.transform.position.Set(-2f,-2f,-10f);
		}
		PlayerCam.transform.position = new Vector3(-2f,-2f,-10f);
		print(PlayerCam.transform.position.x);
		PlayerCam.orthographicSize = zoom;
		_activePlayer = _GameManager.activePlayer;
		Ray _ray;
		RaycastHit _hitInfo;

		// Select a piece if the gameState is 0 or 1
		if(_GameManager.gameState < 2)
		{
			// On Left Click
			if(Input.GetMouseButtonDown(0))
			{
				_ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click

				// Raycast and verify that it collided
				//if (Physics.Raycast (_ray, out _hitInfo, 25.0f, LayerMask.GetMask ("board"))) {
					//Debug.Log("Hit2");
					// Select the piece if it has the good Tag
					//Debug.Log(_GameManager.gameState);
					//print (_hitInfo.collider.gameObject.tag);

				//}
				if (Physics.Raycast (_ray, out _hitInfo, 25.0f, LayerMask.GetMask ("piece"))) {
						if (_hitInfo.collider.gameObject.tag == (_activePlayer.ToString ())) {
							//Debug.Log ("Hit3");
							_GameManager.SelectPiece (_hitInfo.collider.gameObject);

						}
				}
				
			}
		}
	
		// Move the piece if the gameState is 1
		if(_GameManager.gameState == 1)
		{
			Vector2 selectedCoord;

			// On Left Click
			if(Input.GetMouseButtonDown(0))
			{
				_ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click

				// Raycast and verify that it collided
				if(Physics.Raycast (_ray,out _hitInfo))
				{
					
					//print (_hitInfo.collider.gameObject.tag);
					// If the ray hit a cube, move. If it hit a piece of the other player, eat it.
					if(_hitInfo.collider.gameObject.tag.Contains("clone"))
					{   /*
						selectedCoord = new Vector2(_hitInfo.collider.gameObject.transform.position.x,_hitInfo.collider.gameObject.transform.position.y);
						_GameManager.MovePiece(selectedCoord);
						*/
						_GameManager.MovePiece(_hitInfo.collider.gameObject);
					}
					else if(_hitInfo.collider.gameObject.tag == ((-1*_activePlayer).ToString()))
					{
						//_GameManager.EatPiece(_hitInfo.collider.gameObject);

						Debug.Log ("Select");
					}
				}
			}	
		}
	}
}