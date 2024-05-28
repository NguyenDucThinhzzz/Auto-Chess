using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessDetection : MonoBehaviour
{

	[field:SerializeField] public List<BaseChess> TargetChesses { get; private set; }
	public float SphereRadius { get; set; }
	private BaseChess BaseChess;
	//Unity Methods
	private void Awake()
	{
		TargetChesses = new List<BaseChess>();
		BaseChess = GetComponentInParent<BaseChess>();
	}
	public void GetTargetChesses()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, SphereRadius);
		foreach (Collider collider in colliders)
		{
			BaseChess chess = collider.GetComponent<BaseChess>();
			if (chess == null) continue;
			if (TargetChesses.Contains(chess) ||
				chess.CompareTag(BaseChess.tag) || 
				BaseChess.Board != chess.Board) continue;
			TargetChesses.Add(chess);
			chess.OnDeath.AddListener(() => { TargetChesses.Remove(chess); });
		}
	}
}
