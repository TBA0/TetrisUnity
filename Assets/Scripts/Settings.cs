using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
public class Settings : ScriptableObject
{
	private static string[] Options = new string[1] { "start-speed" };

	public int startLevel = 0;

	public void ReadSettingsFile(string dir)
	{
		StreamReader reader = new StreamReader(dir);
		string line;
		while ((line = reader.ReadLine()) != null)
		{
			startLevel = 0;
			if (line.Contains(Options[0] + "="))
			{
				if (int.TryParse(line.Split('=')[1], out int level))
				{
					startLevel = level;
				}
			}
		}
	}
}
