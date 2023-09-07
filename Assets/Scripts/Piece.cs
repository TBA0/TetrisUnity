using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;
using System.Collections;
using System.Runtime.Remoting.Contexts;
using UnityEngine.InputSystem.Controls;

public class Piece : MonoBehaviour
{
	public Board board { get; private set; }
	public TetrominoData data { get; private set; }
	public Vector3Int[] cells { get; private set; }
	public Vector3Int position { get; private set; }
	public int rotationIndex { get; private set; }

	public float fps;

	public float lockDelay = 0f;
	private float prevFallDelay;

	private bool pushedDown = false;
	public int pushdownPoints = 0;

	public float time = 0f;
	public float fallTime = 1.6f;
	private float lockTime;

	private float downTime, upTime, pressTime = 0;
	public int pushDownRate = 40;
	public bool readyLeft = false;
	public bool readyRight = false;

	public float dasRate = 0.08f;
	public float dasDelay = 0.25f;
	private bool shift = false;

	public bool holdingDas = false;

	public bool unrotate = false;

	public float HAxis = 0f;
	public float VAxis = 0f;

	public float boardSpeed = 0f;

	public bool pieceLocked = false;

	public bool spawnedPiece = true;

	public bool paused = false;

	public bool unpaused = false;

	public string trtColor = "red";
	public string droughtColor = "white";

	public bool resetButtonsNotPressed = true;

	public bool reset = true;

	public bool firstFall = true;

	public float tapHz;
	public float lastTapL;
	public float lastTapR;

	private Input m_PlayerInput;
	private ButtonControl b_Start;
	private ButtonControl b_MoveLeft;
	private ButtonControl b_MoveRight;
	private ButtonControl b_RotateCCW;
	private ButtonControl b_RotateCW;
	private ButtonControl b_SoftDrop;
	private ButtonControl b_Reset;

	private void Awake()
	{
		m_PlayerInput = new Input();
		m_PlayerInput.Enable();

		b_Start = (ButtonControl)m_PlayerInput.Player.Start.controls[0];
		b_MoveLeft = (ButtonControl)m_PlayerInput.Player.MoveLeft.controls[0];
		b_MoveRight = (ButtonControl)m_PlayerInput.Player.MoveRight.controls[0];
		b_RotateCCW = (ButtonControl)m_PlayerInput.Player.RotateCCW.controls[0];
		b_RotateCW = (ButtonControl)m_PlayerInput.Player.RotateCW.controls[0];
		b_SoftDrop = (ButtonControl)m_PlayerInput.Player.SoftDrop.controls[0];
		b_Reset = (ButtonControl)m_PlayerInput.Player.Reset.controls[0];
	}

	public void Initialize(Board board, Vector3Int position, TetrominoData data)
	{
		this.board = board;
		this.position = position;
		this.data = data;
		rotationIndex = 0;
		if (!reset)
		{
			fallTime = Time.time + board.speed;
		}
		else
		{
			fallTime = Time.time + 1.6f;
			reset = false;
		}
		lockTime = 0f;
		spawnedPiece = true;
		pushdownPoints = 0;

		unrotate = false;

		if (cells == null)
		{
			cells = new Vector3Int[data.cells.Length];
		}

		for (int i = 0; i < data.cells.Length; i++)
		{
			cells[i] = (Vector3Int)data.cells[i];
		}

	}

