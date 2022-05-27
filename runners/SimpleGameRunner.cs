using Godot;
using System.Linq;
public static partial class GameRunners
{
	/// <summary>
	/// Creates a simple game where the local user controls both players
	/// </summary>
	/// <param name="runner"></param>
	/// <returns></returns>
	public static GameRunner AsSimpleGame(this GameRunner runner)
	{
		var factions = new Faction[]
		{
			TauCeti.Instance.Rules.FactionFactories[0].Instance(),
			TauCeti.Instance.Rules.FactionFactories[1].Instance()
		};
		int currentPlayerIndex = 0;

		Unit spawn(string id, Faction player)
		{
			return TauCeti.Instance.Rules.UnitFactories.Single(x => x.UnitId == id).Instance(player.Color);
		}

		runner.State = FiniteStateMachine.Create(runner)
			.ComposeState("setup", state => state
				.WithBegin(m => 
				{
					TauCeti.Instance.Map.Generate(20);
					spawn("scout", factions[0]);
					spawn("scout", factions[1]);
				})
			)
			.ComposeState("turn-start", state => state
				// move units
				// ...
				// resolve conflicts
				// ...
				// bring all the units that have no move orders into idle
				// ...
			)
			.ComposeState("turn-end", state => state
				.WithBegin(m => 
				{
					currentPlayerIndex++;

					if (currentPlayerIndex == factions.Length)
						currentPlayerIndex = 0;
				})
			)
		;

		return runner;
	} 
}