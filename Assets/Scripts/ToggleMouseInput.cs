using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMouseInput : MonoBehaviour
{
    public Settings settings;

	public Toggle toggleUI;

	void Start()
    {
		if (File.Exists("settings.txt"))
		{
			settings.ReadSettingsFile("settings.txt");
		}
		toggleUI.isOn = settings.mouseInputEnabled;
	}

	public void Enable()
	{
		settings.mouseInputEnabled = toggleUI.isOn;
		if (File.Exists("settings.txt"))
		{
			settings.WriteSettingsFile("settings.txt");
		}
	}
}
