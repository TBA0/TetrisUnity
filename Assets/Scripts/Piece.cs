using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;
using System.Collections;
using System.Runtime.Remoting.Contexts;

public class Piece : MonoBehaviour
{
	public Board board { get; private set; }
	public TetrominoData data { get; private set; }
	public Vector3Int[] cells { get; private set; }
	public Vector3Int position { get; private set; }
	public int rotationIndex { get; private set; }

	public float lockDelay = 0f;
	private float prevFallDelay;

	private bool pushedDown = false;
	private bool pushingDown = false;

	public float time = 0f;
	public float fallTime = Speed.Lv0;
	private float lockTime;

	private float downTime, upTime, pressTime = 0;
	public int pushDownRate = 40;
	public bool readyLeft = false;
	public bool readyRight = false;

	public float dasRate = 0.08f;
	public float dasDelay = 0.25f;
	private bool shift = false;

	public bool holdingDas = false;

	public float HAxis = 0f;
	public float VAxis = 0f;

	public float boardSpeed = 0f;

	public bool spawnedPiece = true;

	public bool paused = false;

	public string trtColor = "red";
	public string droughtColor = "white";

	public bool resetButtonsNotPressed = true;

	private void Awake()
	{

	}

	public void Initialize(Board board, Vector3Int position, TetrominoData data)
	{
		this.board = board;
		this.position = position;
		this.data = data;
		this.rotationIndex = 0;
		this.fallTime = Time.time + board.speed;
		this.lockTime = 0f;
		this.spawnedPiece = true;



		if (this.cells == null)
		{
			this.cells = new Vector3Int[data.cells.Length];
		}

		for (int i = 0; i < data.cells.Length; i++)
		{
			this.cells[i] = (Vector3Int)data.cells[i];
		}
		Input input = new Input();
		input.Player.Enable();
		//input.Player.Start.performed += StartButton;
		//input.Player.RotateCCW.performed += RotateCCW;
		//input.Player.RotateCW.performed += RotateCW;
		//input.Player.MoveLeft.performed += MoveLeft;
		//input.Player.MoveRight.performed += MoveRight;
		//input.Player.SoftDrop.performed += SoftDrop;
	}

	/*private void StartButton(InputAction.CallbackContext context)
	{
		Debug.Log(context);
		if (!paused && !board.gameOver)
		{
			if (context.performed)
			{
				board.stopwatch.Stop();
				FindObjectOfType<AudioManager>().Play("Pause");
				board.pausedText.text = "PAUSED";
				board.tilemapRenderer.sortingOrder = -1;
				paused = true;
				return;
			}
		}       
		if (paused && !board.gameOver)
		{
			if (context.performed)
			{
				board.stopwatch.Start();
				board.pausedText.text = "";
				board.tilemapRenderer.sortingOrder = 2;
				paused = false;
			}
			return;
		}
		if (board.gameOver)
		{
			if (context.performed)
			{
				paused = false;
				board.pausedText.text = "";
				board.tilemapRenderer.sortingOrder = 2;
				board.gameOver = false;
				board.ResetBoard();
			}
			return;
		}
	}

	private void RotateCCW(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Rotate(-1);
		}
	}

	private void RotateCW(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Rotate(1);
		}
	}

	private void MoveLeft(InputAction.CallbackContext context)
	{
		if (context.performed && !ready)
		{
			Move(Vector2Int.left);
			downTime = Time.time;
			pressTime = downTime + dasDelay;
			ready = true;
		}
		if (!context.performed)
		{
			ready = false;
			holdingDas = false;
		}
		if (Time.time >= pressTime && ready && context.performed)
		{
			holdingDas = true;
			if (!shift)
			{
				downTime = Time.time;
				pressTime = downTime + dasRate;
				shift = true;
			}
			if (Time.time >= pressTime && shift)
			{
				Move(Vector2Int.left);
				shift = false;
			}
		}
	}

	private void MoveRight(InputAction.CallbackContext context)
	{
		if (context.performed && !ready)
		{
			Debug.Log("moved right");
			Move(Vector2Int.right);
			downTime = Time.time;
			pressTime = downTime + dasDelay;
			ready = true;
		}
		if (!context.performed)
		{
			ready = false;
			holdingDas = false;
		}
		if (Time.time >= pressTime && ready && Time.time >= pressTime && ready && context.performed)
		{
			holdingDas = true;
			if (!shift)
			{
				downTime = Time.time;
				pressTime = downTime + dasRate;
				shift = true;
			}
			if (Time.time >= pressTime && shift)
			{
				Move(Vector2Int.right);
				shift = false;
			}
		}
	}

	private void SoftDrop(InputAction.CallbackContext context)
	{
		if (context.performed && !pushingDown)
		{
			pushedDown = true;
			fallTime = Time.time;
		}
		if (context.performed && pushedDown)
		{
			if (!pushingDown)
			{
				prevFallDelay = board.speed;
				pushingDown = true;
			}
			if (board.level < 19)
			{
				board.speed = Speed.Lv19to28;
			}
		}
		else if (!context.performed && (pushedDown || pushingDown))
		{
			board.speed = prevFallDelay;
			pushingDown = false;
		}
	}*/

