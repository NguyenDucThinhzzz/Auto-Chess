using Assets.Scripts.ChessBoard;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
	[field: SerializeField] public TileController TilePrefab { get; private set; }
	[field: SerializeField] public Transform PhysicalTransform { get; private set; }
	[field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public int Width { get; private set; }
	[field: SerializeField] public float CellSize { get; private set; }
	public Grid<PathNode> Grid { get; private set; }
	public PathFinding PathFinding { get; private set; }

	public virtual void Awake()
	{
        Vector3 originalPos = transform.position - new Vector3(Width * CellSize/2f, 0, Height * CellSize/2f);
        Grid = new Grid<PathNode>(originalPos,(Grid<PathNode> grid,int x, int y) => {
			Vector3 cellPos = grid.GetCellCenterPosition(x, y);
			PathNode temp = new PathNode(grid, x, y);
			//Create and initialize tile sprite
			TileController a = Instantiate(TilePrefab, cellPos,TilePrefab.transform.rotation);
			a.transform.parent = transform;
			a.transform.localScale *= CellSize;
			temp.Tile = a;
			// Create Debug text
			temp.DebugTileText = Utility.Instance.CreateTextInWorld(Utility.Instance.gameObject.transform, x + " , " + y, cellPos, 80, Color.blue, 10);
			
			return temp;
		},Width, Height, CellSize);
        PhysicalTransform.localScale = new Vector3(Width * CellSize, .1f, Height * CellSize);
		PathFinding = new PathFinding(this);
	}


}
