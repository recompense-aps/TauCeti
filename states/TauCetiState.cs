using Godot;
using System;

public static class TauCetiState
{
	public static FiniteStateMachine ComposeStates(this TauCeti ceti)
	{
		return FiniteStateMachine
			.Create(ceti)
			.ComposeState("init", state => state
				.WithBegin(m => 
					m.DataAs<TauCeti>()
						.LoadAssets()
						.ActivateInitialScene()
				)
			)
			.ComposeState("main-menu", state => state
				.WithBegin(machine => {

				})
				.WithExecute(machine => {

				})
				.WithEnd(machine => {

				})
			);
	}
}
