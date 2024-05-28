using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsHandling : MonoBehaviour
{
	[field: SerializeField] public Toggle FullScreen { get; private set; }
	[field: SerializeField] public TMP_Dropdown QualityDropdown { get; private set; }
	[field: SerializeField] public TMP_Dropdown ResolutionDropdown { get; private set; }
	public List<Resolution> InMenuResolutions { get; private set; }
	public double HighestRefreshRate { get; private set; }
	Resolution[] resolutions;
	private void Awake()
	{
		InMenuResolutions = new List<Resolution>();
		resolutions = Screen.resolutions;
		GetMaxRefresh();
		SetUpResolutionSetting();

		QualityDropdown.value = QualitySettings.GetQualityLevel();
	}

	private void SetUpResolutionSetting()
	{
		ResolutionDropdown.ClearOptions();
		List<string> options = new List<string>();
		int temp = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			if (resolutions[i].refreshRateRatio.value != HighestRefreshRate) continue;
			string op = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(op);
			InMenuResolutions.Add(resolutions[i]);
			if (resolutions[i].width == Screen.currentResolution.width &&
				resolutions[i].height == Screen.currentResolution.height)
			{
				temp = i;
			}
		}
		ResolutionDropdown.AddOptions(options);
		ResolutionDropdown.RefreshShownValue();
		ResolutionDropdown.value = temp;
	}
	private void GetMaxRefresh()
	{
		foreach (var resolution in resolutions)
		{
			if (HighestRefreshRate < resolution.refreshRateRatio.value)
				HighestRefreshRate = resolution.refreshRateRatio.value;
		}
	}

	//Settings Functions
	public void ChangeGraphicsQuality(int value)
	{
		if (QualitySettings.GetQualityLevel() == value) return;
		QualitySettings.SetQualityLevel(value);
	}
	public void ChangeFullScreen(bool value)
	{
		if (Screen.fullScreen == value) return;
		Screen.fullScreen = value;
	}
	public void ChangeResolution(int value)
	{
		if (InMenuResolutions[value].width == Screen.currentResolution.width &&
				InMenuResolutions[value].height == Screen.currentResolution.height)
			return;
			Screen.SetResolution(InMenuResolutions[value].width, InMenuResolutions[value].height, Screen.fullScreenMode, InMenuResolutions[value].refreshRateRatio);
	}
	public void ChangeSettings()
	{
		ChangeGraphicsQuality(QualityDropdown.value);
		ChangeResolution(ResolutionDropdown.value);
		ChangeFullScreen(FullScreen.isOn);
	}
}
