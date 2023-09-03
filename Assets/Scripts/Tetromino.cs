using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
	I,
	O,
	T,
	J,
	L,
	S,
	Z,
}


public static class Speed
{
	public const float Lv0 = 0.8f;
	public const float Lv1 = 0.716f;
	public const float Lv2 = 0.633f;
	public const float Lv3 = 0.55f;
	public const float Lv4 = 0.466f;
	public const float Lv5 = 0.383f;
	public const float Lv6 = 0.3f;
	public const float Lv7 = 0.216f;
	public const float Lv8 = 0.133f;
	public const float Lv9 = 0.1f;
	public const float Lv10to12 = 0.083f;
	public const float Lv13to15 = 0.066f;
	public const float Lv16to18 = 0.05f;
	public const float Lv19to28 = 0.033f;
	public const float Lv29 = 0.016f;
}

[System.Serializable]
public struct TetrominoData
{
	private Board board;
	public Tetromino tetromino;
	[HideInInspector]public Tile tile;
	public Vector2Int[] cells { get; private set; }
	public Vector2Int[,] wallKicks { get; private set; }

	public void Initialize()
	{
		cells = Data.Cells[tetromino];
		wallKicks = Data.WallKicks[tetromino];
	}
	[SerializeField]
	public Tile Lv0;
	public Tile Lv1;
	public Tile Lv2;
	public Tile Lv3;
	public Tile Lv4;
	public Tile Lv5;
	public Tile Lv6;
	public Tile Lv7;
	public Tile Lv8;
	public Tile Lv9;
}