using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Collections;

public class Board : MonoBehaviour
{
	public Material[] skyboxes;
	public AudioManager audioPlayer { get; private set; }
	public Tilemap tilemap { get; private set; }
	public TilemapRenderer tilemapRenderer { get; set; }
	public Piece activePiece { get; private set; }
	public NextPiece nextPiece { get; private set; }
	public Text[] text { get; set; }
	public Text stats { get; set; }
	public Text pausedText { get; set; }
	public Settings settings;
	public TetrominoData[] tetrominoes;
	public int nextPieceInt = -1;
	public int history = -1;
	public Vector3Int spawnPosition;
	public Vector2Int boardSize = new Vector2Int(10, 22);
	public int score = 0;
	public int highscore = 0;
	public int lines = 0;
	public int level = 0;
	public int prevLevel = 0;
	public int tetrisLines = 0;
	public float tetrisRate = 0f;
	public float speed = 0.8f;
	private float wait;
	public bool lineClearWait = false;
	public bool lockWait = false;
	public float lineClearWaitTime = 0.5f;
	public float lockWaitTime = 0.2f;
	public bool gameOver = false;
	public int pointsScored = 0;
	public int linesCleared = 0;
	public int droughtCounter = 0;
	public int maxDrought = 0;
	public Stopwatch stopwatch;
	public System.TimeSpan ts;
	public bool leveledUpOnce = false;
	public int transitionLines = 0;
	public int transitionLevel = 0;
	public int tetrises = 0;

	[SerializeField]
	public TileBase Lv0A;
	public TileBase Lv0B;
	public TileBase Lv0C;
	public TileBase Lv1A;
	public TileBase Lv1B;
	public TileBase Lv1C;
	public TileBase Lv2A;
	public TileBase Lv2B;
	public TileBase Lv2C;
	public TileBase Lv3A;
	public TileBase Lv3B;
	public TileBase Lv3C;
	public TileBase Lv4A;
	public TileBase Lv4B;
	public TileBase Lv4C;
	public TileBase Lv5A;
	public TileBase Lv5B;
	public TileBase Lv5C;
	public TileBase Lv6A;
	public TileBase Lv6B;
	public TileBase Lv6C;
	public TileBase Lv7A;
	public TileBase Lv7B;
	public TileBase Lv7C;
	public TileBase Lv8A;
	public TileBase Lv8B;
	public TileBase Lv8C;
	public TileBase Lv9A;
	public TileBase Lv9B;
	public TileBase Lv9C;

	public RectInt Bounds
	{
		get
		{
			Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
			return new RectInt(position, boardSize);
		}
	}

	private void Awake()
	{
		int randomSkybox = Random.Range(0, skyboxes.Length);
		RenderSettings.skybox = skyboxes[randomSkybox];
		stopwatch = new Stopwatch();
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
		if (PlayerPrefs.HasKey("HighScore"))
		{
			highscore = PlayerPrefs.GetInt("HighScore", highscore);
		}
		settings.startLevel = 0;
		if (File.Exists("settings.txt"))
		{
			ReadSettingsFile("settings.txt");
		}
		level = settings.startLevel;
		prevLevel = settings.startLevel;
		tilemap = GetComponentInChildren<Tilemap>();
		activePiece = GetComponentInChildren<Piece>();
		nextPiece = GetComponentInChildren<NextPiece>();
		text = GetComponentsInChildren<Text>();
		audioPlayer = FindObjectOfType<AudioManager>();
		tilemapRenderer = FindObjectOfType<TilemapRenderer>();
		stats = text[0];
		pausedText = text[1];

		for (int i = 0; i < tetrominoes.Length; i++)
		{
			tetrominoes[i].Initialize();
		}
	}

