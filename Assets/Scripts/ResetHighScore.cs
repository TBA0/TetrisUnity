using System.Collections;
using UnityEngine;

public class ResetHighScore : MonoBehaviour
{
	public void RemoveHighScore()
	{
		if (PlayerPrefs.HasKey("HighScore"))
		{
			PlayerPrefs.SetInt("HighScore", 0);
			PlayerPrefs.Save();
		}
	}
}
