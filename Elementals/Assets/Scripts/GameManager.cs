using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	
	//public GameObject cell ;
	
	
	public int activePlayer = 1;
	// 1 = player 1, -1 = player 2
	public bool player1AI = false;
	public bool player2AI = false;


	public Text winText;
	public Button restartbtn;
	public Button quitbtn;

	public int gameState = 0;

	PieceClass piece = new PieceClass ();
	public List<GameObject> PiecePrefabs;
	private List<GameObject> activePiece = new List<GameObject> ();
	
	private GameObject SelectedPiece;
	// Selected Piece
	private float selectionX;
	private float selectionY;

	public Quaternion rotation = Quaternion.Euler (0, 0, 45);
	
	private Camera PlayerCam;

	// (0,-5.25) -> (-5.25,0)
	// (5.25,0)	-> (0,5.25)
	public Vector2 Bottom = Vector2.down * 5.25f;
	public Vector2 Left = Vector2.left * 5.25f;
	public Vector2 Right = Vector2.right * 5.25f;
	public Vector2 Top = Vector2.up * 5.25f;
	
	List<Vector2> around = new List<Vector2> ();
	// list of around a piece
	List<Vector2> passedTrack = new List<Vector2> ();
	public bool hop = false;
	List<Vector2> moves = new List<Vector2> ();

	void OnGUI ()
	{
		string _activePlayerColor;
		if (activePlayer == 1)
			_activePlayerColor = "Green";
		else
			_activePlayerColor = "Red";
		
		
		
	}
	
	// Initialize the board area
	void Start ()
	{
		createAllPieces ();
		DrawBoard ();

	}

	void createAllPieces ()
	{
		// player1
		CreatePiece ("Dark", 0f, -2.8f, 1);          //0
		CreatePiece ("Light", 0f, -4.2f, 1);			//1

		CreatePiece ("Fire", 0.35f, -3.85f, 1);		//2	
		CreatePiece ("Water", -0.35f, -3.85f, 1);	//3
		CreatePiece ("Earth", -0.7f, -3.5f, 1);		//4	
		CreatePiece ("Wind", 0.7f, -3.5f, 1);		//5

		CreatePiece ("Fire", -1.4f, -2.8f, 1);		//6
		CreatePiece ("Water", -1.05f, -3.15f, 1);	//7
		CreatePiece ("Earth", 1.05f, -3.15f, 1);		//8
		CreatePiece ("Wind", 1.4f, -2.8f, 1);		//9

		CreatePiece ("Fire", 2.1f, -2.8f, 1);		//10
		CreatePiece ("Water", 1.05f, -2.45f, 1);		//11
		CreatePiece ("Earth", -2.1f, -2.8f, 1);		//12
		CreatePiece ("Wind", -1.05f, -2.45f, 1);		//13

		// player2
		CreatePiece ("Dark", 0f, 2.8f, -1);			//14
		CreatePiece ("Light", 0f, 4.2f, -1);			//15

		CreatePiece ("Fire", 0.35f, 3.85f, -1);		//16
		CreatePiece ("Water", -0.35f, 3.85f, -1);	//17
		CreatePiece ("Earth", -0.7f, 3.5f, -1);		//18
		CreatePiece ("Wind", 0.7f, 3.5f, -1);		//19

		CreatePiece ("Fire", -1.4f, 2.8f, -1);		//20
		CreatePiece ("Water", -1.05f, 3.15f, -1);	//21
		CreatePiece ("Earth", 1.05f, 3.15f, -1);		//22
		CreatePiece ("Wind", 1.4f, 2.8f, -1);		//23

		CreatePiece ("Fire", 2.1f, 2.8f, -1);		//24
		CreatePiece ("Water", 1.05f, 2.45f, -1);		//25
		CreatePiece ("Earth", -2.1f, 2.8f, -1);		//26
		CreatePiece ("Wind", -1.05f, 2.45f, -1);		//27

		//CreateBoard();
	
	}

	void Update ()
	{
		UpdateSelection ();
		/*
		GameObject[] deboardList = GameObject.FindGameObjectsWithTag ("board");


		UpdateSelection();
		DrawBoard ();
		int isEnd = CheckEndGame ();
		print( isEnd);
		if ( isEnd == 0) {
			
		} else if (isEnd != 0) {
			

			foreach( GameObject de in deboardList){
				Destroy (de);
			}
			if (isEnd == 1) {
				winText.text = "Player 1 Win";
			}
			if (isEnd == -1) {
				winText.text = "Player 2 Win";
			}
		}
		*/

	}

	void UpdateSelection ()
	{
		if (!Camera.main)
			return;
		/*
		Vector2 _hitInfo = Camera.main.ScreenPointToRay (Input.mousePosition).origin;
		//Debug.Log (_hitInfo);
		if (Input.GetMouseButtonDown (0)) 
		{
			selectionX = _hitInfo.x;
			selectionY = _hitInfo.y;
			Debug.Log (_hitInfo);
		}
*/
	}
	// Create the board by placing cubes
	void CreateBoard ()
	{
		GameObject cell = null;
		Vector2 bot = Bottom + new Vector2 (0f, 0.35f);
		for (int k = 0; k <= 14; k++) {
			cell = PiecePrefabs [12];
			Vector2 run = Vector2.one * k * 0.35f;
			Vector2 run2 = bot + run;
			cell = Object.Instantiate (cell, run2, rotation) as GameObject;
			cell.tag = "board";

			for (int l = 0; l <= 14; l++) {
				cell = PiecePrefabs [12];
				run.Set (-1.0f, 1.0f);
				run = run * l * 0.35f;

				cell = Object.Instantiate (cell, run2 + run, rotation) as GameObject;
				cell.tag = "board";
			}

		}
	}

	void DrawBoard ()
	{
		




		for (int i = 0; i <= 15; i++) {
			Vector2 start = Vector2.one * i * 0.35f;
			Debug.DrawLine (Bottom + start, Left + start);

			for (int j = 0; j <= 15; j++) {
				start.Set (-1.0f, 1.0f);
				start = start * j * 0.35f;
				Debug.DrawLine (Bottom + start, Right + start);

			}

		}



		//cell = Object.Instantiate (cell,new Vector2(_posX,_posY), Quaternion.identity) as GameObject;
	}
	
	
	
	// Spawn a piece on the board
	
	void CreatePiece (string _pieceName, float _posX, float _posY, int _playerTag)
	{
		GameObject _PieceToCreate = null;
		int _pieceIndex = 0;
		//Select the right prefab to instantiate
		if (_playerTag == 1) {
			switch (_pieceName) {
			case "Dark": 
				_pieceIndex = 1;
				break;
			case "Light": 
				_pieceIndex = 0;
				break;
			case "Fire": 
				_pieceIndex = 3;
				break;
			case "Water": 
				_pieceIndex = 4;
				break;
			case "Earth": 
				_pieceIndex = 2;
				break;
			case "Wind": 
				_pieceIndex = 5;
				break;
			}
		} else if (_playerTag == -1) {
			switch (_pieceName) {
			case "Dark": 
				_pieceIndex = 6;
				break;
			case "Light": 
				_pieceIndex = 7;
				break;
			case "Fire": 
				_pieceIndex = 9;
				break;
			case "Water": 
				_pieceIndex = 10;
				break;
			case "Earth": 
				_pieceIndex = 8;
				break;
			case "Wind": 
				_pieceIndex = 11;
				break;
			}
		}
		
		_PieceToCreate = PiecePrefabs [_pieceIndex];
		// Instantiate the piece as a GameObject to be able to modify it after
		_PieceToCreate = Object.Instantiate (_PieceToCreate, new Vector2 (_posX, _posY), Quaternion.identity) as GameObject;
		_PieceToCreate.name = _pieceName + "p1";
		//_PieceToCreate.transform.SetParent(transform);
		activePiece.Add (_PieceToCreate);
		//Add material to the piece and tag it
		if (_playerTag == 1) {
			_PieceToCreate.tag = "1";
			_PieceToCreate.name = _pieceName + "p1";
		} else if (_playerTag == -1) {
			_PieceToCreate.tag = "-1";
			_PieceToCreate.name = _pieceName + "p2";
		}
	}
	// Tạo các clone để chọn nước đi
	public void CreateClone (GameObject _SelectedPiece, List<Vector2> _PossibleMoves)
	{
		GameObject clone;
		if (_PossibleMoves != null) {
			foreach (Vector2 move in _PossibleMoves) {
				clone = Object.Instantiate (_SelectedPiece, move, Quaternion.identity) as GameObject;
				clone.tag = "clone";
				clone.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1f, 0.5f);
			}
		}
	}


	// thay vì chỉ duyệt 1 ô thì duyệt 2 ô liên tiếp nhau, hàm test move sẽ kiểm tra xem là tại vi trí đó có quân nào rồi hay chưa
	public void PossibleMove (GameObject _SelectedPiece)
	{
		GameObject clone;

		if (_SelectedPiece != null) {
			if (hop) {
				for (int a = 0; a < passedTrack.Count; a++) {
					print (passedTrack [a]);
				}
				GetAroundPieces (_SelectedPiece);
				for (int i = 0; i <= 7; i++) {
					if (!TestMovement (_SelectedPiece, around [i])) {
						if (TestMovement (_SelectedPiece, around [i + 8])) {
							
							if (!passedTrack.Contains (around [i + 8])) {
								clone = Object.Instantiate (_SelectedPiece, around [i + 8], Quaternion.identity) as GameObject;
								clone.tag = "cloneH";
								clone.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1f, 0.5f);
							}
						}
					}
				}
			} else {
				GetAroundPieces (_SelectedPiece);
				for (int i = 0; i <= 7; i++) {
					if (TestMovement (_SelectedPiece, around [i])) {
					
						clone = Object.Instantiate (_SelectedPiece, around [i], Quaternion.identity) as GameObject;
						clone.tag = "clone";
						clone.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1f, 0.5f);

					} else if (TestMovement (_SelectedPiece, around [i + 8])) {
						clone = Object.Instantiate (_SelectedPiece, around [i + 8], Quaternion.identity) as GameObject;
						clone.tag = "cloneH";
						clone.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1f, 0.5f);
					}

				}
			}

		
		}		
	}

	public void DecloneMove ()
	{
		GameObject[] decloneList = GameObject.FindGameObjectsWithTag ("clone");
		foreach (GameObject declone1 in decloneList) {
			DestroyImmediate (declone1);
		}
		GameObject[] decloneListS = GameObject.FindGameObjectsWithTag ("cloneH");
		foreach (GameObject declone2 in decloneListS) {
			DestroyImmediate (declone2);
		}
	}

	
	//Update SlectedPiece with the GameObject inputted to this function
	public void SelectPiece (GameObject _PieceToSelect)
	{
		
		// Unselect the piece if it was already selected
		if (_PieceToSelect == SelectedPiece) {
			SelectedPiece.GetComponent<Renderer> ().material.color = Color.white;
			SelectedPiece = null;
			ChangeState (0);
			DecloneMove ();
			if (hop) {
				DecloneMove ();
				hop = false;
				passedTrack.Clear ();
				activePlayer = -activePlayer;
			}
		} else {
			// Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
			if (SelectedPiece) {
				SelectedPiece.GetComponent<Renderer> ().material.color = Color.white;
				DecloneMove ();
			}
			SelectedPiece = _PieceToSelect;
			SelectedPiece.GetComponent<Renderer> ().material.color = new Color (0f, 0f, 5f, 0.8f);
			PossibleMove (_PieceToSelect);
			ChangeState (1);
		}


	}
	
	// Move the SelectedPiece to the inputted coords
	public void MovePiece (GameObject _objMove)
	{
		
		Vector2 _coordPiece = new Vector2 (SelectedPiece.transform.position.x, SelectedPiece.transform.position.y);
		Vector2 _coordToMove = new Vector2 (_objMove.transform.position.x, _objMove.transform.position.y);
		//Debug.Log (_coordPiece);
		// Don't move if the user clicked on its own cube or if there is a piece on the cube
		if ((_coordToMove.x != _coordPiece.x || _coordToMove.y != _coordPiece.y)) {	
			if (_objMove.tag == "clone") {
				SelectedPiece.transform.position = new Vector2 (_coordToMove.x, _coordToMove.y);		// Move the piece
				EatPiece (SelectedPiece, activePlayer);
				SelectedPiece.GetComponent<Renderer> ().material.color = Color.white;	// Change it's color back
				SelectedPiece = null;									// Unselect the Piece
				ChangeState (0);
				DecloneMove ();
				activePlayer = -activePlayer;
			} else {
				hop = true;
				DecloneMove ();
				SelectedPiece.transform.position = new Vector2 (_coordToMove.x, _coordToMove.y);		// Move the piece
				EatPiece (SelectedPiece, activePlayer);
				passedTrack.Add (_coordPiece);
				PossibleMove (SelectedPiece);
				if (GameObject.FindGameObjectsWithTag ("cloneH").Length == 0) {
					Debug.Log ("check");
					hop = false;
					passedTrack.Clear ();
					SelectedPiece.GetComponent<Renderer> ().material.color = Color.white;
					SelectedPiece = null;
					ChangeState (0);
					activePlayer = -activePlayer;
				}
					
			}

		}
	}
	// Get list pieces around selected piece
	/*
	tạo thêm 1 bộ vecto cách vị trí cần duyệt 2 ô 
	*/
	public List<Vector2> GetAroundPieces (GameObject _SelectedPiece)
	{
		float pos_x = _SelectedPiece.gameObject.transform.position.x;
		float pos_y = _SelectedPiece.gameObject.transform.position.y;
		
		Vector2 N = new Vector2 (pos_x, pos_y + 0.7f);
		Vector2 E = new Vector2 (pos_x + 0.7f, pos_y);
		Vector2 S = new Vector2 (pos_x, pos_y - 0.7f);
		Vector2 W = new Vector2 (pos_x - 0.7f, pos_y);
		Vector2 NE = new Vector2 (pos_x + 0.35f, pos_y + 0.35f);
		Vector2 SE = new Vector2 (pos_x + 0.35f, pos_y - 0.35f);
		Vector2 SW = new Vector2 (pos_x - 0.35f, pos_y - 0.35f);
		Vector2 NW = new Vector2 (pos_x - 0.35f, pos_y + 0.35f);
		
		Vector2 N2 = new Vector2 (pos_x, pos_y + 1.4f);
		Vector2 E2 = new Vector2 (pos_x + 1.4f, pos_y);
		Vector2 S2 = new Vector2 (pos_x, pos_y - 1.4f);
		Vector2 W2 = new Vector2 (pos_x - 1.4f, pos_y);
		Vector2 NE2 = new Vector2 (pos_x + 0.7f, pos_y + 0.7f);
		Vector2 SE2 = new Vector2 (pos_x + 0.7f, pos_y - 0.7f);
		Vector2 SW2 = new Vector2 (pos_x - 0.7f, pos_y - 0.7f);
		Vector2 NW2 = new Vector2 (pos_x - 0.7f, pos_y + 0.7f);
		
		around.Clear ();
		around.Add (N);
		around.Add (E);
		around.Add (S);
		around.Add (W);
		around.Add (NE);
		around.Add (SE);
		around.Add (SW);
		around.Add (NW); ///
		// ô xung quanh thứ 2
		around.Add (N2);
		around.Add (E2);
		around.Add (S2);
		around.Add (W2);
		around.Add (NE2);
		around.Add (SE2);
		around.Add (SW2);
		around.Add (NW2);

		return around;
	}
	// If the movement is legal, eat the piece
	public void EatPiece (GameObject _SelectedPiece, int _playerTag)
	{
		
		GetAroundPieces (_SelectedPiece);
		//print (around[1]+"aaa");

		if (_playerTag == -1) {
			for (int i = 0; i <= 13; i++) {
				for (int j = 0; j <= 7; j++) {
					if ((Mathf.Abs (around [j].x - activePiece [i].gameObject.transform.position.x) <= 0.1) && (Mathf.Abs (around [j].y - activePiece [i].gameObject.transform.position.y) <= 0.1)) {
						if ((_SelectedPiece.name == "Darkp2") || (activePiece [i].name == "Darkp1")) {
							if (activePiece [i].name != "Lightp1")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							
						} else if (_SelectedPiece.name == "Windp2") {
							if (activePiece [i].name == "Waterp1")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							if (activePiece [i].name == "Firep1")
								_SelectedPiece.GetComponent<Renderer> ().enabled = false;
						} else if (_SelectedPiece.name == "Waterp2") {
							if (activePiece [i].name == "Earthp1")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							if (activePiece [i].name == "Windp1")
								_SelectedPiece.GetComponent<Renderer> ().enabled = false;
						} else if (_SelectedPiece.name == "Earthp2") {
							if (activePiece [i].name == "Firep1")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							if (activePiece [i].name == "Waterp1")
								_SelectedPiece.GetComponent<Renderer> ().enabled = false;
						} else if (_SelectedPiece.name == "Firep2") {
							if (activePiece [i].name == "Windp1")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							if (activePiece [i].name == "Earthp1")
								_SelectedPiece.GetComponent<Renderer> ().enabled = false;
						}
						
					}
				}

			}
		}
		if (_playerTag == 1) {
			for (int i = 14; i <= 27; i++) {
				for (int j = 0; j <= 7; j++) {
					if ((Mathf.Abs (around [j].x - activePiece [i].gameObject.transform.position.x) <= 0.1) && (Mathf.Abs (around [j].y - activePiece [i].gameObject.transform.position.y) <= 0.1)) {
						if ((_SelectedPiece.name == "Darkp1") || (activePiece [i].name == "Darkp2")) {
							if (activePiece [i].name != "Lightp2")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							
						} else if (_SelectedPiece.name == "Windp1") {
							if (activePiece [i].name == "Waterp2")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							if (activePiece [i].name == "Firep2")
								_SelectedPiece.GetComponent<Renderer> ().enabled = false;
						} else if (_SelectedPiece.name == "Waterp1") {
							if (activePiece [i].name == "Earthp2")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							if (activePiece [i].name == "Windp2")
								_SelectedPiece.GetComponent<Renderer> ().enabled = false;
						} else if (_SelectedPiece.name == "Earthp1") {
							if (activePiece [i].name == "Firep2")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							if (activePiece [i].name == "Waterp2")
								_SelectedPiece.GetComponent<Renderer> ().enabled = false;
						} else if (_SelectedPiece.name == "Firep1") {
							if (activePiece [i].name == "Windp2")
								activePiece [i].GetComponent<Renderer> ().enabled = false;
							if (activePiece [i].name == "Earthp2")
								_SelectedPiece.GetComponent<Renderer> ().enabled = false;
						}
					}
				}
			}
		}

			
		
		

	}
	
	// Test if the piece can do the player's movement
	/* 
	*/
	bool TestMovement (GameObject _SelectedPiece, Vector2 _coordToMove)
	{
		
		bool _movementLegalBool = true;
		bool _collisionDetectBool = false;
		
		//_collisionDetectBool = true;
		Vector2 _coordPiece = new Vector2 (_SelectedPiece.transform.position.x, _SelectedPiece.transform.position.y);


		if (_coordToMove.magnitude > 4.3) {
			_movementLegalBool = false;
		}
		//Debug.Log (gameState);

		for (int a = 0; a <= 27; a++) {
			if ((Mathf.Abs (activePiece [a].gameObject.transform.position.x - _coordToMove.x) <= 0.01) && (Mathf.Abs (activePiece [a].gameObject.transform.position.y - _coordToMove.y) <= 0.01)) {
				_movementLegalBool = false;
				break;
			}
		}
		return (_movementLegalBool && !_collisionDetectBool);

		//return true;
	}
	

	// Change the state of the game
	public void ChangeState (int _newState)
	{
		gameState = _newState;
	}

	public int CheckEndGame ()
	{
		
		if (activePiece [1].gameObject.transform.position.x == 0f && activePiece [1].gameObject.transform.position.y == 0f) {
			
			return 1;
		} else if (activePiece [15].gameObject.transform.position.x == 0f && activePiece [15].gameObject.transform.position.y == 0f) {
			
			return -1;
		}

		return 0;
	}

	public void EndGame ()
	{
		Application.Quit ();
	}
	
}
