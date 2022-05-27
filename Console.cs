using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Godot;

public static class Console
{
	public static object ProcessCommand(string command)
	{
		var (commandId, args) = ProcessLine(command);

		switch(commandId)
		{
			case "unit":
				return UnitCommand(args);
			case "tile":
				return TileCommand(args);
			case "scn":
				return ScenarioCommand(args);
			default:
				GD.Print($"{commandId} is not a valid command");
				return null;
		}
	}

	private static (string commandId, List<string> args) ProcessLine(string command)
	{
		return (
			command.Split(' ')[0].Substring(1)
			,
			command.Split(' ').Skip(1).ToList()
		);
	}

	private static Unit UnitCommand(List<string> args)
	{
		var factory = TauCeti.Instance.Rules.UnitFactories.Single(f => f.UnitId == args[0]);
		var x = int.Parse(args[1]);
		var y = int.Parse(args[2]);

		var unit = factory.Instance();
		TauCeti.Instance.AddChild(unit);

		unit.Position = new Vector2(TauCeti.Instance.Map.GetCellByGridPosition(x, y).CenterPosition);
		unit.State = unit.ComposeBasicTestStates();
		unit.State.Switch("spawning");
		return unit;
	}

	private static object TileCommand(List<string> args)
	{
		Map.Environment environment = Map.Environment.Sand;
		Enum.TryParse(args[0], out environment);

		var x = int.Parse(args[1]);
		var y = int.Parse(args[2]);

		TauCeti.Instance.Map.SetEnvironmentAt(x, y, environment);
		return null;
	}

	private static object ScenarioCommand(List<string> args)
	{
		string path = args[0];

		var lines = System.IO.File.ReadAllLines(path);

		foreach(var line in lines)
			ProcessCommand(line.Trim());

		return null;
	}
}