	private void Update()
	{
		//Get FPS
		fps = 1.0f / Time.deltaTime;

		//Current game playtime
		string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", board.ts.Hours, board.ts.Minutes, board.ts.Seconds, board.ts.Milliseconds / 10);

		//Stats text output
		board.stats.text = "FPS: " + fps.ToString("0.0") + "\n\n\nNEXT:\n\n\n\n\n\n  HIGHSCORE: " + string.Format("{0:n0}", board.highscore) + "\n      SCORE: " + string.Format("{0:n0}", board.score) + "\n\n      LEVEL: " + board.level.ToString() + "\n      LINES: " + board.lines.ToString() + "\n\nTETRIS RATE: " + "<color=" + trtColor + ">" + board.tetrisRate.ToString() + "%</color> <color=#00FF00FF>" + board.tetrises + "</color>\n\n    DROUGHT: <color=" + droughtColor + ">" + board.droughtCounter.ToString() + "</color>\nMAX DROUGHT: " + board.maxDrought.ToString() + "\n\n       TIME: " + elapsedTime.ToString();

		//Tap hz text output
		board.tapHz.text = "TAP: " + tapHz.ToString("0.0") + "Hz";

		if (!Application.isFocused && !paused && !board.gameOver)
		{
			m_PlayerInput.Disable();
			board.OnPause.Invoke();
			Pause();
			return;
		}
		else if (Application.isFocused)
		{
			m_PlayerInput.Enable();
		}

		if ((m_PlayerInput.Player.Start.activeControl != null) && ((ButtonControl)m_PlayerInput.Player.Start.activeControl != b_Start))
		{
			b_Start = (ButtonControl)m_PlayerInput.Player.Start.activeControl;
		}
		if ((m_PlayerInput.Player.MoveLeft.activeControl != null) && ((ButtonControl)m_PlayerInput.Player.MoveLeft.activeControl != b_Start))
		{
			b_MoveLeft = (ButtonControl)m_PlayerInput.Player.MoveLeft.activeControl;
		}
		if ((m_PlayerInput.Player.MoveRight.activeControl != null) && ((ButtonControl)m_PlayerInput.Player.MoveRight.activeControl != b_Start))
		{
			b_MoveRight = (ButtonControl)m_PlayerInput.Player.MoveRight.activeControl;
		}
		if ((m_PlayerInput.Player.RotateCCW.activeControl != null) && ((ButtonControl)m_PlayerInput.Player.RotateCCW.activeControl != b_Start))
		{
			b_RotateCCW = (ButtonControl)m_PlayerInput.Player.RotateCCW.activeControl;
		}
		if ((m_PlayerInput.Player.RotateCW.activeControl != null) && ((ButtonControl)m_PlayerInput.Player.RotateCW.activeControl != b_Start))
		{
			b_RotateCW = (ButtonControl)m_PlayerInput.Player.RotateCW.activeControl;
		}
		if ((m_PlayerInput.Player.SoftDrop.activeControl != null) && ((ButtonControl)m_PlayerInput.Player.SoftDrop.activeControl != b_Start))
		{
			b_SoftDrop = (ButtonControl)m_PlayerInput.Player.SoftDrop.activeControl;
		}
		if ((m_PlayerInput.Player.Reset.activeControl != null) && ((ButtonControl)m_PlayerInput.Player.Reset.activeControl != b_Start))
		{
			b_Reset = (ButtonControl)m_PlayerInput.Player.Reset.activeControl;
		}

		//Pause
		if (!paused && !board.gameOver)
		{
			if (b_Start.wasPressedThisFrame && !firstFall && !unpaused)
			{
				unpaused = true;
				board.OnPause.Invoke();
				Pause();
				return;
			}
		}
		else
		{
			return;
		}

		unpaused = false;


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

		time = Time.time;
		boardSpeed = board.speed;
		//HAxis = Input.GetAxisRaw("Horizontal");
		//VAxis = Input.GetAxisRaw("Vertical");

		lockTime += Time.deltaTime;

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

		//Clear board (always put actions below this line of code)
		if (!(board.lockWait || board.lineClearWait) && spawnedPiece)
		{
			board.Clear(this);
		}

		//Move
		//left
		if (b_MoveLeft.wasPressedThisFrame && !readyLeft && !pushedDown)
		{
			Move(Vector2Int.left);
			downTime = Time.time;
			pressTime = downTime + dasDelay;
			readyLeft = true;
			tapHz = 1.0f / (Time.time - lastTapL);
			lastTapL = Time.time;
		}
		if (b_SoftDrop.wasPressedThisFrame) readyLeft = false;
		if (b_MoveLeft.wasReleasedThisFrame)
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
		if (b_MoveRight.wasPressedThisFrame && !readyRight && !pushedDown)
		{
			Move(Vector2Int.right);
			downTime = Time.time;
			pressTime = downTime + dasDelay;
			readyRight = true;
			tapHz = 1.0f / (Time.time - lastTapR);
			lastTapR = Time.time;
		}
		if (b_SoftDrop.wasPressedThisFrame) readyRight = false;
		if (b_MoveRight.wasReleasedThisFrame)
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
		if (b_SoftDrop.wasPressedThisFrame && !holdingDas)
		{
			pushedDown = true;
			fallTime = Time.time;
		}
		if (pushedDown)
		{
			if (b_MoveRight.wasPressedThisFrame || b_MoveLeft.wasPressedThisFrame) pushedDown = false;
			prevFallDelay = board.speed;
			if (board.level < 19)
			{
				board.speed = Speed.Lv19to28;
			}
		}
		if (b_SoftDrop.wasReleasedThisFrame)
		{
			board.speed = prevFallDelay;
			pushedDown = false;
		}

		//Rotations
		if (b_RotateCCW.wasPressedThisFrame)
		{
			if (data.tetromino == Tetromino.S || data.tetromino == Tetromino.Z || data.tetromino == Tetromino.I)
			{
				Rotate(1);
			}
			else
			{
				Rotate(-1);
			}
		}
		else if (b_RotateCW.wasPressedThisFrame)
		{
			Rotate(1);
		}

		//Hard drop
		//if (Input.GetKeyDown(KeyCode.UpArrow))
		//{
		//HardDrop();
		//}

		//Fall
		if (Time.time >= fallTime)
		{
			if (pushedDown)
			{
				pushdownPoints += 1;
			}
			else
			{
				pushdownPoints = 0;
			}
			if (!(board.lockWait || board.lineClearWait))
			{
				if (spawnedPiece)
				{
					Fall();
				}
			}
		}
		if (!spawnedPiece)
		{
			if (!(board.lockWait || board.lineClearWait))
			{
				board.SpawnPiece();
				board.SpawnNextPiece();
				spawnedPiece = true;
			}
		}
		if (!(board.lockWait || board.lineClearWait))
		{
			board.Clear(this);
		}
		if (!(board.lockWait || board.lineClearWait))
		{
			board.Set(this);
		}
	}

