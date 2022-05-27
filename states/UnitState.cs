using Godot;
using System.Linq;
using System.Collections.Generic;
public static class UnitState
{
	public static FiniteStateMachine ComposeBasicTestStates(this Unit unit)
	{
		BFS<MapCell> search = null;
		var line = TauCeti.Instance.GetNode<Line2D>("Line");

		return FiniteStateMachine
			.Create(unit)
			.ComposeState("initial", state => state)
			.ComposeState("spawning", state => state
				.WithBegin(async m => 
				{
					var t = new Tween();
					t.InterpolateProperty(unit, "modulate", new Color(1, 1, 1, 0), Colors.White, 1);
					unit.AddChild(t);
					t.Start();

					await t.ToSignal(t, "tween_completed");
					t.QueueFree();
					m.Switch("idle");
				})
			)
			.ComposeState("idle", state => state
				.WithBegin(m => 
				{

				})
				.WithExecute(m => 
				{
					if (unit.MouseOver && Input.IsActionJustPressed("mouse_left"))
						m.Switch("selected");
				})
			)
			.ComposeState("selected", state => state
				.WithBegin(m => 
				{
					var map = TauCeti.Instance.Map;
					var start = map.GetCellByWorldPosition(unit.Position.x, unit.Position.y);
					search = BFS<MapCell>
						.Create()
						.WithNeighbors(c => 
						{
							int x = (int)c.GridPosition.x;
							int y = (int)c.GridPosition.y;

							return new List<MapCell>()
							{
								map.GetCellByGridPosition(x - 1, y),
								map.GetCellByGridPosition(x + 1, y),
								map.GetCellByGridPosition(x, y + 1),
								map.GetCellByGridPosition(x, y - 1)
							}.Where(z => z != null).ToList();
						})
						.WithCost(c => {
							switch(c.Environment)
							{
								case Map.Environment.Sand:
									return 1;
								case Map.Environment.Rock:
									return 3;
								default:
									return 1;
							}
						})
						.WithPassable(c => c.Environment != Map.Environment.Water)
						.Search(start);
				})
				.WithExecute(async m => 
				{
					var mousePos = unit.GetViewport().GetMousePosition();
					var toPos = TauCeti.Instance.Map.GetCellByWorldPosition(mousePos.x, mousePos.y);
					var path = search.PathTo(toPos);

					line.Points = path.Select(c => c.CenterPosition).ToArray();

					if (Input.IsActionJustPressed("mouse_left"))
					{
						foreach(var p in path.Select(c => c.CenterPosition))
						{
							var t = new Tween();
							t.InterpolateProperty(unit, "position", unit.Position, p, 0.2f, Tween.TransitionType.Cubic);
							unit.AddChild(t);
							t.Start();

							await t.ToSignal(t, "tween_completed");
							t.QueueFree();
							m.Switch("idle");
						}
					}
				})
				.WithEnd(m => 
				{
					search = null;
					line.Points = null;
				})
			)
			.ComposeState("moving", state => state)
			.ComposeState("attacking", state => state)
			.ComposeState("dying", state => state)
		;
	}
}