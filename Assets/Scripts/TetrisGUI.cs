using UnityEngine;

public class TetrisGUI : MonoBehaviour
{
	public Board board { get; private set; }
	[SerializeField]
	GUIStyle style;
	public string score;
	void Start()
	{
		board = GetComponentInChildren<Board>();
	}
	void OnGUI()
	{
		//int scoreInt = board.score;
		//score = scoreInt.ToString();
		GUI.Label(new Rect(675, 75, 300, 50), "SCORE: ", style);
    }
}
