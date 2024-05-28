using ResilientCore;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWinState : BaseGameState
{  
	public PlayerWinState(GameManager manager) : base(EGameState.PlayerWin, manager) { }
	public override void Enter()
	{
		Manager.TimerText.text = "Game Won!!!";
		Manager.PlayerWinPanel.SetActive(true);
	}

	public override void Exit()
	{
		Manager.Restart = false;
		Manager.Continue = false;
	}
	public override EGameState GetNextState()
	{
		if (Manager.Restart)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		if (Manager.Continue)
		{
			return EGameState.Setup;
		}
		return Key;
	}
}