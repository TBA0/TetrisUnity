using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetGlobalAudioLevel : MonoBehaviour
{
	public Slider sliderUI;
	public TextMeshProUGUI globalAudioLevelText;
	public Settings settings;

	void Start()
	{
		if (File.Exists("settings.txt"))
		{
			settings.ReadSettingsFile("settings.txt");
		}
		sliderUI.value = settings.masterVolume;
		UpdateGlobalAudioLevel();
	}

	public void UpdateGlobalAudioLevel()
	{
		float masterVolumeFloat = sliderUI.value;
		int masterVolumePercentage = Mathf.RoundToInt(sliderUI.value * 100);
		AudioListener.volume = masterVolumeFloat;
		settings.masterVolume = masterVolumeFloat;
		globalAudioLevelText.text = masterVolumePercentage + "%";
		if (File.Exists("settings.txt"))
		{
			settings.WriteSettingsFile("settings.txt");
		}
	}
}
