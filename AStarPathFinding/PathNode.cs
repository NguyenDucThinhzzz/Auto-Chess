using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
	public BaseChess Chess { get; set; }
	public Grid<PathNode> CurrentGrid { get; private set; }
	public Vector2Int Position { get; private set; }
	public TileController Tile { get; set; }
	public TextMesh DebugTileText { get; set; }
	public int G { get; set; }
    public int H { get; set; }
	public int F { get; set; }
    public PathNode LastNode { get; set; }
	public PathNode(Grid<PathNode> currentGrid, int x, int y)
	{
		CurrentGrid = currentGrid;
		Position = new Vector2Int(x, y);
	}
	public void CalculateF()
	{
		F = G + H;
	}
}
