using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : BaseController
{
	[field: SerializeField] public AIWavesDataSO ChessData { get; private set; }
	public int CurrentWave { get; private set; } = 0;
	public override void Initialize()
	{
		base.Initialize();
		SpawnChess(ChessData.Waves[CurrentWave].AIChessDatas);
	}
	public void SpawnChess(List<AIChessData> ChessData)
	{
		DestroyChess();
		foreach (var data in ChessData)
		{
			SpawnChess(data);
		}
	}
	public void SpawnChess(AIChessData data)
    {
		if (GameBoard.IsBottomHalf(data.Position.y)) return;
		if (GameBoard.Grid.GridArray[data.Position.x, data.Position.y].Chess != null) return;
		BaseChess cur = Instantiate(data.Chess,transform);
		cur.tag = "Enemy AI";
		cur.ChangeChessPosition(GameBoard, data.Position);
		AddOnFieldChess(cur);
	}

	public void NextWave()
	{
		SpawnChess(ChessData.Waves[++CurrentWave % ChessData.Waves.Count].AIChessDatas);
	}
}
