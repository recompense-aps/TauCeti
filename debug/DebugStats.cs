using Godot;
using System;

public class DebugStats : Control
{
	
	public override void _Ready()
	{
		
	}

	public override void _Process(float delta)
	{
		GetNode<Label>("HBoxContainer/FPSLabel").Text = Engine.GetFramesPerSecond().ToString();
	}
}
