using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetStartSpeed : MonoBehaviour
{
	public Slider sliderUI;
	public Image sliderHandleImage;
	public TextMeshProUGUI startSpeedText;
	public Settings settings;

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
			settings.ReadSettingsFile("settings.txt");
		}
		sliderUI.value = settings.startLevel;
		UpdateStartSpeed();
	}

	public void UpdateStartSpeed()
    {
		int startSpeedInt = Mathf.RoundToInt(sliderUI.value);
		startSpeedText.text = startSpeedInt.ToString();
		settings.startLevel = startSpeedInt;
		settings.WriteSettingsFile("settings.txt");
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
}
