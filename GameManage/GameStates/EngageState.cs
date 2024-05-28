using ResilientCore;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EngageState : BaseGameState
{
	private bool won;
	private bool lost;
	private int count;
	public EngageState(GameManager manager) : base(EGameState.Engage, manager) { }
	public override void Enter()
	{
		won = false;
		lost = false;
		Manager.TimerText.text = "Engage";
	}

	public override void Exit()
	{
	}

	public override EGameState GetNextState()
	{
		if (won)
			return EGameState.Win;
		if (lost)
			return EGameState.Lose;
		return Key;
	}

	public override void FixedUpdate()
	{
		AutoControllChess();
	}

	public void AutoControllChess()
	{
		count = 0;
		foreach (var chess in Manager.PlayerController.OnFieldChess)
		{
			if (chess.IsDead) continue;
			chess.ChessBehaviour();
			count++;
		}
		if (count == 0)
		{
			lost = true;
			return;
		}

		count = 0;
		foreach (var chess in Manager.EnemyController.OnFieldChess)
		{
			if (chess.IsDead) continue;
			chess.ChessBehaviour();
			count++;
		}
		if (count == 0)
		{
			won = true;
			return;
		}
	}
}