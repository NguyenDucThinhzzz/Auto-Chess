using ResilientCore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameState
{
    InGame,
    Setup,
    Engage,
    Win,
    Lose,
	PlayerDeath,
	PlayerWin,
}
public class GameManager : StateMachine<EGameState>
{
	[field: SerializeField] public PlayerController PlayerController { get; private set; }
	[field: SerializeField] public EnemyAI EnemyController { get; private set; }
	[field: SerializeField] public PlayerChessDragging ChessDragging { get; private set; }

	[field: Header("UI Elements")]
	[field: SerializeField] public MessageTextHandler MessageText { get; private set; }
	[field: SerializeField] public TextMeshProUGUI TimerText { get; private set; }
	[field: SerializeField] public TextMeshProUGUI OnFieldChessCount { get; private set; }
	public Color DefaultTileColor;

	[field: Header("Setup State Values")]
	[field: SerializeField] public float WaitTime { get; private set; } = 10f;
	[field: Header("Engage State Values")]
	[field: SerializeField] public float WaitTimeAfterEngage { get; private set; } = 10f;
	[field: Header("Player Death State Values")]
	[field: SerializeField] public GameObject PlayerDeathPanel { get; private set; }
	[field: Header("Player Win State Values")]
	[field: SerializeField] public GameObject PlayerWinPanel { get; private set; }
	public bool Restart { get; set; } = false;
	public bool Continue { get; set; } = false;

	//Singleton
	public static GameManager Instance { get; private set; } = null;

	public void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		Instance = this;

		States.Add(EGameState.Setup, new SetupState(this));
		States.Add(EGameState.Engage, new EngageState(this));
		States.Add(EGameState.Win, new EngageWinState(this));
		States.Add(EGameState.Lose, new EngageLoseState(this));
		States.Add(EGameState.PlayerDeath, new PlayerDeathState(this));
		States.Add(EGameState.PlayerWin, new PlayerWinState(this));
		CurrentState = States[EGameState.Setup];
		TransitionToState(EGameState.Setup);
	}
	private void Start()
	{
		UpdateChessCountText();
		PlayerController.OnAddChessOnField.AddListener(UpdateChessCountText);
		PlayerController.OnRemoveChessOnField.AddListener(UpdateChessCountText);
	}
	private void OnDestroy()
	{
		PlayerController.OnAddChessOnField.RemoveListener(UpdateChessCountText);
		PlayerController.OnRemoveChessOnField.RemoveListener(UpdateChessCountText);
	}
	//Public Methods
	[ContextMenu("Reset")]
	public void ResetChesses()
	{
		foreach (var node in PlayerController.GameBoard.Grid.GridArray)
		{
			node.Chess = null;
		}
		PlayerController.ResetAllChess();
		EnemyController.ResetAllChess();
	}
	public void ChangeState(EGameState state)
	{
		TransitionToState(state);
	}
	public void BackMenu()
	{
		SceneManager.LoadScene("Menu", LoadSceneMode.Single);
	}
	public void UpdateChessCountText() => OnFieldChessCount.text = PlayerController.OnFieldChess.Count + "/" + PlayerController.MaxChess;

}