	private void Start()
	{
		stopwatch.Start();
		gameOver = false;
		ChangeColours();
		SpawnPiece();
		SpawnNextPiece();
	}
	public void ReadSettingsFile(string dir)
	{
		StreamReader reader = new StreamReader(dir);
		string line;
		while((line = reader.ReadLine()) != null)
		{
			switch (line)
			{
				case "start-speed=0":
					settings.startLevel = 0;
					break;
				case "start-speed=1":
					settings.startLevel = 1;
					break;
				case "start-speed=2":
					settings.startLevel = 2;
					break;
				case "start-speed=3":
					settings.startLevel = 3;
					break;
				case "start-speed=4":
					settings.startLevel = 4;
					break;
				case "start-speed=5":
					settings.startLevel = 5;
					break;
				case "start-speed=6":
					settings.startLevel = 6;
					break;
				case "start-speed=7":
					settings.startLevel = 7;
					break;
				case "start-speed=8":
					settings.startLevel = 8;
					break;
				case "start-speed=9":
					settings.startLevel = 9;
					break;
				case "start-speed=10":
					settings.startLevel = 10;
					break;
				case "start-speed=11":
					settings.startLevel = 11;
					break;
				case "start-speed=12":
					settings.startLevel = 12;
					break;
				case "start-speed=13":
					settings.startLevel = 13;
					break;
				case "start-speed=14":
					settings.startLevel = 14;
					break;
				case "start-speed=15":
					settings.startLevel = 15;
					break;
				case "start-speed=16":
					settings.startLevel = 16;
					break;
				case "start-speed=17":
					settings.startLevel = 17;
					break;
				case "start-speed=18":
					settings.startLevel = 18;
					break;
				case "start-speed=19":
					settings.startLevel = 19;
					break;
				case "start-speed=20":
					settings.startLevel = 20;
					break;
				case "start-speed=21":
					settings.startLevel = 21;
					break;
				case "start-speed=22":
					settings.startLevel = 22;
					break;
				case "start-speed=23":
					settings.startLevel = 23;
					break;
				case "start-speed=24":
					settings.startLevel = 24;
					break;
				case "start-speed=25":
					settings.startLevel = 25;
					break;
				case "start-speed=26":
					settings.startLevel = 26;
					break;
				case "start-speed=27":
					settings.startLevel = 27;
					break;
				case "start-speed=28":
					settings.startLevel = 28;
					break;
				case "start-speed=29":
					settings.startLevel = 29;
					break;
				case @"start-speed=[1-9]\d{2,}|29|[3-9][0-9]":
					settings.startLevel = 29;
					break;
				default:
					settings.startLevel = 0;
					break;
			}
		}
	}

	public void SpawnPiece()
	{
		TetrominoData data;
		if (nextPieceInt < 0)
		{
			int random = Random.Range(0, tetrominoes.Length);
			data = tetrominoes[random];
		}
		else
		{
			data = tetrominoes[nextPieceInt];
		}

		activePiece.Initialize(this, spawnPosition, data);

		if (IsValidPosition(activePiece, spawnPosition))
		{
			activePiece.spawnedPiece = true;
			Set(activePiece);
		}
		else
		{
			stopwatch.Stop();
			gameOver = true;
			audioPlayer.Play("TopOut");
			if (score > highscore)
			{
				highscore = score;
			}
			PlayerPrefs.SetInt("HighScore", highscore);
			PlayerPrefs.Save();

			if (gameOver)
			{
				return;
			}
		}
	}

	public void SpawnNextPiece()
	{
		nextPieceInt = Random.Range(0, tetrominoes.Length);
		if (nextPieceInt == history)
		{
			nextPieceInt = Random.Range(0, tetrominoes.Length);
		}
		history = nextPieceInt;
		if (nextPieceInt != 0)
		{
			droughtCounter++;
		}
		else
		{
			droughtCounter = 0;
		}
		if (droughtCounter > maxDrought)
		{
			maxDrought = droughtCounter;
		}
		TetrominoData data = tetrominoes[nextPieceInt];
		nextPiece.Initialize(this, new Vector3Int(8, 4, 0), data);
	}

	public void ResetBoard()
	{
		tilemap.ClearAllTiles();
		int randomSkybox = Random.Range(0, skyboxes.Length);
		RenderSettings.skybox = skyboxes[randomSkybox];
		score = 0;
		lines = 0;
		tetrisLines = 0;
		tetrisRate = 0f;
		droughtCounter = 0;
		maxDrought = 0;
		stopwatch.Restart();
		level = settings.startLevel;
		ChangeColours();
		leveledUpOnce = false;
		transitionLevel = 0;
		transitionLines = 0;
		tetrises = 0;
	}

	public void Set(Piece piece)
	{
		for (int i = 0; i < piece.cells.Length; i++)
		{
			Vector3Int tilePosition = piece.cells[i] + piece.position;
			tilemap.SetTile(tilePosition, piece.data.tile);
		}
	}

	public void SetNext(NextPiece piece)
	{
		for (int i = 0; i < piece.cells.Length; i++)
		{
			Vector3Int tilePosition = piece.cells[i] + piece.position;
			tilemap.SetTile(tilePosition, piece.data.tile);
		}
	}

