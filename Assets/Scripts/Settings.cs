using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
public class Settings : ScriptableObject
{
	private static string[] Options = new string[1] { "start-speed" };

	public int startLevel = 0;
	public float masterVolume = 1.0f;

	public void ReadSettingsFile(string dir)
	{
		StreamReader reader = new StreamReader(dir);
		string line;
		while ((line = reader.ReadLine()) != null)
		{
			if (line.Contains('='))
			{
				switch (line.Split('=')[0])
				{
					case "master-volume":
						if (float.TryParse(line.Split('=')[1], out float volume))
						{
							masterVolume = volume;
						}
						break;
					case "start-speed":
						if (int.TryParse(line.Split('=')[1], out int level))
						{
							startLevel = level;
						}
						break;
				}
			}
		}
	}
	public void WriteSettingsFile(string dir)
	{
		string saveSettings = "start-speed=" + startLevel + "\nmaster-volume=" + masterVolume;
		File.WriteAllText(dir, saveSettings);
	}
}
