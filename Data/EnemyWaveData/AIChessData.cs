using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIChessData
{
    [field: SerializeField] public BaseChess Chess { get; private set; }
	[field: SerializeField] public Vector2Int Position { get; private set; }
}