	public void Clear(Piece piece)
	{
		for (int i = 0; i < piece.cells.Length; i++)
		{
			Vector3Int tilePosition = piece.cells[i] + piece.position;
			tilemap.SetTile(tilePosition, null);
		}
	}
	public void ClearNext(NextPiece piece, Board board)
	{
		for (int i = 0; i < piece.cells.Length; i++)
		{
			Vector3Int tilePosition = piece.cells[i] + piece.position;
			tilemap.SetTile(tilePosition, null);
		}
		tilemap.SetTile(new Vector3Int(6, 4, 0), null);
		tilemap.SetTile(new Vector3Int(7, 4, 0), null);
		tilemap.SetTile(new Vector3Int(8, 4, 0), null);
		tilemap.SetTile(new Vector3Int(9, 4, 0), null);
		tilemap.SetTile(new Vector3Int(6, 3, 0), null);
		tilemap.SetTile(new Vector3Int(7, 3, 0), null);
		tilemap.SetTile(new Vector3Int(8, 3, 0), null);
		tilemap.SetTile(new Vector3Int(9, 3, 0), null);
		tilemap.SetTile(new Vector3Int(6, 3, 0), null);
		tilemap.SetTile(new Vector3Int(7, 3, 0), null);
		tilemap.SetTile(new Vector3Int(8, 3, 0), null);
		tilemap.SetTile(new Vector3Int(9, 3, 0), null);
	}

	public bool IsValidPosition(Piece piece, Vector3Int position)
	{
		RectInt bounds = Bounds;

		for(int i = 0; i < piece.cells.Length; i++)
		{
			Vector3Int tilePosition = piece.cells[i] + position;

			if (!bounds.Contains((Vector2Int)tilePosition))
			{
				return false;
			}

			if (tilemap.HasTile(tilePosition))
			{
				return false;
			}
		}
		return true;
	}

	async public void ClearLines()
	{
		RectInt bounds = Bounds;
		int row = bounds.yMin;

		linesCleared = 0;

		while (row < bounds.yMax)
		{
			if (IsLineFull(row))
			{
				LineClear(row);
				linesCleared++;
			}
			else
			{
				row++;
			}
		}
		switch (linesCleared)
		{
			case 0:
				audioPlayer.Play("Lock");
				break;
			case 1:
				audioPlayer.Play("LineClear");
				pointsScored = 40 * (level + 1);
				break;
			case 2:
				audioPlayer.Play("LineClear");
				pointsScored = 100 * (level + 1);
				break;
			case 3:
				audioPlayer.Play("LineClear");
				pointsScored = 300 * (level + 1);
				break;
			case 4:
				audioPlayer.Play("Tetris");
				pointsScored = 1200 * (level + 1);
				tetrisLines += 4;
				tetrises += 1;
				break;
			default:
				break;
		}
		if (linesCleared > 0)
		{
			lineClearWait = true;
		}
		else
		{
			lockWait = true;
		}
		if (lockWait)
		{
			await Task.Delay(132);
			lockWait = false;
		}
		if (lineClearWait)
		{
			await Task.Delay(500);
			lineClearWait = false;
		}

		score += pointsScored;

		lines += linesCleared;
		if (leveledUpOnce)
		{
			transitionLines += linesCleared;
		}
		if (lines > 0)
		{
			tetrisRate = Mathf.Round(((float)tetrisLines / (float)lines) * 100f);
		}
		int lastlevel = level;
		prevLevel = level;
		if (int.Parse((lines / 10).ToString(), System.Globalization.NumberStyles.HexNumber) > level && !leveledUpOnce)
		{
			level += 1;
			leveledUpOnce = true;
		}
		else if ((transitionLines / 10) > transitionLevel)
		{
			level += 1;
			transitionLevel += 1;
		}

		if (level > lastlevel)
		{
			audioPlayer.Play("LevelUp");
		}
		if (level != lastlevel)
		{
			int randomSkybox = Random.Range(0, skyboxes.Length);
			RenderSettings.skybox = skyboxes[randomSkybox];
			ChangeColours();
		}

		//UnityEngine.Debug.Log("Lines Cleared: " + linesCleared);
		//UnityEngine.Debug.Log("Points Scored: " + pointsScored);
		//UnityEngine.Debug.Log("---------------------------------");
		//UnityEngine.Debug.Log("Lines: " + lines);
		//UnityEngine.Debug.Log("Level: " + level);
		//UnityEngine.Debug.Log("Score: " + score);
	}

