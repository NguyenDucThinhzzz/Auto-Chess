using ResilientCore;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathState : BaseGameState
{
	public PlayerDeathState(GameManager manager) : base(EGameState.PlayerDeath, manager) { }
	public override void Enter()
	{
		Manager.PlayerDeathPanel.SetActive(true);
		Manager.TimerText.text = "Died";
	}

	public override void Exit()
	{
		Manager.Restart = false;
	}
	public override EGameState GetNextState()
	{
		if (Manager.Restart)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		return Key;
	}
}