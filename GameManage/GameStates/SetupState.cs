using ResilientCore;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SetupState : BaseGameState
{
	public float Timer { get; private set; }
	public SetupState(GameManager manager) : base(EGameState.Setup, manager) { }
	public override void Enter()
	{
		Timer = Manager.WaitTime;
		Manager.ChessDragging.enabled = true;
		Manager.MessageText.ShowText("Wave " + (Manager.EnemyController.CurrentWave + 1));
	}

	public override void Exit()
	{
		Manager.ChessDragging.enabled = false;
	}

	public override void FixedUpdate()
	{

	}

	public override EGameState GetNextState()
	{
		if(Timer <= 0)
		{
			return EGameState.Engage;
		}
		return Key;
	}

	public override void Update()
	{
		Manager.TimerText.text = "Setup " + (int)Timer;
		Timer -= Time.deltaTime;
	}
}