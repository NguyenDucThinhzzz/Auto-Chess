using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanelControl : MonoBehaviour
{
    [field: SerializeField] public GameObject PausePanel { get; private set; }

	private void Start()
	{
		PlayerInput.Instance.Actions.Pause.started += Pause_started;
	}
	private void OnDestroy()
	{
		PlayerInput.Instance.Actions.Pause.started -= Pause_started;
	}

	private void Pause_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		PausePanel.SetActive(!PausePanel.active);
	}

}
