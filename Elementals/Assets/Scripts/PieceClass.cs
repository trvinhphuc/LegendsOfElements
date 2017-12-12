using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceClass {

	public string PieceName;
	public float pos_x;
	public float pos_y;
	public int tag_player;
	public int dead;

	public PieceClass SetPiece(GameObject piece){
		PieceClass state = new PieceClass();
		state.PieceName = piece.name;
		state.pos_x = piece.transform.position.x;
		state.pos_y = piece.transform.position.y;
		state.tag_player = int.Parse(piece.tag);
		state.dead = 0;
		return state;
	}
}