	private void ReadAction(InputAction.CallbackContext context)
	{
		// disabled until i make an Action for resetting the board
		/*if ((Input.GetButton("L3") && Input.GetButton("R3") && Input.GetButton("L1") && Input.GetButton("R1") && resetButtonsNotPressed) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R)))
		{
			paused = false;
			board.pausedText.text = "";
			board.tilemapRenderer.sortingOrder = 2;
			board.gameOver = false;
			board.ResetBoard();
			board.SpawnPiece();
			board.SpawnNextPiece();
			resetButtonsNotPressed = false;
		}
		else if (!(Input.GetButton("L3") && Input.GetButton("R3") && Input.GetButton("L1") && Input.GetButton("R1")))
		{
			resetButtonsNotPressed = true;
		}*/

		/*if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			//HardDrop();
		}*/
	}
	private void Update()
	{
		//Pause
		if (!paused && !board.gameOver)
		{
			if (Keyboard.current.enterKey.wasPressedThisFrame)
			{
				board.stopwatch.Stop();
				FindObjectOfType<AudioManager>().Play("Pause");
				board.pausedText.text = "PAUSED";
				board.tilemapRenderer.sortingOrder = -1;
				paused = true;
				return;
			}
		}
		if (paused && !board.gameOver)
		{
			if (Keyboard.current.enterKey.wasPressedThisFrame)
			{
				board.stopwatch.Start();
				board.pausedText.text = "";
				board.tilemapRenderer.sortingOrder = 2;
				paused = false;
			}
			return;
		}
		if (board.gameOver)
		{
			if (Keyboard.current.enterKey.wasPressedThisFrame)
			{
				paused = false;
				board.pausedText.text = "";
				board.tilemapRenderer.sortingOrder = 2;
				board.gameOver = false;
				board.ResetBoard();
			}
			return;
		}

		if (paused)
		{
			return;
		}

		
		//Tetris rate text colour
		if (board.tetrisRate < 25)
		{
			trtColor = "red";
		}
		else if (board.tetrisRate >= 25 && board.tetrisRate < 50)
		{
			trtColor = "orange";
		}
		else if (board.tetrisRate >= 50 && board.tetrisRate < 75)
		{
			trtColor = "yellow";
		}
		else if (board.tetrisRate >= 75)
		{
			trtColor = "#00FF00FF";
		}

		//Drough counter text colour
		if (board.droughtCounter > 25)
		{
			droughtColor = "red";
		}
		else
		{
			droughtColor = "white";
		}

		board.ts = board.stopwatch.Elapsed;

		this.time = Time.time;
		boardSpeed = board.speed;
		//HAxis = Input.GetAxisRaw("Horizontal");
		//VAxis = Input.GetAxisRaw("Vertical");

		this.lockTime += Time.deltaTime;

		//Speed levels
		switch (board.level)
		{
			case 0:
				board.speed = Speed.Lv0;
				break;
			case 1:
				board.speed = Speed.Lv1;
				break;
			case 2:
				board.speed = Speed.Lv2;
				break;
			case 3:
				board.speed = Speed.Lv3;
				break;
			case 4:
				board.speed = Speed.Lv4;
				break;
			case 5:
				board.speed = Speed.Lv5;
				break;
			case 6:
				board.speed = Speed.Lv6;
				break;
			case 7:
				board.speed = Speed.Lv7;
				break;
			case 8:
				board.speed = Speed.Lv8;
				break;
			case 9:
				board.speed = Speed.Lv9;
				break;
			case 10:
			case 11:
			case 12:
				board.speed = Speed.Lv10to12;
				break;
			case 13:
			case 14:
			case 15:
				board.speed = Speed.Lv13to15;
				break;
			case 16:
			case 17:
			case 18:
				board.speed = Speed.Lv16to18;
				break;
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
				board.speed = Speed.Lv19to28;
				break;
			case 29:
				board.speed = Speed.Lv29;
				break;
		}

		//Current game playtime
		string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", board.ts.Hours, board.ts.Minutes, board.ts.Seconds, board.ts.Milliseconds / 10);

		//Stats text output
		board.stats.text = "NEXT:\n\n\n\n\n\n\n\nHIGHSCORE: " + string.Format("{0:n0}", board.highscore) + "\nSCORE: " + string.Format("{0:n0}", board.score) + "\n\nLEVEL: " + board.level.ToString() + "\nLINES: " + board.lines.ToString() + "\n\nTETRIS RATE: " + "<color=" + trtColor + ">" + board.tetrisRate.ToString() + "%</color> <color=#00FF00FF>" + board.tetrises + "</color>\n\nDROUGHT: <color=" + droughtColor + ">" + board.droughtCounter.ToString() + "</color>\nMAX DROUGHT: " + board.maxDrought.ToString() + "\n\nTIME: " + elapsedTime.ToString();

		//Clear board (always put actions below this line of code)
		if (!(board.lockWait || board.lineClearWait || board.tetrisClearWait))
		{
			this.board.Clear(this);
		}

		//Move
		//left
		if (Keyboard.current.leftArrowKey.wasPressedThisFrame && !readyLeft)
		{
			Move(Vector2Int.left);
			downTime = Time.time;
			pressTime = downTime + dasDelay;
			readyLeft = true;
		}
		if (Keyboard.current.leftArrowKey.wasReleasedThisFrame)
		{
			readyLeft = false;
			holdingDas = false;
		}
		if ((Time.time >= pressTime) && readyLeft)
		{
			holdingDas = true;
			if (!shift)
			{
				downTime = Time.time;
				pressTime = downTime + dasRate;
				shift = true;
			}
			if (Time.time >= pressTime && shift)
			{
				Move(Vector2Int.left);
				shift = false;
			}
		}

		//right
		if (Keyboard.current.rightArrowKey.wasPressedThisFrame && !readyRight)
		{
			Debug.Log("moved right");
			Move(Vector2Int.right);
			downTime = Time.time;
			pressTime = downTime + dasDelay;
			readyRight = true;
		}
		if (Keyboard.current.rightArrowKey.wasReleasedThisFrame)
		{
			readyRight = false;
			holdingDas = false;
		}
		if ((Time.time >= pressTime) && readyRight)
		{
			holdingDas = true;
			if (!shift)
			{
				downTime = Time.time;
				pressTime = downTime + dasRate;
				shift = true;
			}
			if (Time.time >= pressTime && shift)
			{
				Move(Vector2Int.right);
				shift = false;
			}
		}

		//Pushdown
		if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
		{
			Debug.Log("Pressed pushdown");
			pushedDown = true;
			fallTime = Time.time;
		}
		if (pushedDown)
		{
			prevFallDelay = board.speed;
			if (board.level < 19)
			{
				board.speed = Speed.Lv19to28;
			}
		}
		if (Keyboard.current.downArrowKey.wasReleasedThisFrame || Keyboard.current.sKey.wasReleasedThisFrame)
		{
			Debug.Log("Released pushdown");
			board.speed = prevFallDelay;
			pushedDown = false;
		}

		//Rotations
		if (Keyboard.current.zKey.wasPressedThisFrame)
		{
			Rotate(-1);
		}
		else if (Keyboard.current.xKey.wasPressedThisFrame)
		{
			Rotate(1);
		}
		//Fall
		if (Time.time >= this.fallTime)
		{
			if (pushingDown && pushedDown)
			{
				board.score += 1;
			}
			if (!(board.lockWait || board.lineClearWait || board.tetrisClearWait))
			{
				if (spawnedPiece)
				{
					Debug.Log("fell");
					Fall();
				}
				if (this.lockTime >= this.lockDelay)
				{
					Debug.Log("spawned a piece");
					this.board.SpawnPiece();
					spawnedPiece = true;
					this.board.SpawnNextPiece();
				}
			}
		}
		this.board.Set(this);
	}

