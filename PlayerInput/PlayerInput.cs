using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : Singleton<PlayerInput>
{
    public PlayerActions PlayerActions { get; private set; }
    public PlayerActions.MainGameActions Actions { get; private set; }
	public Vector2 MousePosition { get { return Actions.MousePos.ReadValue<Vector2>(); } }

	public override void Awake()
	{
		base.Awake();
		PlayerActions = new PlayerActions();
		PlayerActions.Enable();
		Actions = PlayerActions.MainGame;
	}
	private void OnEnable()
	{
		PlayerActions.Enable();
	}
	private void OnDisable()
	{
		PlayerActions.Disable();
	}
}
