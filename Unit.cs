using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class Unit : Node2D
{
	public string unitId { get; set; }
	public FiniteStateMachine State{ get; set; }
	public bool MouseOver { get; private set; }
	public ModableStat Moves { get; private set; } = new ModableStat();
	public ModableStat Attack { get; private set; } = new ModableStat();
	public ModableStat Defense { get; private set; } = new ModableStat();
	public ModableStat Health { get; private set; } = new ModableStat();
	public bool Land { get; set; } = true;
	public bool Sea { get; set; } = false;
	public bool Air { get; set; } = false;

	public override void _Process(float delta)
	{
		WatchMouse();
		State.Execute();
	}

	private void WatchMouse()
	{
		var mousePosition = GetViewport().GetMousePosition();
		var tex = GetNode<Sprite>("Sprite").Texture;

		float threshHold = Math.Min(tex.GetWidth() / 4, tex.GetHeight() / 4);

		if (mousePosition.DistanceTo(Position) <= threshHold)
		{
			Modulate = new Color(1, 1, 1, 0.5f);
			MouseOver = true;
		}
			
		else
		{
			Modulate = new Color(1, 1, 1, 1);
			MouseOver = false;
		} 
	}

}