using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChessHandling : MonoBehaviour
{
	[field:Header("Require Components")]
	[field: SerializeField] public LayerMask GroundLayerMask { get; private set; }
	[field: SerializeField] public ChessInfoUI ChessInfoUI { get; private set; }
	[field: SerializeField] public GameObject SellEffect {  get; private set; }
	public BaseChess currentChess { get; private set; } = null;

	private PlayerController controller;
	private Camera mainCam;
	private PlayerInput PlayerInput;
	private void Awake()
	{
		controller = GetComponent<PlayerController>();
		mainCam = Camera.main;
		PlayerInput = PlayerInput.Instance;
	}
	private void Start()
	{
		PlayerInput.Actions.LeftMouse.started += LeftMouse_started;
		PlayerInput.Actions.DeleteChess.started += DeleteChess_started;
	}

	private void DeleteChess_started(InputAction.CallbackContext obj)
	{
		if (Physics.Raycast(mainCam.ScreenPointToRay(PlayerInput.MousePosition), out var hit, 100f, GroundLayerMask, QueryTriggerInteraction.Ignore))
		{

			//Check if it is a board
			Board board = hit.collider.GetComponentInParent<Board>();
			if (board == null)
			{
				HideChessInfo();
				return;
			}

			Vector2Int pos = board.Grid.WorldPositionToXY(hit.point);
			BaseChess chess = board.Grid.GridArray[pos.x, pos.y].Chess;

			if (chess == null || chess.tag != "Player") return;

			if(GameManager.Instance.CurrentState.Key is not EGameState.Setup)
			{
				if (board.GetType() == typeof(GameBoard)) return;
				DeleteChess(chess);
				return;
			}

			if (board.GetType() == typeof(GameBoard))
			{
				controller.RemoveOnFieldChess(chess);
			}

			DeleteChess(chess);
			return;
		}
	}
	private void DeleteChess(BaseChess chess)
	{
		chess.PathNode.Chess = null;
		Destroy(chess.gameObject);
	}

	private void OnDestroy()
	{
		PlayerInput.Actions.LeftMouse.started -= LeftMouse_started;
	}
	private void LeftMouse_started(InputAction.CallbackContext context)
	{
		if (Physics.Raycast(mainCam.ScreenPointToRay(PlayerInput.MousePosition), out var hit, 100f, GroundLayerMask, QueryTriggerInteraction.Ignore))
		{

			//Check if it is a board
			Board board = hit.collider.GetComponentInParent<Board>();
			if (board == null)
			{
				HideChessInfo();
				return;
			}

			Vector2Int pos = board.Grid.WorldPositionToXY(hit.point);
			BaseChess chess = board.Grid.GridArray[pos.x, pos.y].Chess;

			if (chess == null)
			{
				HideChessInfo();
				return;
			}
			ShowChessInfo(chess);
			return;
		}
		HideChessInfo();
	}

	public void ShowChessInfo(BaseChess chess)
	{

		//check if there is a selected chess
		if (currentChess != null)
		{
			currentChess.RangeSphere.SetActive(false);
			currentChess = null;
		}
		//Set range sphere
		currentChess = chess;
		currentChess.RangeSphere.SetActive(true);
		float curRange = chess.BaseStats.Range.Value;
		currentChess.RangeSphere.transform.localScale = new Vector3(curRange*2,curRange * 2, curRange * 2);
		//Set Info panel active 
		ChessInfoUI.gameObject.SetActive(true);
		ChessInfoUI.UpdateInfo(chess);
	}
	public void HideChessInfo()
	{
		if(currentChess!= null)
		{
			currentChess.RangeSphere.SetActive(false);
			currentChess = null;
		}

		ChessInfoUI.gameObject.SetActive(false);
	}
}
