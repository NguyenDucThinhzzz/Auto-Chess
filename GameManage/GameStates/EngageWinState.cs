using ResilientCore;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EngageWinState : BaseGameState
{
	public float Timer { get; private set; }
	public EngageWinState(GameManager manager) : base(EGameState.Win, manager) { }
	public override void Enter()
	{
		Timer = Manager.WaitTimeAfterEngage;
	}

	public override void Exit()
	{
		Manager.PlayerController.MaxChess++;
		Manager.PlayerController.GiveRandomChessAmount(Manager.PlayerController.GiveAmount);
		Manager.UpdateChessCountText();
		Manager.ResetChesses();
		Manager.EnemyController.NextWave();
	}
	public override EGameState GetNextState()
	{
		if (Manager.EnemyController.CurrentWave+1 == Manager.EnemyController.ChessData.Waves.Count)
		{
			return EGameState.PlayerWin;
		}
		if (Timer <= 0)
		{
			return EGameState.Setup;
		}
		return Key;
	}

	public override void Update()
	{
		Manager.TimerText.text = "Win " + (int)Timer;
		Timer -= Time.deltaTime;
	}
}