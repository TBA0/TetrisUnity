using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetStartSpeed : MonoBehaviour
{
	private static string[] Options = new string[1] { "start-speed" };
	public Slider sliderUI;
	public Image sliderHandleImage;
	public TextMeshProUGUI startSpeedText;
	public Settings settings;

	[SerializeField]
	public Sprite Lv0;
	public Sprite Lv1;
	public Sprite Lv2;
	public Sprite Lv3;
	public Sprite Lv4;
	public Sprite Lv5;
	public Sprite Lv6;
	public Sprite Lv7;
	public Sprite Lv8;
	public Sprite Lv9;

	void Start()
	{
		if (File.Exists("settings.txt"))
		{
			ReadSettingsFile("settings.txt");
		}
		sliderUI.value = settings.startLevel;
		UpdateStartSpeed();
	}

	public void UpdateStartSpeed()
    {
		int startSpeedInt = Mathf.RoundToInt(sliderUI.value);
		Debug.Log(startSpeedInt);
		startSpeedText.text = startSpeedInt.ToString();
		settings.startLevel = startSpeedInt;
		if (File.Exists("settings.txt"))
		{
			string saveSettings = "start-speed=" + settings.startLevel;
			File.WriteAllText("settings.txt", saveSettings);
		}
		switch (startSpeedInt % 10)
		{
			case 0:
				sliderHandleImage.sprite = Lv0;
				break;
			case 1:
				sliderHandleImage.sprite = Lv1;
				break;
			case 2:
				sliderHandleImage.sprite = Lv2;
				break;
			case 3:
				sliderHandleImage.sprite = Lv3;
				break;
			case 4:
				sliderHandleImage.sprite = Lv4;
				break;
			case 5:
				sliderHandleImage.sprite = Lv5;
				break;
			case 6:
				sliderHandleImage.sprite = Lv6;
				break;
			case 7:
				sliderHandleImage.sprite = Lv7;
				break;
			case 8:
				sliderHandleImage.sprite = Lv8;
				break;
			case 9:
				sliderHandleImage.sprite = Lv9;
				break;
			default:
				sliderHandleImage.sprite = Lv0;
				break;
		}
	}

	public void ReadSettingsFile(string dir)
	{
		StreamReader reader = new StreamReader(dir);
		string line;
		while ((line = reader.ReadLine()) != null)
		{
			settings.startLevel = 0;
			if (line.Contains(Options[0] + "="))
			{
				if (int.TryParse(line.Split('=')[1], out int level))
				{
					settings.startLevel = level;
				}
			}
		}
	}
}
