using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClass  {
	public string PieceName;
	public Vector2 MoveCoord;
	public bool hop;
	public MoveClass p_next;

	public MoveClass SetMove(GameObject selected_piece, GameObject clone){
		MoveClass move = new MoveClass ();
		move.PieceName = selected_piece.name;
		move.MoveCoord = new Vector2(clone.transform.position.x,clone.transform.position.y);
		if (clone.name == "cloneH")
			hop = true;
		else
			hop = false;
		return move;
	}

}
