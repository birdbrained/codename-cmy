using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour 
{
	[SerializeField]
	private Toggle fullscreenToggle;
	[SerializeField]
	private Dropdown resolutionDropdown;
	[SerializeField]
	private Slider musicVolumeSlider;
	[SerializeField]
	private Slider sfxVolumeSlider;

	public Resolution[] resolutions;

	void OnEnable()
	{
		//instance = new GameSettings();

		fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
		resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionUpdate(); });
		musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeUpdate(); });
		sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSfxVolumeUpdate(); });

		resolutions = Screen.resolutions;
		foreach (Resolution r in resolutions)
		{
			resolutionDropdown.options.Add(new Dropdown.OptionData(r.ToString()));
		}
	}

	public void OnFullscreenToggle()
	{
		Screen.fullScreen = fullscreenToggle.isOn;
		GameManager.GSInstance.fullscreen = fullscreenToggle.isOn;
	}

	public void OnResolutionUpdate()
	{
		Screen.SetResolution(resolutions[resolutionDropdown.value].width,
			resolutions[resolutionDropdown.value].height,
			Screen.fullScreen);
	}

	public void OnMusicVolumeUpdate()
	{
		GameManager.GSInstance.musicVolume = musicVolumeSlider.value;
	}

	public void OnSfxVolumeUpdate()
	{
		GameManager.GSInstance.sfxVolume = sfxVolumeSlider.value;
	}

	public void SaveSettings()
	{
		
	}

	public void LoadSettings()
	{
		
	}
}
