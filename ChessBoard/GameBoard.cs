using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : Board
{
	public PlayerController Player;
	public EnemyAI EnemyAI;

	public bool IsBottomHalf(int y)
	{
		if (y <= (Grid.Height - 1) / 2) return true;
		return false;
	}
	private void LateUpdate()
	{
	}
}