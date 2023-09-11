using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInvertScroll : MonoBehaviour
{
	public Settings settings;

	public Toggle toggleUI;

	void Start()
	{
		if (File.Exists("settings.txt"))
		{
			settings.ReadSettingsFile("settings.txt");
		}
		toggleUI.isOn = settings.invertScroll;
	}

	public void Enable()
	{
		settings.invertScroll = toggleUI.isOn;
		if (File.Exists("settings.txt"))
		{
			settings.WriteSettingsFile("settings.txt");
		}
	}
}
