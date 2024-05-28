using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : BaseController
{
	[field: SerializeField] public WaitingBoard WaitingBoard { get; private set; }
	[field: SerializeField] public BuffUIManager BuffUIManager { get; private set; }
	[field: SerializeField] public HealthBar HealthBar { get; private set; }
	[field: SerializeField] public Color HealthColor { get; private set; } = Color.green;

	[field: Header("Player Stats")]
	[field: SerializeField] public float MaxHP { get; private set; } = 10;
	[field: SerializeField] public float HP { get; set; }
	[field: SerializeField] public int MaxChess { get; set; } = 3;
	[field: SerializeField] public int GiveAmount { get; set; } = 3;
	public List<ChessRate> ChessRates;
	public bool IsAtChessLimit { 
		get
		{
			return OnFieldChess.Count >= MaxChess;
		}
	}

	//Override methods
	public override void Initialize()
	{
		base.Initialize();
		HP = MaxHP;
		HealthBar.SetMaxHealth(MaxHP);
		GiveRandomChessAmount(GiveAmount);
	}
	//Public Methods
	public void TakeDamage(float damage)
	{
		HP = Mathf.Clamp(HP - damage, 0, MaxHP);
		HealthBar.ChangeValue(HP);
	}

	public void Death()
	{
		GameManager.Instance.ChangeState(EGameState.PlayerDeath);
	}
	public void GiveChess(BaseChess data)
	{
		BaseChess cur = Instantiate(data, transform);
		cur.tag = "Player";
		cur.HealthBar.HealthImage.color = HealthColor;
		WaitingBoard.PlaceChess(cur);
	}
	public void GiveRandomChessAmount(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GiveChess(ChoseRandomChess());
		}
	}
	public BaseChess ChoseRandomChess()
	{
		int current = 0;
		int target = UnityEngine.Random.Range(1, CalculateSum());

		foreach(var chess in ChessRates)
		{
			current += chess.Rate;
			if (target <= current)
			{
				return chess.BaseChess;
			}
		}
		return null;
	}
	public int CalculateSum()
	{
		int sum = 0;
		foreach(var chess in ChessRates)
		{
			sum+= chess.Rate;
		}
		return sum;
	}
	protected override void ColorBuffs(ColorClass color, int value)
	{
		if (value == 0)
		{
			BuffUIManager.RemovePanel(color.ToString());
			return;
		}
		RemoveColorBuff(color);
		//Choose Buff
		colorBuffs[color] = ChessBuffs.GetBuff(color.ToString()).GetBuffTarget(value);

		if (colorBuffs[color] is null)
		{
			BuffUIManager.RemovePanel(color.ToString());
			return;
		}
		BuffUIManager.AddPanel(color.ToString(), ChessBuffs.GetBuff(color.ToString()), value);
		AddColorBuff(color, colorBuffs[color]);
	}
	protected override void ShapeBuffs(ShapeClass shape, int value)
	{
		if (value == 0)
		{
			BuffUIManager.RemovePanel(shape.ToString());
			return;
		}
		RemoveShapeBuff(shape);
		//Choose Buff
		shapeBuffs[shape] = ChessBuffs.GetBuff(shape.ToString()).GetBuffTarget(value);

		if (shapeBuffs[shape] is null)
		{
			BuffUIManager.RemovePanel(shape.ToString());
			return;
		}
		BuffUIManager.AddPanel(shape.ToString(), ChessBuffs.GetBuff(shape.ToString()), value);
		AddShapeBuff(shape, shapeBuffs[shape]);
	}
}
