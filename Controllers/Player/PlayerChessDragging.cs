using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChessDragging : MonoBehaviour
{
	[field: SerializeField] public LayerMask GroundLayerMask { get; private set; }
	[field: SerializeField] public float Offset { get; private set; } = 5f;
	public bool IsLeftMouseDown { get; private set; }
	public Color AccessibleTileColor;
	public Color InaccessibleTileColor;

	private PlayerController controller;
	private PlayerInput PlayerInput;
	private Camera mainCam;

	private BaseChess currentChess;
	#region Unity Methods
	private void Awake()
	{
		controller = GetComponent<PlayerController>();
		mainCam = Camera.main;
		PlayerInput = PlayerInput.Instance;
	}
	private void Update()
	{
		if (IsLeftMouseDown)
		{
			LeftMouse_performed();
		}
	}
	private void Start()
	{
		PlayerInput.Actions.LeftMouse.started += LeftMouse_started;
		PlayerInput.Actions.LeftMouse.canceled += LeftMouse_canceled;
	}
	private void OnDisable()
	{
		if(currentChess == null) return;
		ResetTileColor();
		currentChess.ResetPosition();
		currentChess = null;
	}
	private void OnDestroy()
	{
		PlayerInput.Actions.LeftMouse.started -= LeftMouse_started;
		PlayerInput.Actions.LeftMouse.canceled -= LeftMouse_canceled;
	}
	#endregion

	#region Private Methods
	private void LeftMouse_canceled(InputAction.CallbackContext obj)
	{
		if (GameManager.Instance.CurrentState.Key != EGameState.Setup)
			return;
		IsLeftMouseDown = false;
		if (currentChess == null) return;
		HandleChessPlacing();

		currentChess = null;
		ResetTileColor();
	}
	private void LeftMouse_started(InputAction.CallbackContext obj)
	{
		if (GameManager.Instance.CurrentState.Key != EGameState.Setup)
			return;
		IsLeftMouseDown = true;
		if (Physics.Raycast(mainCam.ScreenPointToRay(PlayerInput.MousePosition), out var hit, 100f, GroundLayerMask,QueryTriggerInteraction.Ignore))
		{
			//Check if it is a board
			Board board = hit.collider.GetComponentInParent<Board>();
			if (board == null) return;

			Vector2Int pos = board.Grid.WorldPositionToXY(hit.point);
			BaseChess chess = board.Grid.GridArray[pos.x, pos.y].Chess;
			if (chess == null) return;

			if (!chess.CompareTag("Player")) return;

			currentChess = chess;
			ChangeTileColorPlacing();
		}
	}
	private void LeftMouse_performed()
	{
		if (GameManager.Instance.CurrentState.Key != EGameState.Setup)
			return;
		if (currentChess == null) return;

		if (Physics.Raycast(mainCam.ScreenPointToRay(PlayerInput.MousePosition), out var hit, 100f, GroundLayerMask,QueryTriggerInteraction.Ignore))
		{
			currentChess.transform.position = hit.point + new Vector3(0, Offset, 0);
		}
	}
	private void HandleChessPlacing()
	{
		//Check if there is a hit point
		if (!Physics.Raycast(mainCam.ScreenPointToRay(PlayerInput.MousePosition), out var hit, 100f, GroundLayerMask,QueryTriggerInteraction.Ignore))
		{
			currentChess.ResetPosition();
			return;
		}

		Board board = hit.collider.GetComponentInParent<Board>();
		//Check if it is a board
		if (board == null)
		{
			currentChess.ResetPosition();
			return;
		}
		//Check if the board is of type GameBoard
		Vector2Int temp = board.Grid.WorldPositionToXY(hit.point);
		if (board.GetType() != typeof(GameBoard))
		{
			BaseChess chess = board.Grid.GridArray[temp.x, temp.y].Chess;
			if (chess == null)
			{
				currentChess.ChangeChessPosition(board, temp);
			}
			else
			{
				currentChess.SwapChess(chess);
				if (currentChess.Board != board)
					controller.AddOnFieldChess(chess);
			}
			controller.RemoveOnFieldChess(currentChess);
			return;
		}

		//Check if the current point is in the bottom half of the board
		GameBoard gameBoard = (GameBoard)board;
		if (!gameBoard.IsBottomHalf(temp.y))
		{
			currentChess.ResetPosition();
			return;
		}

		BaseChess targetChess = board.Grid.GridArray[temp.x, temp.y].Chess;
		if (targetChess == null)
		{
			if (controller.IsAtChessLimit && currentChess.Board != gameBoard)
			{
				currentChess.ResetPosition();
				return;
			}
			currentChess.ChangeChessPosition(board, temp);
			controller.AddOnFieldChess(currentChess);
			return;
		}

		currentChess.SwapChess(targetChess);
		if (targetChess.Board != board)
		{
			controller.RemoveOnFieldChess(targetChess);
		}
		controller.AddOnFieldChess(currentChess);
		return;
	}
	#endregion

	public void ChangeTileColorPlacing()
	{
		GameBoard temp = controller.GameBoard;
		foreach (PathNode node in temp.Grid.GridArray)
		{
			if (temp.IsBottomHalf(node.Position.y))
			{
				node.Tile.SpriteRenderer.color = AccessibleTileColor;
				continue;
			}
			node.Tile.SpriteRenderer.color = InaccessibleTileColor ;
		}
	}
	public void ResetTileColor()
	{
		GameBoard temp = controller.GameBoard;
		foreach (PathNode node in temp.Grid.GridArray)
		{
			node.Tile.SpriteRenderer.color = GameManager.Instance.DefaultTileColor;
		}
	}
}
