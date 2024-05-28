using ResilientCore;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EngageLoseState : BaseGameState
{
	public float Timer { get; private set; }
	public EngageLoseState(GameManager manager) : base(EGameState.Lose, manager) { }
	public override void Enter()
	{
		Timer = Manager.WaitTimeAfterEngage;
		DamagePlayer();
	}

	public override void Exit()
	{
		Manager.PlayerController.GiveRandomChessAmount(Manager.PlayerController.GiveAmount/2);
		Manager.ResetChesses();
	}

	public override void FixedUpdate()
	{

	}

	public override EGameState GetNextState()
	{
		if(Manager.PlayerController.HP <= 0)
		{
			return EGameState.PlayerDeath;
		}
		if(Timer <= 0)
		{
			return EGameState.Setup;
		}
		return Key;
	}

	public override void Update()
	{
		Manager.TimerText.text = "Lose " + (int)Timer;
		Timer -= Time.deltaTime;
	}
	//private methods
	private void DamagePlayer()
	{
		int count = 0;
		foreach(BaseChess chess in Manager.EnemyController.OnFieldChess)
		{
			if(!chess.IsDead)
			{
				count++;
			}
		}
		Manager.PlayerController.TakeDamage(count*2);
	}
}