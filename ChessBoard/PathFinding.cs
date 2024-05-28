using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ChessBoard
{
	public class PathFinding
	{
		private Board Board;
		private List<PathNode> list = new List<PathNode>();
		private List<PathNode> visited = new List<PathNode>();
		private List<Vector2Int> dirOffset = new List<Vector2Int>() {
		new Vector2Int(0,1),
		new Vector2Int(1,0),
		new Vector2Int(0,-1),
		new Vector2Int(-1,0),
		new Vector2Int(1,1),
		new Vector2Int(1,-1),
		new Vector2Int(-1,1),
		new Vector2Int(-1,-1),
	};
		public PathFinding(Board board)
		{
			Board = board;
		}

		//Find Path using BFS
		public List<PathNode> FindPath(Vector2Int start, Vector2Int end)
		{
			ResetLastNode();
			if (start == end) return new List<PathNode>();
			PathNode startNode = Board.Grid.GridArray[start.x, start.y];

			list.Clear();
			visited.Clear();
			list.Add(startNode);
			while (list.Count != 0)
			{
				PathNode cur = list.First();
				list.Remove(list.First());
				if (cur.Position == end)
				{
					return CalculatePath(cur);
				}
				visited.Add(cur);

				foreach (var offset in dirOffset)
				{
					Vector2Int target = cur.Position + offset;
					if (!Board.Grid.CheckInBounds(target))
					{
						continue;
					}

					PathNode next = Board.Grid.GridArray[target.x, target.y];
					if (!visited.Contains(next) && !list.Contains(next) && (next.Position == end || next.Chess == null))
					{
						next.LastNode = cur;
						list.Add(next);
					}
				}
			}
			return new List<PathNode>();
		}
		//Find Path using Astar
		public List<PathNode> FindPathAStar(Vector2Int start, Vector2Int end)
		{
			ResetLastNode();
			if (start == end) return new List<PathNode>();
			PathNode startNode = Board.Grid.GridArray[start.x, start.y];

			list.Clear();
			visited.Clear();
			list.Add(startNode);
			while (list.Count != 0)
			{
				PathNode cur = list.First();
				list.Remove(list.First());

				if (cur.Position == end)
				{
					return CalculatePath(cur);
				}
				visited.Add(cur);

				foreach (var offset in dirOffset)
				{
					Vector2Int target = cur.Position + offset;
					if (!Board.Grid.CheckInBounds(target))
					{
						continue;
					}

					PathNode next = Board.Grid.GridArray[target.x, target.y];
					if (!visited.Contains(next) && !list.Contains(next) && (next.Position == end || next.Chess == null))
					{
						next.G = cur.G + 1;
						next.H = CalculateHCost(target,end);
						next.CalculateF();
						next.LastNode = cur;
						list.Add(next);
					}
				}
				list.Sort((a,b) => a.F.CompareTo(b.F));
			}
			return new List<PathNode>();
		}
		public List<PathNode> CalculatePath(PathNode endNode)
		{
			List<PathNode> nodes = new List<PathNode>();
			PathNode last = endNode.LastNode;
			while (last.LastNode != null)
			{
				nodes.Add(last);
				last = last.LastNode;
			}
			
			nodes.Reverse();
			return nodes;
		}

		public void PrintPath(List<PathNode> results)
		{
			if (results == null) return;
			for (int i = 0; i < results.Count - 1; i++)
			{
				Vector3 f = Board.Grid.GetCellCenterPosition(results[i].Position);
				Vector3 s = Board.Grid.GetCellCenterPosition(results[i + 1].Position);
				Debug.DrawLine(f, s, Color.red, 1f);
			}
		}
		public int CalculateHCost(Vector2Int cur, Vector2Int end)
		{
			Vector2Int temp = end - cur;
			return (Mathf.Abs(temp.x)+Mathf.Abs(temp.y));
		}
		public void ResetLastNode()
		{
			foreach (var node in Board.Grid.GridArray)
			{
				node.LastNode = null;
				node.G = 0;
				node.H = 0;
			}
		}
	}
}