	private void ChangeColours()
	{
		int levelColour = level % 10;
		int prevLevelColour = prevLevel % 10;

		TileBase currentTileA = Lv0A;
		TileBase currentTileB = Lv0B;
		TileBase currentTileC = Lv0C;
		switch (prevLevelColour)
		{
			case 0:
				currentTileA = Lv0A;
				currentTileB = Lv0B;
				currentTileC = Lv0C;
				break;
			case 1:
				currentTileA = Lv1A;
				currentTileB = Lv1B;
				currentTileC = Lv1C;
				break;
			case 2:
				currentTileA = Lv2A;
				currentTileB = Lv2B;
				currentTileC = Lv2C;
				break;
			case 3:
				currentTileA = Lv3A;
				currentTileB = Lv3B;
				currentTileC = Lv3C;
				break;
			case 4:
				currentTileA = Lv4A;
				currentTileB = Lv4B;
				currentTileC = Lv4C;
				break;
			case 5:
				currentTileA = Lv5A;
				currentTileB = Lv5B;
				currentTileC = Lv5C;
				break;
			case 6:
				currentTileA = Lv6A;
				currentTileB = Lv6B;
				currentTileC = Lv6C;
				break;
			case 7:
				currentTileA = Lv7A;
				currentTileB = Lv7B;
				currentTileC = Lv7C;
				break;
			case 8:
				currentTileA = Lv8A;
				currentTileB = Lv8B;
				currentTileC = Lv8C;
				break;
			case 9:
				currentTileA = Lv9A;
				currentTileB = Lv9B;
				currentTileC = Lv9C;
				break;
		}
		switch (levelColour)
		{

			case 0:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv0;
				}
				if (currentTileA != Lv0A)
				{
					tilemap.SwapTile(currentTileA, Lv0A);
					tilemap.SwapTile(currentTileB, Lv0B);
					tilemap.SwapTile(currentTileC, Lv0C);
				}
				break;
			case 1:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv1;
				}
				if (currentTileA != Lv1A)
				{
					tilemap.SwapTile(currentTileA, Lv1A);
					tilemap.SwapTile(currentTileB, Lv1B);
					tilemap.SwapTile(currentTileC, Lv1C);
				}
				break;
			case 2:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv2;
				}
				if (currentTileA != Lv2A)
				{
					tilemap.SwapTile(currentTileA, Lv2A);
					tilemap.SwapTile(currentTileB, Lv2B);
					tilemap.SwapTile(currentTileC, Lv2C);
				}
				break;
			case 3:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv3;
				}
				if (currentTileA != Lv3A)
				{
					tilemap.SwapTile(currentTileA, Lv3A);
					tilemap.SwapTile(currentTileB, Lv3B);
					tilemap.SwapTile(currentTileC, Lv3C);
				}
				break;
			case 4:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv4;
				}
				if (currentTileA != Lv4A)
				{
					tilemap.SwapTile(currentTileA, Lv4A);
					tilemap.SwapTile(currentTileB, Lv4B);
					tilemap.SwapTile(currentTileC, Lv4C);
				}
				break;
			case 5:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv5;
				}
				if (currentTileA != Lv5A)
				{
					tilemap.SwapTile(currentTileA, Lv5A);
					tilemap.SwapTile(currentTileB, Lv5B);
					tilemap.SwapTile(currentTileC, Lv5C);
				}
				break;
			case 6:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv6;
				}
				if (currentTileA != Lv6A)
				{
					tilemap.SwapTile(currentTileA, Lv6A);
					tilemap.SwapTile(currentTileB, Lv6B);
					tilemap.SwapTile(currentTileC, Lv6C);
				}
				break;
			case 7:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv7;
				}
				if (currentTileA != Lv7A)
				{
					tilemap.SwapTile(currentTileA, Lv7A);
					tilemap.SwapTile(currentTileB, Lv7B);
					tilemap.SwapTile(currentTileC, Lv7C);
				}
				break;
			case 8:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv8;
				}
				if (currentTileA != Lv8A)
				{
					tilemap.SwapTile(currentTileA, Lv8A);
					tilemap.SwapTile(currentTileB, Lv8B);
					tilemap.SwapTile(currentTileC, Lv8C);
				}
				break;
			case 9:
				for (int i = 0; i < tetrominoes.Length; i++)
				{
					tetrominoes[i].tile = tetrominoes[i].Lv9;
				}
				if (currentTileA != Lv9A)
				{
					tilemap.SwapTile(currentTileA, Lv9A);
					tilemap.SwapTile(currentTileB, Lv9B);
					tilemap.SwapTile(currentTileC, Lv9C);
				}
				break;
		}
	}

	private bool IsLineFull(int row)
	{
		RectInt bounds = Bounds;

		for(int col = bounds.xMin; col< bounds.xMax; col++)
		{
			Vector3Int position = new Vector3Int(col, row, 0);

			if (!tilemap.HasTile(position))
			{
				return false;
			}
		}
		return true;
	}
	private void LineClear(int row)
	{

		RectInt bounds = Bounds;

		for (int col = bounds.xMin; col < bounds.xMax; col++)
		{
			Vector3Int position = new Vector3Int(col, row, 0);

			tilemap.SetTile(position, null);
		}
		while (row < bounds.yMax)
		{
			for (int col = bounds.xMin; col < bounds.xMax; col++)
			{
				Vector3Int position = new Vector3Int(col, row + 1, 0);
				TileBase above = tilemap.GetTile(position);

				position = new Vector3Int(col, row, 0);
				tilemap.SetTile(position, above);
			}

			row++;
		}
	}
}