	private void Fall()
	{
		fallTime = Time.time + board.speed;

		Move(Vector2Int.down);

		board.Set(this);

		firstFall = false;

		if (lockTime >= lockDelay)
		{
			board.score += pushdownPoints;
			pushedDown = false;
			board.speed = prevFallDelay;
			board.ClearLines();
			spawnedPiece = false;
		}
	}

	public void Pause()
	{
		if (!paused)
		{
			board.stopwatch.Stop();
			FindObjectOfType<AudioManager>().Play("Pause");
			board.tilemapRenderer.sortingOrder = -1;
			paused = true;
		}
		else
		{
			board.stopwatch.Start();
			board.tilemapRenderer.sortingOrder = 2;
			paused = false;
		}
	}

	public void Reset()
	{
		fallTime = Time.time + 1.6f;
		reset = true;
		paused = false;
		firstFall = true;
		board.tilemapRenderer.sortingOrder = 2;
		board.gameOver = false;
		board.ResetBoard();
		board.SpawnPiece();
		board.SpawnNextPiece();
	}

	/*private void HardDrop()
	{
		while (Move(Vector2Int.down) == true)
		{
			continue;
		}
	}*/

	private bool CheckValidSpace(Vector2Int translation)
	{
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
		return board.IsValidPosition(this, newPosition);
    }


	private bool Move(Vector2Int translation)
	{
		if (board.lockWait || board.lineClearWait)
		{
			return true;
		}
		Vector3Int newPosition = position;
		Vector3Int origPosition = newPosition;
		newPosition.x += translation.x;
		newPosition.y += translation.y;

		bool valid = board.IsValidPosition(this, newPosition);

		if (valid)
		{
			if (newPosition.x != origPosition.x)
			{
				FindObjectOfType<AudioManager>().Play("Shift");
			}
			position = newPosition;
			lockTime = 0f;
		}

		return valid;
	}
	private void Rotate(int direction)
	{
		if (board.lockWait || board.lineClearWait) return;
		if (!spawnedPiece)
		{
			return;
		}
		int originalRotation = rotationIndex;
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

		if (data.tetromino != Tetromino.O && data.tetromino != Tetromino.I && data.tetromino != Tetromino.S && data.tetromino != Tetromino.Z)
		{
			ApplyRotationMatrix(direction);

			if (!TestWallKicks(rotationIndex, direction))
			{
				rotationIndex = originalRotation;
				ApplyRotationMatrix(-direction);
			}
			else
			{
				FindObjectOfType<AudioManager>().Play("Rotate");
			}
		}
		else if (data.tetromino != Tetromino.O)
		{
			if (unrotate)
			{
				ApplyRotationMatrix(-direction);
				unrotate = false;

				if (!TestWallKicks(rotationIndex, -direction))
				{
					rotationIndex = originalRotation;
					ApplyRotationMatrix(direction);
				}
				else
				{
					FindObjectOfType<AudioManager>().Play("Rotate");
				}
			}
			else
			{
				ApplyRotationMatrix(direction);
				unrotate = true;

				if (!TestWallKicks(rotationIndex, direction))
				{
					rotationIndex = originalRotation;
					ApplyRotationMatrix(-direction);
				}
				else
				{
					FindObjectOfType<AudioManager>().Play("Rotate");
				}
			}
		}
		else
		{
			FindObjectOfType<AudioManager>().Play("Rotate");
		}
	}

	private void ApplyRotationMatrix(int direction)
	{
		for (int i = 0; i < data.cells.Length; i++)
		{
			Vector3 cell = cells[i];

			int x, y;

			x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
			y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));

			cells[i] = new Vector3Int(x, y, 0);
		}
	}

	private bool TestWallKicks(int rotationIndex, int rotationDirection)
	{
		int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

		for (int i = 0; i < data.wallKicks.GetLength(1); i++)
		{
			Vector2Int translation = data.wallKicks[wallKickIndex, i];

			if (Move(translation))
			{
				return true;
			}
		}
		return false;
	}

	private int GetWallKickIndex(int rotationIndex, int rotationDirection)
	{
		int wallKickIndex = rotationIndex * 2;

		if (rotationDirection < 0)
		{
			wallKickIndex--;
		}

		return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
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