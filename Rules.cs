using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Rules
{
	public const char COMMENT_TOKEN = ';';
	public Dictionary<string,string> GeneralRules = new Dictionary<string, string>();
	public List<UnitFactory> UnitFactories = new List<UnitFactory>();
	public List<FactionFactory> FactionFactories = new List<FactionFactory>();
	private Factory currentFactory;
	private int currentIndex;
	private List<string> lines;
	private FiniteStateMachine machine;
	public void Load()
	{
		InitStates();
		var files = new List<string>()
		{
			"GENERAL","UNIT","FACTION","STRUCTURE"
		};
		machine.Switch("initial");

		foreach(var file in files)
			ParseFile(file);
	}

	private void ParseFile(string fileName)
	{
		lines = File.ReadAllLines($"rules/{fileName}")
			.Where(line => line.Trim().FirstOrDefault() != COMMENT_TOKEN)
			.Where(line => !string.IsNullOrWhiteSpace(line))
			.ToList();

		currentIndex = -1;

		if (lines.Count > 0)
			Advance();
	}

	private void InitStates()
	{
		machine = FiniteStateMachine
			.Create(this)
			.ComposeState("initial", state => state)
			.ComposeState("general", state => state
				.WithExecute(m => 
				{
					var (_,key,value) = ParsedLine();
					GeneralRules.Add(key, value);
					Advance();
				})
			)
			.ComposeState("unit", state => state
				.WithBegin(FactoryBegin<UnitFactory>())
				.WithExecute(FactoryExecute)
				.WithEnd(FactoryEnd<UnitFactory>(UnitFactories))
			)
			.ComposeState("faction", state => state
				.WithBegin(FactoryBegin<FactionFactory>())
				.WithExecute(FactoryExecute)
				.WithEnd(FactoryEnd<FactionFactory>(FactionFactories))
			)
			.ComposeState("structure", state => state)
		;
	}

	private Action<FiniteStateMachine> FactoryBegin<T>() where T:new()
	{
		return m => currentFactory = new T() as Factory;
	}
	private Action<FiniteStateMachine> FactoryEnd<T>(List<T> f) where T:Factory
	{
		return m => f.Add(currentFactory as T);
	}

	private void FactoryExecute(FiniteStateMachine m)
	{
		var (_,key,value) = ParsedLine();
		currentFactory.AddValue(key, value);
		Advance();
	}

	private (string,string,string) ParsedLine()
	{
		string raw = lines.ElementAtOrDefault(currentIndex).Trim();
		
		if (raw[0] == '#')
			return (raw.Substring(1).Trim(), null, null);

		string[] parts = raw.Split('=');
		return (null, parts[0].Trim(), parts[1].Trim());
	}

	private void Advance() 
	{  
		currentIndex++;

		var (newState, _, __) = ParsedLine();

		if (newState != null)
			machine.Switch(newState);

		if (currentIndex < lines.Count - 1)
			machine.Execute();
	}
}