	private void Fall()
	{
		if (board.lockWait || board.lineClearWait || board.tetrisClearWait)
		{
			return;
		}

		this.fallTime = Time.time + board.speed;

		Move(Vector2Int.down);
		if (this.lockTime >= this.lockDelay)
		{
			Debug.Log("Set the piece");
			pushedDown = false;
			readyLeft = false;
			readyRight = false;
			board.speed = prevFallDelay;
			this.board.Set(this);
			this.board.ClearLines();
			if (board.lockWait || board.lineClearWait || board.tetrisClearWait)
			{
				spawnedPiece = false;
				return;
			}
		}
	}

	/*private void HardDrop()
	{
		while (Move(Vector2Int.down) == true)
		{
			continue;
		}
	}*/


	private bool Move(Vector2Int translation)
	{
		if (board.lockWait || board.lineClearWait || board.tetrisClearWait)
		{
			return true;
		}
		Vector3Int newPosition = this.position;
		Vector3Int origPosition = newPosition;
		newPosition.x += translation.x;
		newPosition.y += translation.y;

		bool valid = this.board.IsValidPosition(this, newPosition);

		if (valid)
		{
			if (newPosition.x != origPosition.x)
			{
				FindObjectOfType<AudioManager>().Play("Shift");
			}
			this.position = newPosition;
			this.lockTime = 0f;
		}

		return valid;
	}
	private void Rotate(int direction)
	{
		if (board.lockWait || board.lineClearWait || board.tetrisClearWait)
		{
			return;
		}
		int originalRotation = this.rotationIndex;
		/*switch (this.data.tetromino)
		{
			case Tetromino.I:
				this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 2);
				break;
			case Tetromino.Z:
				this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 2);
				break;
			case Tetromino.S:
				this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 2);
				break;
			default:
				this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);
				break;
		}*/

		if (this.data.tetromino != Tetromino.O)
		{
			ApplyRotationMatrix(direction);

			if (!TestWallKicks(this.rotationIndex, direction))
			{
				Debug.Log("am inside the TestWallKicks if statement?");
				this.rotationIndex = originalRotation;
				ApplyRotationMatrix(-direction);
			}
		}
		else
		{
			Debug.Log("is it an O piece?");

			FindObjectOfType<AudioManager>().Play("Rotate");
		}
	}

	private void ApplyRotationMatrix(int direction)
	{
		Debug.Log("Am I inside that ApplyRotationMatrix function yet?");

		FindObjectOfType<AudioManager>().Play("Rotate");

		for (int i = 0; i < this.data.cells.Length; i++)
		{
			Vector3 cell = this.cells[i];

			int x, y;

			x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
			y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));

			this.cells[i] = new Vector3Int(x, y, 0);
		}
	}

	private bool TestWallKicks(int rotationIndex, int rotationDirection)
	{
		int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

		for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
		{
			Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

			Debug.Log(i);

			if (Move(translation))
			{
				return true;
			}
		}
		Debug.Log("false wallkick test?");

		return false;
	}

	private int GetWallKickIndex(int rotationIndex, int rotationDirection)
	{
		int wallKickIndex = rotationIndex * 2;

		if (rotationDirection < 0)
		{
			wallKickIndex--;
		}

		return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
	}

	public int Wrap(int input, int min, int max)
	{
		if (input < min)
		{
			return max - (min - input) % (max - min);
		}
		else
		{
			return min + (input - min) % (max - min);
		}
	}
}