using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    public int Width { get; private set; }
	public int Height { get; private set; }
	public Vector3 OriginalPos { get; private set; }
	public float CellSize { get; private set; }
	public T[,] GridArray { get; private set; }

	public Grid(Vector3 originalPos, Func<Grid<T>, int, int, T> createFunc, int width = 10, int height = 8, float cellSize = 1f)
	{
		OriginalPos = originalPos;
		Width = width;
		Height = height;
		CellSize = cellSize;

		GridArray = new T[Width, Height];
		for(int x = 0; x < GridArray.GetLength(0); x++)
		{
			for(int y = 0; y < GridArray.GetLength(1); y++)
			{
				GridArray[x,y] = createFunc(this,x,y);
				Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x+1,y),Color.white,1000f);
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 1000f);
			}
		}
		Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 1000f);
		Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width, height), Color.white, 1000f);
	}
	public Vector3 GetWorldPosition(int x, int y)
	{
		return OriginalPos + new Vector3(x, 0, y) * CellSize;
	}
	public Vector3 GetWorldPosition(Vector2Int vector2)
	{
		return OriginalPos + new Vector3(vector2.x, 0, vector2.y) * CellSize;
	}
	public Vector2Int WorldPositionToXY(Vector3 worldPosition)
	{
		Vector3 vector = worldPosition - OriginalPos;
		Vector2Int result = new Vector2Int();
		result.x = Mathf.FloorToInt(vector.x / CellSize);
		result.y = Mathf.FloorToInt(vector.z / CellSize);
		return result;
	}
	public Vector3 GetCellCenterPosition(int x, int y)
	{
		return GetWorldPosition(x, y) + new Vector3(CellSize / 2f, OriginalPos.y + 0.1f, CellSize / 2f);
	}
	public Vector3 GetCellCenterPosition(Vector2Int vector2)
	{
		return GetWorldPosition(vector2.x, vector2.y) + new Vector3(CellSize / 2f, OriginalPos.y + 0.5f, CellSize / 2f);
	}
	public void SetValue(Vector3 worldPosition, T value)
	{
		Vector2Int pos = WorldPositionToXY(worldPosition);
		SetValue(pos.x, pos.y, value);
	}
	public void SetValue(int x, int y, T value)
	{
		if (x < 0 || y < 0 || x > Width || y > Height) return;
		GridArray[x,y] = value;
	}
	public bool CheckInBounds(Vector2Int vector2)
	{
		if (vector2.x >= Width || vector2.y >= Height || vector2.x < 0 || vector2.y <0) 
			return false;
		return true;
	}

}
