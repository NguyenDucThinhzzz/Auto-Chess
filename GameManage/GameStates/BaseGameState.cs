using ResilientCore;

public class BaseGameState : BaseState<EGameState>
{
	public GameManager Manager { get; private set; }
	public BaseGameState(EGameState state, GameManager manager) : base(state)
	{
		Manager = manager;
	}
	public override void Enter()
	{

	}

	public override void Exit()
	{
	}

	public override void FixedUpdate()
	{
	}

	public override EGameState GetNextState()
	{
		return Key;
	}

	public override void Update()
	{
	}

}