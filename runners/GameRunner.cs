using Godot;
public class GameRunner : Node
{
	public FiniteStateMachine State { get; set; }
	public override void _Process(float delta)
	{
		State?.Execute();
	}
}