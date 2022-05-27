using Godot;
using System;
using System.Collections.Generic;

public class FiniteStateMachine
{
	public class State
	{
		public Action<FiniteStateMachine> Begin { get; private set; }
		public Action<FiniteStateMachine> Execute { get; private set; }
		public Action<FiniteStateMachine> End { get; private set; }

		public State WithBegin(Action<FiniteStateMachine> begin)
		{
			Begin = begin;
			return this;
		}
		public State WithExecute(Action<FiniteStateMachine> execute)
		{
			Execute = execute;
			return this;
		}
		public State WithEnd(Action<FiniteStateMachine> end)
		{
			End = end;
			return this;
		}
	}

	public object Data { get; private set; }
	private Dictionary<string,State> states = new Dictionary<string, State>();
	private State currentState = null;

	public T DataAs<T>() where T:class
	{
		return Data as T;
	}

	public FiniteStateMachine ComposeState(string id, Func<State,State> composer)
	{
		states.Add(id, composer(new State()));
		return this;
	}

	public static FiniteStateMachine Create(object data)
	{
		return new FiniteStateMachine()
		{
			Data = data
		};
	}

	public void Execute()
	{
		if (currentState?.Execute != null)
			currentState.Execute(this);
	}

	public void Switch(string newState)
	{
		if (currentState?.End != null)
			currentState.End(this);
		
		currentState = states[newState];

		if (currentState?.Begin != null)
			currentState.Begin(this);
	}
	
}
