using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	[field: Header("Pause Panel")]
	[field: SerializeField] public GameObject PausePanel { get; private set; }
	[field: SerializeField] public TextMeshProUGUI DebugText { get; private set; }
	[field: Header("Settings Panel")]
	[field: SerializeField] public GameObject SettingsPanel { get; private set; }

	private void OnEnable()
	{
		PlayerInput.Instance.enabled = false;
		Time.timeScale = 0f;
		PausePanel.SetActive(true);
		SettingsPanel.SetActive(false);
	}
	private void OnDisable()
	{
		PlayerInput.Instance.enabled = true;
		Time.timeScale = 1f;
	}
	public void Resume()
	{
		gameObject.SetActive(false);
	}
	public void TriggerDebug()
	{
		if (Utility.Instance.enableDebug)
		{
			DebugText.text = "Debug: Off";
			Utility.Instance.DisableDebug();
		}
		else
		{
			DebugText.text = "Debug: On";
			Utility.Instance.EnableDebug();
		}
	}
	public void ReturnToMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Menu");
	}

}
