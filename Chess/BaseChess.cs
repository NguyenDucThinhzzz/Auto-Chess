using Assets.Scripts.Chess;
using DG.Tweening;
using ResilientCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ColorClass
{
	Red,
	Green, 
	Blue,
	Yellow,
	All
}
public enum ShapeClass
{
	Cube,
	Sphere,
	Capsule,
	All
}
[RequireComponent(typeof(BaseStats))]
[RequireComponent(typeof(AudioSource))]
[Serializable]
public class BaseChess : Damagable, IDraggable
{
	[field:Header("Require Components")]
	[field: SerializeField] public ChessSO Data { get; private set; }
	[field: SerializeField] public GameObject Bullet { get; private set; }
	[field: SerializeField] public HealthBar HealthBar { get; private set; }
	[field: SerializeField] public GameObject RangeSphere { get; private set; }
	public ChessDetection ChessDetection { get; private set; }
	public BaseStats BaseStats { get; private set; }
	public AudioSource Audio { get; private set; }
	public PathNode PathNode { get; private set; }
	public Board Board { get; private set; }
	public Vector2Int OriginalPos { get; private set; }
	public List<PathNode> route { get; private set; } = new List<PathNode>();
	private GameObject bulletPrefab = null;
	private bool isSelected = false;
	private bool isMoving = false;
	//Testing Values

	//Unity Methods
	protected override void Awake()
	{
		base.Awake();
		BaseStats = GetComponent<BaseStats>();
		Audio = GetComponent<AudioSource>();
		ChessDetection = GetComponentInChildren<ChessDetection>();

		BaseStats.LoadValues(Data.StatData);
		BaseStats.Range.OnChangeValue.AddListener(ChangeATKRange);

		OnDamaged.AddListener(ChangeHealth);
	}
	private void Start()
	{
		HP = BaseStats.Health.Value;
		HealthBar.SetMaxHealth(BaseStats.Health.Value);
		ChangeATKRange();
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		BaseStats.Range.OnChangeValue.RemoveListener(ChangeATKRange);
	}
	//Interface
	public void OnBeginDrag()
	{
		isSelected = true;
	}

	public void OnEndDrag()
	{
		isSelected = false;
	}

	//Public Methods
	public void ChangeATKRange()
	{
		ChessDetection.SphereRadius = BaseStats.Range.Value;
	}
	public void ChangeHealth()
	{
		HealthBar.ChangeValue(HP);
	}
	public void SwapChess(BaseChess chess)
	{
		Board board = chess.Board;
		Vector2Int pos = chess.PathNode.Position;
		chess.ChangeChessPosition(Board, PathNode.Position);

		PathNode = board.Grid.GridArray[pos.x, pos.y];
		PathNode.Chess = this;
		transform.position = board.Grid.GetCellCenterPosition(pos.x, pos.y);
		Board = board;
		OriginalPos = pos;
	}
	public void ChangeChessPosition(Board board,Vector3 pos)
	{
		if(PathNode != null)
			PathNode.Chess = null;
		Vector2Int temp = board.Grid.WorldPositionToXY(pos);
		PathNode = board.Grid.GridArray[temp.x,temp.y];
		PathNode.Chess = this;
		transform.position = board.Grid.GetCellCenterPosition(temp.x, temp.y);
		Board = board;
		OriginalPos = temp;
	}
	public void ChangeChessPosition(Board board, Vector2Int pos)
	{
		if (PathNode != null)
			PathNode.Chess = null;
		PathNode = board.Grid.GridArray[pos.x, pos.y];
		PathNode.Chess = this;
		transform.position = board.Grid.GetCellCenterPosition(pos.x, pos.y);
		Board = board;
		OriginalPos = pos;
	}
	public void ResetPosition()
	{
		PathNode = Board.Grid.GridArray[OriginalPos.x, OriginalPos.y];
		PathNode.Chess = this;
		transform.position = Board.Grid.GetCellCenterPosition(OriginalPos.x, OriginalPos.y);
	}
	public void ResetChess()
	{
		gameObject.SetActive(true);
		transform.DOKill();
		isMoving = false;
		ResetPosition();
		route.Clear();
		IsDead = false;
		HealthBar.SetMaxHealth(BaseStats.Health.Value);
		HP = BaseStats.Health.Value;
	}

