using UnityEngine;

public class NextPiece : MonoBehaviour
{
	public Board board { get; private set; }
	public TetrominoData data { get; private set; }
	public Vector3Int[] cells { get; private set; }
	public Vector3Int position { get; private set; }
	public void Initialize(Board board, Vector3Int position, TetrominoData data)
	{
		this.board = board;
		this.position = position;
		this.data = data;


		if (cells == null)
		{
			cells = new Vector3Int[data.cells.Length];
		}

		for (int i = 0; i < data.cells.Length; i++)
		{
			cells[i] = (Vector3Int)data.cells[i];
		}
		this.board.ClearNext(this, this.board);
		this.board.SetNext(this);
	}
}
