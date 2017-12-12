using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GameManager : MonoBehaviour
{

	public int activePlayer = 1;
	// 1 = player 1, -1 = player 2
	public bool player1AI = false;
	public bool player2AI = true;


	public Text winText;
	public Button restartbtn;
	public Button quitbtn;

	public int gameState = 0;

	private PieceClass piece = new PieceClass ();
	public List<PieceClass> State = new List<PieceClass> ();
	public int[,] matrix = new int[15,15];

	public List<GameObject> PiecePrefabs;
	public List<GameObject> activePiece = new List<GameObject> ();
	public List<GameObject> graveyard = new List<GameObject> ();


	private GameObject SelectedPiece;
	// Selected Piece
	private float selectionX;
	private float selectionY;

	public Quaternion rotation = Quaternion.Euler (0, 0, 45);

	// (0,-5.25) -> (-5.25,0)
	// (5.25,0)	-> (0,5.25)
	public Vector2 Bottom = Vector2.down * 5.25f;
	public Vector2 Left = Vector2.left * 5.25f;
	public Vector2 Right = Vector2.right * 5.25f;
	public Vector2 Top = Vector2.up * 5.25f;



	// list of around a piece
	List<Vector2> passedTrack = new List<Vector2> ();

	public MoveClass best_move = new MoveClass ();
	public float startSpeed = 0.0f;
	public float moveSpeed = 0.1f;
	public float fragment = 10.0f;
	SlowMove m;


	public bool hop = false;
	public int isEnd = 0;
	private int Max_Depth = 2;

	//Animation variables declaration
	int[] dead = new int[29];

	public bool isAttacking = false;
	protected Animation animation;
	protected Animator animator;
	Vector3 killedVtor, attackVtor = new Vector3();
	public float duration = 1.5f;
	public List<AnimationClass> animList = new List<AnimationClass>();
	GameObject[] subLocationN = new GameObject[14];
	public List<GameObject> animLightAtked = new List<GameObject> ();
	public Vector3 moveWind = new Vector3(0.0f, 0.0f, 0.0f);
	private float windSpeed = 0.05f;
	private Vector3 windTermVector;
	public bool multiAnimPlaying = false;

	private Vector3 cameraLocation = new Vector3();
	private int cameraSize = 2;
	private Camera PlayerCam;

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
		animator = GetComponentInChildren<Animator>();
		PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		m = gameObject.GetComponent<SlowMove>();
		//DrawBoard ();

		// (activePiece);
		//printState ();
	}

	void createAllPieces ()
	{
		// player1
		CreatePiece ("Dark", 0f, -2.8f, 1);         //0
		CreatePiece ("Light", 0f, -4.2f, 1);		//1

		CreatePiece ("1Fire", 0.35f, -3.85f, 1);		//2	
		CreatePiece ("1Water", -0.35f, -3.85f, 1);	//3
		CreatePiece ("1Earth", -0.7f, -3.5f, 1);		//4	
		CreatePiece ("1Wind", 0.7f, -3.5f, 1);		//5

		CreatePiece ("2Fire", -1.4f, -2.8f, 1);		//6
		CreatePiece ("2Water", -1.05f, -3.15f, 1);	//7
		CreatePiece ("2Earth", 1.05f, -3.15f, 1);	//8
		CreatePiece ("2Wind", 1.4f, -2.8f, 1);		//9

		CreatePiece ("3Fire", 2.1f, -2.8f, 1);		//10
		CreatePiece ("3Water", 1.05f, -2.45f, 1);	//11
		CreatePiece ("3Earth", -2.1f, -2.8f, 1);		//12
		CreatePiece ("3Wind", -1.05f, -2.45f, 1);	//13

		// player2
		CreatePiece ("Dark", 0f, 2.8f, -1);			//14
		CreatePiece ("Light", 0f, 4.2f, -1);		//15

		CreatePiece ("1Fire", 0.35f, 3.85f, -1);		//16
		CreatePiece ("1Water", -0.35f, 3.85f, -1);	//17
		CreatePiece ("1Earth", -0.7f, 3.5f, -1);		//18
		CreatePiece ("1Wind", 0.7f, 3.5f, -1);		//19

		CreatePiece ("2Fire", -1.4f, 2.8f, -1);		//20
		CreatePiece ("2Water", -1.05f, 3.15f, -1);	//21
		CreatePiece ("2Earth", 1.05f, 3.15f, -1);	//22
		CreatePiece ("2Wind", 1.4f, 2.8f, -1);		//23

		CreatePiece ("3Fire", 2.1f, 2.8f, -1);		//24
		CreatePiece ("3Water", 1.05f, 2.45f, -1);	//25
		CreatePiece ("3Earth", -2.1f, 2.8f, -1);		//26
		CreatePiece ("3Wind", -1.05f, -1.75f, -1);	//27

		//CreateBoard();
	
	}

	void Update ()
	{
		UpdateSelection ();
		DrawBoard ();
		if (isAttacking) StartCoroutine(cameraController ());

		if (animator) {
			
		}


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
			if (_pieceName.Contains ("Dark"))
				_pieceIndex = 0;
			
			if (_pieceName.Contains ("Light"))
				_pieceIndex = 1;

			if (_pieceName.Contains ("Fire"))
				_pieceIndex = 3;
				
			if (_pieceName.Contains ("Water"))
				_pieceIndex = 4;
			
			if (_pieceName.Contains ("Earth"))
				_pieceIndex = 2;
			
			if (_pieceName.Contains ("Wind"))
				_pieceIndex = 5;
				
		} else if (_playerTag == -1) {
			if (_pieceName.Contains ("Dark"))
				_pieceIndex = 6;
		
			if (_pieceName.Contains ("Light"))
				_pieceIndex = 7;
		
			if (_pieceName.Contains ("Fire"))
				_pieceIndex = 9;
				
			if (_pieceName.Contains ("Water"))
				_pieceIndex = 10;
				
			if (_pieceName.Contains ("Earth"))
				_pieceIndex = 8;
				
			if (_pieceName.Contains ("Wind"))
				_pieceIndex = 11;
				
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

	public void SetState (List<GameObject> pieces)
	{
		State.Clear();
		//matrix = new int[15, 15];
		PieceClass p = new PieceClass ();
		for (int i = 0; i <= 27; i++) {
			State.Add (piece.SetPiece (pieces [i]));
			//int[] a = p.GetMatrix (pieces [i]);
			int piece_index;
			if (i < 14)
				piece_index = i + 1;
			else
				piece_index = 13 - i;
			//matrix [a [0],a [1]] = piece_index;
		}

	}
	public int[,] SetMatrix(List<PieceClass> pieces)
	{
		PieceClass p = new PieceClass ();
		int[,] m = new int[15, 15];
		for (int i = 0; i <= 27; i++) { 
			
	//		int[] a = p.GetMatrix (pieces [i]);
			int piece_index;
			if (i < 14)
				piece_index = i + 1;
			else
				piece_index = 13 - i;
	//		if (!((a [0] <= 0) || (a [0] >= 14) || (a [1] <= 0) || (a [1] >= 14))) 
	//			m [a [0],a [1]] = piece_index;
		}
		return m;
	}

	//Test
	public void printState ()
	{
		for (int i = 0; i <= 5; i++) {
			for (int j = 0; j <= 5; j++) {
				print (matrix[i,j]);
			}
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
				List<Vector2> around = GetAroundPieces (_SelectedPiece.transform.position.x,_SelectedPiece.transform.position.y);
				for (int i = 0; i <= 7; i++) {
					if (TestMovement (_SelectedPiece, around [i]) == _SelectedPiece.tag) {
						if (TestMovement (_SelectedPiece, around [i + 8]) == "0") {
							bool check = true;
							for (int a = 0; a < passedTrack.Count; a++) {
								if (passedTrack [a] == around [i + 8])
									check = false;
							}
							if (check) {
								//Debug.Log (around [i + 8]);
								clone = Object.Instantiate (_SelectedPiece, around [i + 8], Quaternion.identity) as GameObject;
								clone.tag = "cloneH";
								clone.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1f, 0.5f);
							}
						}

					}
				}
			} else {
				List<Vector2> around = GetAroundPieces (_SelectedPiece.transform.position.x,_SelectedPiece.transform.position.y);
				for (int i = 0; i <= 7; i++) {
					if (TestMovement (_SelectedPiece, around [i]) == "0") {
						clone = Object.Instantiate (_SelectedPiece, around [i], Quaternion.identity) as GameObject;
						clone.tag = "clone";
						clone.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1f, 0.5f);
					} else if (TestMovement (_SelectedPiece, around [i]) == _SelectedPiece.tag && !_SelectedPiece.name.Contains ("Light")) {
						if (TestMovement (_SelectedPiece, around [i + 8]) == "0") {
							clone = Object.Instantiate (_SelectedPiece, around [i + 8], Quaternion.identity) as GameObject;
							clone.tag = "cloneH";
							clone.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 1f, 0.5f);
						}
					}

				}
			}

		
		}		
	}

	// use for AI
	public List<MoveClass> PossibleMove (List<PieceClass> state , PieceClass _SelectedPiece)
	{
		List<MoveClass> Moves = new List<MoveClass> ();
		int[,] matrx = new int[15,15];
		matrx = SetMatrix(state);

		if (_SelectedPiece != null) {
			
				List<Vector2> around = GetAroundPieces (_SelectedPiece.pos_x,_SelectedPiece.pos_y);
				for (int i = 0; i <= 7; i++) {
				if (UnrealTestMovement (state, _SelectedPiece, around [i]) == 0) {
						MoveClass m = new MoveClass ();
						m.PieceName = _SelectedPiece.PieceName;
						m.MoveCoord = around [i];
						Moves.Add (m);

				} else if (UnrealTestMovement (state, _SelectedPiece, around [i]) == _SelectedPiece.tag_player) {
						if (!_SelectedPiece.PieceName.Contains ("Light")) {
						if (UnrealTestMovement (state, _SelectedPiece, around [i + 8]) == 0) {
								MoveClass m = new MoveClass ();
								List<Vector2> pass = new List<Vector2>();
								m.PieceName = _SelectedPiece.PieceName;
								m.MoveCoord = around [i + 8];
								m.hop = true;
								Moves.Add (m);

								List<PieceClass> state2 = UnrealMovePiece (state, m);
								_SelectedPiece.pos_x = m.MoveCoord.x;
								_SelectedPiece.pos_y = m.MoveCoord.y;
								pass.Add (m.MoveCoord);
								List<MoveClass> moves = new List<MoveClass> ();
							
								moves = FindAllHopMoves (state2, _SelectedPiece,pass);

								if (moves.Count != 0) {
									for (int a = 0; a < moves.Count; a++) {
										MoveClass m2 = new MoveClass ();
										m2.PieceName = m.PieceName;
										m2.MoveCoord = m.MoveCoord;
										m2.hop = true;
										m2.p_next = moves [a];

										Moves.Add (m2);
									}
								}

							}
						}
					}

				}

		}

		return Moves;

	}
	private List<MoveClass> FindAllHopMoves(List<PieceClass> state,PieceClass _SelectedPiece, List<Vector2> pass  ){
		List<MoveClass> Moves = new List<MoveClass> ();
		int[,] matrx = new int[15,15];
		matrx = SetMatrix(state);
		if (_SelectedPiece != null) {
			List<Vector2> around = GetAroundPieces (_SelectedPiece.pos_x, _SelectedPiece.pos_y);
			for (int i = 0; i <= 7; i++) {
				if (UnrealTestMovement (state, _SelectedPiece, around [i]) == _SelectedPiece.tag_player) {
					if (UnrealTestMovement (state, _SelectedPiece, around [i + 8]) == 0) {
						bool check = true;
						for (int a = 0; a < pass.Count; a++) {
							if (pass [a] == around [i + 8])
								check = false;
						}
						if (check) {
							MoveClass m = new MoveClass ();
							m.PieceName = _SelectedPiece.PieceName;
							m.MoveCoord = around [i + 8];
							m.hop = true;
							Moves.Add (m);

							List<PieceClass> state2 = UnrealMovePiece (state, m);
							_SelectedPiece.pos_x = m.MoveCoord.x;
							_SelectedPiece.pos_y = m.MoveCoord.y;
							pass.Add (m.MoveCoord);
							List<MoveClass> moves = FindAllHopMoves (state2, _SelectedPiece, pass);
							if (moves.Count != 0) {
								for (int a = 0; a < moves.Count; a++) {
									MoveClass m2 = new MoveClass ();
									m2.PieceName = m.PieceName;
									m2.MoveCoord = m.MoveCoord;
									m2.hop = true;
									m2.p_next = moves [a];
									Moves.Add (m2);
								}

							} 
						}

					}
				}
			}
		}
		return Moves;
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
			SelectedPiece.GetComponent<Renderer> ().material.color = new Color (0f, 0f, 1f, 0.8f);
			PossibleMove (_PieceToSelect);
			ChangeState (1);
		}


	}

	// Move the SelectedPiece to the inputted coords
	public IEnumerator MovePiece (GameObject _objMove)
	{
		
		Vector2 _coordPiece = new Vector2 (SelectedPiece.transform.position.x, SelectedPiece.transform.position.y);
		Vector2 _coordToMove = new Vector2 (_objMove.transform.position.x, _objMove.transform.position.y);
		Vector2 move = _coordPiece - _coordToMove;


		// Don't move if the user clicked on its own cube or if there is a piece on the cube
		if ((_coordToMove.x != _coordPiece.x || _coordToMove.y != _coordPiece.y)) {	
			if (_objMove.tag == "clone") {

				m.Move(SelectedPiece,_coordToMove, activePlayer);

				SelectedPiece.GetComponent<Renderer> ().material.color = Color.white;	// Change it's color back
				SelectedPiece = null;									// Unselect the Piece
				ChangeState (0);
				DecloneMove ();
				activePlayer = -activePlayer;
				turnUpdate ();
			} else {
				hop = true;
				DecloneMove ();
				SelectedPiece.transform.position = new Vector2 (_coordToMove.x, _coordToMove.y);		// Move the piece
				EatPiece (SelectedPiece, activePlayer);
				passedTrack.Add (_coordPiece);
				PossibleMove (SelectedPiece);
				if (GameObject.FindGameObjectsWithTag ("cloneH").Length == 0) {
					
					hop = false;
					passedTrack.Clear ();
					SelectedPiece.GetComponent<Renderer> ().material.color = Color.white;
					SelectedPiece = null;
					ChangeState (0);
					activePlayer = -activePlayer;
				}
					
			}
			yield return null;
		}
	}

	public void turnUpdate() {
		for (int i = 0; i < 28; i++) {
			if (dead[i] > 0) dead [i] -= 1;	
		}
	}

	public void MovePiece(MoveClass move){
		int selected = 0;
		for (int i = 0; i <= 27; i++) {
			if (activePiece [i].name == move.PieceName) {
				selected = i;
				break;
			}
		}
		if (!move.hop) {
			activePiece [selected].transform.position = move.MoveCoord;
			EatPiece (activePiece [selected],-1);

		} else {
			activePiece [selected].transform.position = move.MoveCoord;
			EatPiece (activePiece [selected],-1);
			if (move.p_next != null) {
//				Debug.Log (move.p_next.PieceName);
//				print (move.p_next.MoveCoord);
				move = move.p_next;
				MovePiece (move);
			}

		}
		activePlayer = -activePlayer;
			
	}
	// Get list pieces around selected piece
	/*
	tạo thêm 1 bộ vecto cách vị trí cần duyệt 2 ô 
	*/
	public List<Vector2> GetAroundPieces (float pos_x,float pos_y)
	{
		List<Vector2> around = new List<Vector2> ();

		Vector2 E = new Vector2 (pos_x + 0.7f, pos_y);
		Vector2 SE = new Vector2 (pos_x + 0.35f, pos_y - 0.35f);
		Vector2 S = new Vector2 (pos_x, pos_y - 0.7f);
		Vector2 SW = new Vector2 (pos_x - 0.35f, pos_y - 0.35f);
		Vector2 W = new Vector2 (pos_x - 0.7f, pos_y);
		Vector2 NW = new Vector2 (pos_x - 0.35f, pos_y + 0.35f);
		Vector2 N = new Vector2 (pos_x, pos_y + 0.7f);
		Vector2 NE = new Vector2 (pos_x + 0.35f, pos_y + 0.35f);

		Vector2 E2 = new Vector2 (pos_x + 1.4f, pos_y);
		Vector2 SE2 = new Vector2 (pos_x + 0.7f, pos_y - 0.7f);
		Vector2 N2 = new Vector2 (pos_x, pos_y + 1.4f);
		Vector2 S2 = new Vector2 (pos_x, pos_y - 1.4f);
		Vector2 W2 = new Vector2 (pos_x - 1.4f, pos_y);
		Vector2 NE2 = new Vector2 (pos_x + 0.7f, pos_y + 0.7f);
		Vector2 SW2 = new Vector2 (pos_x - 0.7f, pos_y - 0.7f);
		Vector2 NW2 = new Vector2 (pos_x - 0.7f, pos_y + 0.7f);
		
		around.Clear ();
		around.Add (E); //j = 0
		around.Add (SE); //1
		around.Add (S); //2
		around.Add (SW); //3
		around.Add (W); //4
		around.Add (NW);
		around.Add (N);
		around.Add (NE);

		// ô xung quanh thứ 2
		around.Add (E2);
		around.Add (SE2);
		around.Add (S2);
		around.Add (SW2);
		around.Add (W2);
		around.Add (NW2);
		around.Add (N2);
		around.Add (NE2);

		return around;
	}
	// If the movement is legal, eat the piece
	public void EatPiece (GameObject _SelectedPiece, int _playerTag)
	{
		float pos_x = _SelectedPiece.gameObject.transform.position.x;
		float pos_y = _SelectedPiece.gameObject.transform.position.y;
		List<Vector2> around = GetAroundPieces (pos_x , pos_y);

		if (_playerTag == -1) {
			for (int i = 0; i <= 13; i++) {
				for (int j = 0; j <= 7; j++) {
					if ((Mathf.Abs (around [j].x - activePiece [i].gameObject.transform.position.x) <= 0.1) && (Mathf.Abs (around [j].y - activePiece [i].gameObject.transform.position.y) <= 0.1)) {

						if(!_SelectedPiece.name.Contains("Light")){

							int direct = (j + 4) % 8; //Direct for attacked piece

							//Dark p2 Attack
							if ((_SelectedPiece.name == "Darkp2")) {
								if (activePiece [i].name != "Lightp1")
									Killpiece (activePiece [i], _SelectedPiece, 5, j, true,i);
							
							//Wind p2 Attack
							} else if (_SelectedPiece.name.Contains("Windp2")) {
								if (activePiece [i].name.Contains("Waterp1") || (activePiece [i].name == "Darkp1"))
									Killpiece (activePiece [i], _SelectedPiece, 4, j, true,i);
								if (activePiece [i].name.Contains ("Firep1"))
									Killpiece (_SelectedPiece,activePiece [i], 1, direct, false,19+(int.Parse(_SelectedPiece.name [0].ToString())-1)*4);
							
							//Water p2 Attack
							} else if (_SelectedPiece.name.Contains("Waterp2")) {
								if (activePiece [i].name.Contains("Earthp1") || (activePiece [i].name == "Darkp1"))
									Killpiece (activePiece [i],_SelectedPiece, 3, j, true,i);
								if (activePiece [i].name.Contains("Windp1"))
									Killpiece (_SelectedPiece,activePiece [i], 4, direct, false,17+(int.Parse(_SelectedPiece.name [0].ToString())-1)*4);

							//Earth p2 Attack
							} else if (_SelectedPiece.name.Contains("Earthp2")) {
								if (activePiece [i].name.Contains("Firep1") || (activePiece [i].name == "Darkp1"))
									Killpiece (activePiece [i],_SelectedPiece, 2, j, true,i);
								if (activePiece [i].name.Contains("Waterp1"))
									Killpiece (_SelectedPiece,activePiece [i], 3, direct, false,18+(int.Parse(_SelectedPiece.name [0].ToString())-1)*4);

							//Fire p2 Attack
							} else if (_SelectedPiece.name.Contains("Firep2")) {
								if (activePiece [i].name.Contains("Windp1") || (activePiece [i].name == "Darkp1")) {
									Killpiece (activePiece [i],_SelectedPiece, 1, j, true,i);
								}
								if (activePiece [i].name.Contains("Earthp1"))
									Killpiece (_SelectedPiece,activePiece [i], 2, direct, false,16+(int.Parse(_SelectedPiece.name [0].ToString())-1)*4);
							}
						}

					}
				}

			}
		}
		if (_playerTag == 1) {
			for (int i = 14; i <= 27; i++) {
				for (int j = 0; j <= 7; j++) {
					if ((Mathf.Abs (around [j].x - activePiece [i].gameObject.transform.position.x) <= 0.1) && (Mathf.Abs (around [j].y - activePiece [i].gameObject.transform.position.y) <= 0.1)) {
						if (!_SelectedPiece.name.Contains ("Light")) { 
							
							int direct = (j + 4) % 8; //Direct for attacked piece

							//Dark p1 Attack
							if ((_SelectedPiece.name == "Darkp1")) {
								if (activePiece [i].name != "Lightp2")
									Killpiece (activePiece [i],_SelectedPiece, 5, j, true,i);
							
							//Wind p1 Attack
							} else if (_SelectedPiece.name.Contains ("Windp1")) {
								if (activePiece [i].name.Contains ("Waterp2") || (activePiece [i].name == "Darkp2"))
									Killpiece (activePiece [i],_SelectedPiece, 4, j, true,i);
								if (activePiece [i].name.Contains ("Firep2"))
									Killpiece (_SelectedPiece,activePiece [i], 1, direct, false,5+(int.Parse(_SelectedPiece.name [0].ToString())-1)*4);

							//Water p1 Attack
							} else if (_SelectedPiece.name.Contains ("Waterp1")) {
								if (activePiece [i].name.Contains ("Earthp2") || (activePiece [i].name == "Darkp2"))
									Killpiece (activePiece [i],_SelectedPiece, 3, j, true,i);
								if (activePiece [i].name.Contains ("Windp2"))
									Killpiece (_SelectedPiece,activePiece [i], 4, direct, false,3+(int.Parse(_SelectedPiece.name [0].ToString())-1)*4);

							//Earth p1 Attack
							} else if (_SelectedPiece.name.Contains ("Earthp1")) {
								if (activePiece [i].name.Contains ("Firep2") || (activePiece [i].name == "Darkp2"))
									Killpiece (activePiece [i],_SelectedPiece, 2, j, true,i);
								if (activePiece [i].name.Contains ("Waterp2"))
									Killpiece (_SelectedPiece,activePiece [i], 3, direct, false,4+(int.Parse(_SelectedPiece.name [0].ToString())-1)*4);
							
							//Fire p1 Attack
							} else if (_SelectedPiece.name.Contains ("Firep1")) {
								if (activePiece [i].name.Contains ("Windp2") || (activePiece [i].name == "Darkp2"))
									Killpiece (activePiece [i],_SelectedPiece, 1, j, true,i);
								if (activePiece [i].name.Contains ("Earthp2"))
									Killpiece (_SelectedPiece,activePiece [i], 2, direct, false,2+(int.Parse(_SelectedPiece.name [0].ToString())-1)*4);
							}
						}
					}
				}
			}
		}
		StartCoroutine (waitDestroy ());
	}

	private IEnumerator waitDestroy() {
		if (animList.Count () > 2)
			multiAnimPlaying = true;
		for(int i = 0 ; i <= 28 ; i++) {
			if (dead [i] == 3) {
				activePiece [i].transform.position = new Vector2 (100, 100);
				yield return new WaitForSeconds (1.0f);
			}
		}
		yield return null;
	}

	private void Killpiece (GameObject killedPiece, GameObject attackPiece,
		int _eatIndex, int j, bool _isAnimPriorityTag, int pieceIndex)
	{
		dead [pieceIndex] = 3;
		killedVtor =  killedPiece.transform.position;
		attackVtor = attackPiece.transform.position;
		killedVtor.z = -1.0f;
		attackVtor.z = -1.1f;
		cameraLocation = attackVtor;
		if (_eatIndex == 4)
			windVictimCreator (killedPiece);

		//Add animation in animList in order
		AnimationClass listIndex = new AnimationClass ();
		listIndex.setInfo (attackVtor, killedVtor, _isAnimPriorityTag, _eatIndex, j, killedPiece.name);
		animList.Add (listIndex);

		//Destroy by moving piece far away from board
		graveyard.Add (killedPiece);
	}

	private void Killpiece (PieceClass piece, int _eatIndex)
	{
		//eatIndex = _eatIndex;
		//graveyard.Add (piece);
		//activePiece.Remove (piece);
		piece.pos_x = 100f;
		piece.pos_y = 100f;
	}
	// Test if the piece can do the player's movement
	/* 
	*/
	string TestMovement (GameObject _SelectedPiece, Vector2 _coordToMove)
	{
		
		string _movementLegalBool = "0";// 0 is empty, 1 is player1, -1 is player2, 10 is outrange
		
		//_collisionDetectBool = true;
		Vector2 _coordPiece = new Vector2 (_SelectedPiece.transform.position.x, _SelectedPiece.transform.position.y);


		if (_coordToMove.magnitude > 4.3) {
			_movementLegalBool = "10";
		}
		//Debug.Log (gameState);

		for (int a = 0; a <= 27; a++) {
			if ((Mathf.Abs (activePiece [a].gameObject.transform.position.x - _coordToMove.x) <= 0.01) && (Mathf.Abs (activePiece [a].gameObject.transform.position.y - _coordToMove.y) <= 0.01)) {
				_movementLegalBool = activePiece [a].tag;
				break;
			}
		}
		return (_movementLegalBool);

		//return true;
	}
	// AI

	public float Minimax (List<PieceClass> state, int Depth, int turn)
	{
		float best_score = new float();
		if (turn == -1)
			best_score = -999;
		if (turn == 1)
			best_score = 999;
		List<MoveClass> moves = new List<MoveClass> ();
		if ((Depth <= 0) || (this.CheckEndGame () != 0))
			return HeuristicFunc (state, turn);
		else {
			if (turn == -1) {
				moves = UnrealPossibleMove (state, turn);
				foreach (MoveClass move in moves) {
//				if (move.PieceName.Contains ("Light"))
//					Debug.Log (move.MoveCoord);
					List<PieceClass> state2 = UnrealMovePiece (state, move);
					float value = Minimax (state2, Depth - 1, 1);
					if (value > best_score) {
						//print (best_score);
						best_score = value;
						//if( Depth == Max_Depth)
							best_move = move;
					}	
				} 
			} else if (turn == 1) {
				moves = UnrealPossibleMove (state, turn);
				foreach (MoveClass move in moves) {
					List<PieceClass> state2 = UnrealMovePiece (state, move);
					float value = Minimax (state2, Depth - 1, -1);
					if (value < best_score) {
						best_score = value;
						//best_move = move;
					}	
				} 
			}
		}
		return best_score;
	}
	int UnrealTestMovement (List<PieceClass> state, PieceClass _SelectedPiece, Vector2 _coordToMove)
	{

		int _movementLegalBool = 0;// 0 is empty, 1 is player1, -1 is player2, 10 is outrange

		Vector2 _coordPiece = new Vector2 (_SelectedPiece.pos_x, _SelectedPiece.pos_y);
		if (_coordToMove.magnitude > 4.3) {
			_movementLegalBool = 10;
		}
//		PieceClass p = new PieceClass();
//		int [] a = p.GetMatrix (_coordToMove);
//		if ((a [0] <= 0) || (a [0] >= 14) || (a [1] <= 0) || (a [1] >= 14) ) 
//			_movementLegalBool = 10;
//		else {
//			if(ma[a[0],a[1]] < 0)
//				_movementLegalBool = -1;
//			if(ma[a[0],a[1]] > 0)
//				_movementLegalBool = 1;
//		}
		for (int a = 0; a <= 27; a++) {
					
					if ((Mathf.Abs (state [a].pos_x - _coordToMove.x) <= 0.1) && (Mathf.Abs (state [a].pos_y - _coordToMove.y) <= 0.1)) {
						_movementLegalBool = state[a].tag_player;
						break;
					}
				}

		return (_movementLegalBool);

		//return true;
	}
	int UnrealTestMovement (int[,] ma, PieceClass _SelectedPiece, Vector2 _coordToMove)
	{

		int _movementLegalBool = 0;// 0 is empty, 1 is player1, -1 is player2, 10 is outrange
//		int[,] ma = new int[15, 15];
//		ma = SetMatrix(state);
		PieceClass p = new PieceClass();
//		int [] a = p.GetMatrix (_coordToMove);
		//_collisionDetectBool = true;
		Vector2 _coordPiece = new Vector2 (_SelectedPiece.pos_x, _SelectedPiece.pos_y);


//		if ((a [0] < 0) || (a [0] > 14) || (a [1] < 0) || (a [1] > 14) ||((a[0]==0)&&(a[1]==0)||(a[0]==14)&&(a[1]==14))) 
//			_movementLegalBool = 10;
//		else {
//			if(ma[a[0],a[1]] < 0)
//				_movementLegalBool = -1;
//			if(ma[a[0],a[1]] > 0)
//				_movementLegalBool = 1;
//		}

		return (_movementLegalBool);

		//return true;
	}

	private List<MoveClass> UnrealPossibleMove (List<PieceClass> state, int turn)
	{
		
		List<MoveClass> possile_moves = new List<MoveClass> ();
		int[] a = new int[40];
		if (turn == 1)
			a = Enumerable.Range (0, 14).ToArray ();
		else 
			a = Enumerable.Range (14, 27).ToArray ();
		
		for (int i = a [0]; i <= a [13]; i++) {
			
			List<MoveClass> m = PossibleMove (state, state [i]);
			possile_moves.AddRange (m);
		}
		return possile_moves;
	}
	//
	private float HeuristicFunc (List<PieceClass> m,int turn)
	{
		int[] a = new int[14];
		int[] b = new int[14];
		float AttackPoint = 2 ;
		float DefendPoint = 2 ;
		float LightPoint = 10 ;
		float CenterPoint = 1;
		float ReturnPoint = 0;
		if ( CheckEndGame () == -1)
			return 9999;
		if ( CheckEndGame () == 1)
			return -9999;
//		if (turn == 1) {
//			a = Enumerable.Range (0, 14).ToArray ();
//			b = Enumerable.Range (14, 28).ToArray ();
//		} else if (turn == -1) {
//			a = Enumerable.Range (14, 28).ToArray ();
//			b = Enumerable.Range (0, 14).ToArray ();
//		}
		a = Enumerable.Range (14, 28).ToArray ();
		b = Enumerable.Range (0, 14).ToArray ();
		for (int i = a [0]; i <= a [13]; i++) {
			if (m [i].pos_x > 99)
				ReturnPoint = ReturnPoint - DefendPoint;
			else {
				float temp3 = (((4.2f - Mathf.Abs (m [i].pos_y)) / 0.35f) * CenterPoint);
				float temp4 = (((4.2f - Mathf.Abs (m [i].pos_x)) / 0.35f) * CenterPoint);
				ReturnPoint = ReturnPoint + temp3 + temp4;
		
			}
		}

		for (int i = b [0]; i <= b [13]; i++) {
			if (m [i].pos_x > 99)
				ReturnPoint = ReturnPoint + AttackPoint;
			else {
				float temp3 = (((4.2f - Mathf.Abs (m [i].pos_y)) / 0.35f) * CenterPoint);
				float temp4 = (((4.2f - Mathf.Abs (m [i].pos_x)) / 0.35f) * CenterPoint);
				ReturnPoint = ReturnPoint - temp3 - temp4;
			}
		}
		float temp =  (((4.2f - Mathf.Abs (m [a [1]].pos_y))/0.35f)*LightPoint);
		ReturnPoint = ReturnPoint + temp;

		float temp2 = (((4.2f - Mathf.Abs (m [b [1]].pos_y))/0.35f)*LightPoint);
		ReturnPoint = ReturnPoint - temp2;

		return ReturnPoint;
	}
	//
	public List<PieceClass> UnrealMovePiece (List<PieceClass> state, MoveClass move)
	{
		PieceClass selected_p = new PieceClass();
		for(int i = 0 ; i <= 27 ; i++){
			if (state[i].PieceName == move.PieceName){
				selected_p = state [i];
				break;
			}
		}
		// Don't move if the user clicked on its own cube or if there is a piece on the cube
		if (!move.hop) {
				selected_p.pos_x = move.MoveCoord.x; // Move the piece
				selected_p.pos_y = move.MoveCoord.y;
				state = UnrealEatPiece ( state , selected_p);
				//ChangeState (0);

		} else {
			selected_p.pos_x = move.MoveCoord.x; // Move the piece
			selected_p.pos_y = move.MoveCoord.y;
			state = UnrealEatPiece ( state , selected_p);
			if (move.p_next != null) {
				//Debug.Log (move.p_next.PieceName);
				//print (move.p_next.MoveCoord);
				move = move.p_next;
				UnrealMovePiece (state,move);
			}

		}
			
		return state;
	}
	public List<PieceClass> UnrealEatPiece (List<PieceClass> state, PieceClass _SelectedPiece)
	{

		List<Vector2> around = GetAroundPieces (_SelectedPiece.pos_x,_SelectedPiece.pos_y);
		//print (around[1]+"aaa");
		int _eatIndex;
		if (_SelectedPiece.tag_player == -1) {
			for (int i = 0; i <= 13; i++) {
				for (int j = 0; j <= 7; j++) {
					if ((Mathf.Abs (around [j].x - state [i].pos_x) <= 0.1) && (Mathf.Abs (around [j].y - state [i].pos_y) <= 0.1)) {
						if (!_SelectedPiece.PieceName.Contains ("Light")) { 
							if ((_SelectedPiece.PieceName == "Darkp2") || (state [i].PieceName == "Darkp1")) {
								
								if (state [i].PieceName != "Lightp1")
									Killpiece (state [i], 0);
							} else if (_SelectedPiece.PieceName.Contains ("Windp2")) {
								
								if (state [i].PieceName.Contains ("Waterp1"))
									Killpiece (state [i], 0);
								if (state [i].PieceName.Contains ("Firep1")) {
									Killpiece (_SelectedPiece, 0);
								}
							} else if (_SelectedPiece.PieceName.Contains ("Waterp2")) {
								
								if (state [i].PieceName.Contains ("Earthp1"))
									Killpiece (state [i], 0);
								if (state [i].PieceName.Contains ("Windp1"))
									Killpiece (_SelectedPiece, 0);
							} else if (_SelectedPiece.PieceName.Contains ("Earthp2")) {
								
								if (state [i].PieceName.Contains ("Firep1"))
									Killpiece (state [i], 0);
								if (state [i].PieceName.Contains ("Waterp1"))
									Killpiece (_SelectedPiece, 0);
							} else if (_SelectedPiece.PieceName.Contains ("Firep2")) {
								if (state [i].PieceName.Contains ("Windp1"))
									Killpiece (state [i], 0);
								if (state [i].PieceName.Contains ("Earthp1"))
									Killpiece (_SelectedPiece, 0);
							}

						}
					}
				}

			}

		}
		if (_SelectedPiece.tag_player == 1) {
			for (int i = 14; i <= 27; i++) {
				for (int j = 0; j <= 7; j++) {
					if ((Mathf.Abs (around [j].x - state [i].pos_x) <= 0.1) && (Mathf.Abs (around [j].y - state [i].pos_y) <= 0.1)) {
						if (!_SelectedPiece.PieceName.Contains ("Light")) {
							if ((_SelectedPiece.PieceName == "Darkp1") || (state [i].PieceName == "Darkp2")) {
								
								if (state [i].PieceName != "Lightp2")
									Killpiece (state [i], 0);
							} else if (_SelectedPiece.PieceName.Contains ("Windp1")) {
								
								if (state [i].PieceName.Contains ("Waterp2"))
									Killpiece (state [i], 0);
								if (state [i].PieceName.Contains ("Firep2"))
									Killpiece (_SelectedPiece, 0);
							} else if (_SelectedPiece.PieceName.Contains ("Waterp1")) {
								
								if (state [i].PieceName.Contains ("Earthp2"))
									Killpiece (state [i], 0);
								if (state [i].PieceName.Contains ("Windp2"))
									Killpiece (_SelectedPiece, 0);
							} else if (_SelectedPiece.PieceName.Contains ("Earthp1")) {

								if (state [i].PieceName.Contains ("Firep2"))
									Killpiece (state [i], 0);
								if (state [i].PieceName.Contains ("Waterp2"))
									Killpiece (_SelectedPiece, 0);
							} else if (_SelectedPiece.PieceName.Contains ("Firep1")) {

								if (state [i].PieceName.Contains ("Windp2"))
									Killpiece (state [i], 0);
								if (state [i].PieceName.Contains ("Earthp2"))
									Killpiece (_SelectedPiece, 0);
							}
						}
					}
				}
			}
		}
		return state;
	}

	// Change the state of the game
	public void ChangeState (int _newState)
	{
		gameState = _newState;
	}

	public int CheckEndGame ()
	{

		if (Mathf.Abs(activePiece [1].gameObject.transform.position.x) < 0.01f && Mathf.Abs(activePiece [1].gameObject.transform.position.y) < 0.01f) {

			return 1;
		} else if (Mathf.Abs(activePiece [15].gameObject.transform.position.x) < 0.01f && Mathf.Abs(activePiece [15].gameObject.transform.position.y) < 0.01f) {

			return -1;
		}

		return 0;
	}

	public int CheckEndGame (List<PieceClass> state)
	{

		if (Mathf.Abs(state[1].pos_x) < 0.01f && Mathf.Abs(state[1].pos_y) < 0.01f) {

			return 1;
		} else if (Mathf.Abs(state[15].pos_x) < 0.01f && Mathf.Abs(state[15].pos_y) < 0.01f) {

			return -1;
		}

		return 0;
	}

	#region animation
	//Animation movement process
	public void AnimLocate (AnimationClass animIndex) {
		
		GameObject animLocation, subLocation, subLocation2;
		switch (animIndex.eatAnim) {
		case 1: //Fire Animation
			animLocation = GameObject.Find ("fire_atk");
			subLocation = GameObject.Find ("fire_atked");
			animLocation.transform.position = animIndex.atkLocation;
			subLocation.transform.position = animIndex.killLocation;
			redirectVtorAnim (animLocation, subLocation, animIndex.eatAnim, animIndex.angleAnim); //redirect fire
			break;
		case 2: //Earth Animation
			subLocation = GameObject.Find ("earth_atked");
			subLocation.transform.position = animIndex.killLocation;
			;
			break;
		case 3: //Water Animation
			animLocation = GameObject.Find ("water_atk");
			subLocation = GameObject.Find ("water_atked");
			animLocation.transform.position = animIndex.atkLocation;
			subLocation.transform.position = animIndex.killLocation;
			;
			redirectVtorAnim (animLocation, subLocation, animIndex.eatAnim, animIndex.angleAnim); //redirect water
			break;
		case 4: //Wind Animation
			animLocation = GameObject.Find ("wind_atk");
			subLocation = GameObject.Find ("wind_atked");
			animLocation.transform.position = animIndex.atkLocation;
			subLocation.transform.position = animIndex.killLocation;

			windAttack (animIndex.blowAngleRate(), subLocation);
			redirectVtorAnim (animLocation, windTermVector, animIndex.eatAnim, animIndex.angleAnim); //redirect wind
			break;
		case 5: //Dark Animation
			animLocation = GameObject.Find ("dark_atk");
			subLocation = GameObject.Find ("dark_atked_1");
			subLocation2 = GameObject.Find ("dark_atked_2");
			animLocation.transform.position = animIndex.atkLocation;
			subLocation.transform.position = animIndex.killLocation;
			subLocation2.transform.position = animIndex.killLocation;
			redirectVtorAnim (animLocation, subLocation, animIndex.eatAnim, animIndex.angleAnim);
			break;
		case 6: //Light Animation
			animLocation = GameObject.Find ("light_atk");
			animLocation.transform.position = animIndex.atkLocation;

			//All enemy's pieces vanished
			int range = 0;
			if (isEnd == 1)
				range = 14;
			for (int i = 0; i < 14; i++) {
				string nameLightanim = "light_atked (" + i + ")";
				subLocationN [i] = GameObject.Find (nameLightanim);
				subLocationN [i].transform.position = new Vector3 (
					activePiece [i + range].transform.position.x,
					activePiece [i + range].transform.position.y,
					-0.5f);
				StartCoroutine(eventProcess(activePiece [i + range]));
			}
			break;
		}
	}

	//Animation process in Unity
	public IEnumerator AnimProcess(AnimationClass animIndex) {
		if (multiAnimPlaying) {
			if (isEnd == 0) {
				//Set eatIndex in Unity
				animator.SetInteger ("eatIndex", animIndex.eatAnim);
				if (animIndex.eatAnim != 0)
					isAttacking = true;
				yield return new WaitForSeconds (duration);
				isAttacking = false;
				Destroy (GameObject.Find ("wind_atked"));
				animIndex.eatAnim = 0;
				animator.SetInteger ("eatIndex", animIndex.eatAnim);
			} else {
				isAttacking = false;
				animator.SetInteger ("eatIndex", animIndex.eatAnim);
				yield return new WaitForSeconds (10.0f);
			}
		} else
			multiAnimPlaying = false;
//		if (isEnd == 0 && !multiAnimPlaying) {
//			//Set eatIndex in Unity
//			animator.SetInteger ("eatIndex", animIndex.eatAnim);
//			multiAnimPlaying = true;
//			if (animIndex.eatAnim != 0)
//				isAttacking = true;
//			//Set after animation finished playing
//			yield return new WaitForSeconds (duration);
//			isAttacking = false;
//			Destroy(GameObject.Find("wind_atked"));
//			animIndex.eatAnim = 0;
//			animator.SetInteger ("eatIndex", animIndex.eatAnim);
//			multiAnimPlaying = false;
//		}
//
//		else if (isEnd != 0) {
//			isAttacking = false;
//			animator.SetInteger ("eatIndex", animIndex.eatAnim);
//			yield return new WaitForSeconds (10.0f);
//		}
	}

	void multiAnimation(AnimationClass animIndex) {
		
	}

	//Get midpoint from 2 objects
	public Vector3 getMidPts(Vector3 a, Vector3 b, int index) {
		float rate = 1;
		if (index == 1 || index == 4)
			rate = 1.5f;
		Vector3 c = new Vector3 ();
		c.x = ((2.0f - rate) * a.x + rate * b.x) / 2;
		c.y = ((2.0f - rate) * a.y + rate * b.y) / 2;
		c.z = a.z;
		return c;
	}

	//Calculate rotating angle of animation
	public float getAngle(Vector3 x, bool isWind) {
		float angle = Vector3.Angle (x, Vector3.right);
		if (isWind)
			angle += 120.0f;
		return angle;
	}

	//Get the 3rd point in triangle with attack point and killed point
	public void fixWaterAnim(GameObject a, GameObject b, int angleIndex) {
		float eX, eY, eZ;
		eX = 0.35f;
		eY = 0.35f * Mathf.Sqrt (2.0f) / 2 + 0.35f * 2 - 0.1f;
		eZ = 0.5f;

		//Rotate
		a.transform.rotation = Quaternion.Euler(0, 0, 45 * -angleIndex);

		//Transform + Scale
		switch (angleIndex) {
		case 0:
			a.transform.position = new Vector3 (
				a.transform.position.x + eX,
				a.transform.position.y + eY,
				eZ);
			break;
		case 1:
			a.transform.position = new Vector3 (
				a.transform.position.x + 2 * eX,
				a.transform.position.y + eX,
				eZ);
			break;
		case 2:
			a.transform.position = new Vector3 (
				a.transform.position.x + eY,
				a.transform.position.y - eX,
				eZ);
			break;
		case 3:
			a.transform.localScale = new Vector3 (0.9f, -0.9f, 1.0f);
			a.transform.position = new Vector3 (
				a.transform.position.x - 2 * eX,
				a.transform.position.y + eX,
				eZ);
			break;
		case 4:
			a.transform.localScale = new Vector3 (0.9f, -0.9f, 1.0f);
			a.transform.position = new Vector3 (
				a.transform.position.x - eX,
				a.transform.position.y + eY,
				eZ);
			break;
		case 5:
			a.transform.localScale = new Vector3 (0.9f, -0.9f, 1.0f);
			a.transform.position = new Vector3 (
				a.transform.position.x + eX,
				a.transform.position.y + 2 * eX,
				eZ);
			break;
		case 6:
			a.transform.position = new Vector3 (
				a.transform.position.x - eY,
				a.transform.position.y + eX,
				eZ);
			break;
		case 7:
			a.transform.position = new Vector3 (
				a.transform.position.x - eX,
				a.transform.position.y + 2 * eX,
				eZ);
			break;
		}
	}

	//Rotate animation
	public void redirectVtorAnim(GameObject rect, GameObject todirect, int _eatIndex, int _angleIndex) {
		switch (_eatIndex) {
		case 1:
			rect.transform.position = getMidPts (rect.transform.position, todirect.transform.position, _eatIndex);
			rect.transform.rotation = Quaternion.Euler (0, 0, 45 * -_angleIndex);
			break;
		case 3: 
			fixWaterAnim (rect, todirect, _angleIndex);
			break;
		case 4:
			rect.transform.position = getMidPts (rect.transform.position, todirect.transform.position, _eatIndex);
			rect.transform.rotation = Quaternion.Euler (0, 0, 45 * -_angleIndex + 150);
			break;
		case 5:
			//Relocate dark's eyes
			rect.transform.position = new Vector3 (rect.transform.position.x,
				rect.transform.position.y - 0.05f, rect.transform.position.z);

			//Relocate thunder
			todirect.transform.position = new Vector3 (todirect.transform.position.x + 0.1f,
				todirect.transform.position.y + 1.5f,
				todirect.transform.position.z);
			break;
		}
	}

	//Poly function
	public void redirectVtorAnim(GameObject rect, Vector3 todirect, int _eatIndex, int _angleIndex) {
		switch (_eatIndex) {
		case 4:
			rect.transform.position = getMidPts (rect.transform.position, todirect, _eatIndex);
			rect.transform.rotation = Quaternion.Euler (0, 0, 45 * -_angleIndex + 150);
			break;
		}
	}

	//Wind animation calculation
	void windAttack(Vector2 blowRate, GameObject windVictim) {
		if (windVictim.transform.position.magnitude < 50.0f) {
			moveWind.x += blowRate.x * windSpeed;
			moveWind.y += blowRate.y * windSpeed;
			windVictim.transform.position = new Vector3 (moveWind.x, moveWind.y, moveWind.z);
		} else
			moveWind = new Vector3 (0.0f, 0.0f, 0.0f);
	}

	//Animation events
	public IEnumerator eventProcess(GameObject a) {
		yield return new WaitForSeconds (1.0f);
		a.SetActive (false);
	}

	//Create wind clone
	void windVictimCreator(GameObject victimName) {
		GameObject victim = Object.Instantiate (victimName, victimName.transform.position, Quaternion.identity) as GameObject;
		if (!isAttacking) windTermVector = victimName.transform.position;
		victim.name = "wind_atked";
		moveWind = victim.transform.position;
	}



	#endregion

	#region Camera controller
	IEnumerator cameraController() {
		if (isEnd == 0) {
			PlayerCam.transform.position = new Vector3 (
				cameraLocation.x, cameraLocation.y, -10.0f);
			PlayerCam.orthographicSize = cameraSize;
			yield return new WaitForSeconds (3.0f);
			PlayerCam.transform.position = new Vector3 (0.0f, 0.0f, -10.0f);
			PlayerCam.orthographicSize = 5;
		}
	}

	#endregion

	public void EndGame ()
	{
		Application.Quit ();
	}
	
}