	public void ChessBehaviour()
	{
		if (isSelected || isMoving || IsDead)
			return;
		//Attack
		ChessDetection.GetTargetChesses();
		if (ChessDetection.TargetChesses.Count != 0)
		{
			route.Clear();
			Attack(ChessDetection.TargetChesses.First());
			return;
		}
		//Destroy bullet when there is no enemy
		DestroyBullet();
		//Movement
		MoveChess();
	}
	public override void TakeDamage(DamageInfo damageInfo)
	{
		if (IsDead) return;
		HP -= Mathf.Clamp(damageInfo.Amount - BaseStats.Defence.Value,0f,999999f);
		OnDamaged.Invoke();
		if (HP <= 0)
		{
			HP = 0;
			IsDead = true;
			OnDeath?.Invoke();
		}
	}
	public override void Death()
	{
		base.Death();
		PathNode.Chess = null;
		DestroyBullet();
		ChessDetection.TargetChesses.Clear();
	}
	//private methods
	private void DestroyBullet()
	{
		if (bulletPrefab != null)
		{
			bulletPrefab.transform.DOKill();
			Destroy(bulletPrefab);
		}
	}

	private void Attack(BaseChess enemy)
	{
		if(bulletPrefab == null)
		{
			bulletPrefab = Instantiate(Bullet, transform);
			bulletPrefab.transform.position = transform.position;
			PlayShootingAudio();
			bulletPrefab.transform.DOMove(enemy.transform.position, 1f/BaseStats.AttackSpeed.Value).OnComplete(() =>
			{
				Destroy(bulletPrefab);
				enemy.TakeDamage(new DamageInfo(this, BaseStats.Attack.Value));
			});
			return;
		}
	}
	private void MoveChess()
	{
		if (isMoving) return;
		//Get Path
		BaseChess enemy = GetClosestChess();
		if (enemy == null)
			return;
		route = Board.PathFinding.FindPathAStar(PathNode.Position, enemy.PathNode.Position);
		if (route.Count == 0) return;
		//Move Chess
		PathNode target = route.First();
		route.Remove(route.First());

		if(target.Chess != null)
		{
			route.Clear();
			return;
		}
		MoveChessPosition(target);
	}
	private void MoveChessPosition(PathNode target)
	{
		isMoving = true;
		if (PathNode != null)
			PathNode.Chess = null;
		PathNode = PathNode.CurrentGrid.GridArray[target.Position.x, target.Position.y];
		PathNode.Chess = this;
		transform.DOMove(target.CurrentGrid.GetCellCenterPosition(target.Position), 1f/BaseStats.Speed.Value).OnComplete(() => {
			isMoving = false;
		});
	}
	private BaseChess GetClosestChess()
	{
		Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position,100f,LayerMask.GetMask("Chess"),QueryTriggerInteraction.Ignore);
		if (colliders.Length == 0) return null;

		float min = float.MaxValue;
		BaseChess closestChess = null;
		foreach (var collider in colliders)
		{
			BaseChess chess = collider.GetComponent<BaseChess>();
			if (this.tag.Equals(chess.tag)|| this.Board != chess.Board) continue;

			float dist = (transform.position - chess.transform.position).magnitude;
			if (min > dist)
			{
				min = dist;
				closestChess = chess;
			}
		}
		return closestChess;
	}
	public void PlayShootingAudio()
	{
		Audio.clip = Data.Shooting;
		Audio.Play();
	}
	[ContextMenu("Test Damage")]
	public void DamageCap()
	{
		TakeDamage(new DamageInfo(this, 10f));
	}

}
