using System;
using System.Collections.Generic;
using SystemEx;

namespace StateMachineEx
{
	public interface IState : IDisposable
	{
		string Name { get; }
		StateMachine StateMachine { get; }
		Transition Transition { set; }

		void Enter();
		void Exit();
		void Update();
	}

	public class State
	{
		StateMachine stateMachine = null;
		string name = null;

		public string Name { get { return name; } }
		public StateMachine StateMachine { get { return stateMachine; } set { stateMachine = value; } }
		public object[] Parameters { get; set; }
		public Transition Transition { set { stateMachine.Transition = value; } }		

		protected State()
		{
			var attr = this.GetType().GetAttribute<StateNameAttribute>();
			if (attr != null)
				name = attr.name;
			else
				name = this.GetType().Name;
		}
	}

	public class Transition
	{
		public string Name { get; protected set; }
		public object[] Parameters { get; protected set; }

		public Transition(string name)
		{
			Name = name;
		}
		
		public Transition Params(params object[] parameters)
		{
			Parameters = parameters;
			return this;
		}
	}

	public class StateMachine
	{
		public static readonly Transition DirectTransition = new Transition("<direct>");


		Dictionary<string, IState> states = new Dictionary<string, IState>();
		Dictionary<Tuple<string, string>, string> transitions = new Dictionary<Tuple<string,string>, string>();

		string defaultStateName = null;
		string currentStateName = null;
		string errorStateName = null;

		IState defaultState = null;
		IState currentState = null;
		IState errorState = null;

		public Transition Transition { get; set; }

		public void DeclareState<T>(T state) where T : IState
		{
			AddState(state);
			foreach (var tr in typeof(T).GetAttributes<TransitionAttribute>()) {
				transitions.Add(Tuple.Create(state.Name, tr.name), tr.stateName);
			}
		}

		public int AddState(IState state)
		{
			((State)state).StateMachine = this;
			states.Add(state.Name, state);
			return states.Count - 1;
		}

		public void SetDefaultState(string stateName)
		{
			defaultStateName = stateName;
		}

		public void SetErrorState(string stateName)
		{
			errorStateName = stateName;
		}

		public void Reset()
		{
			defaultState = states[defaultStateName];
			errorState = states[errorStateName];

			currentState = null;
			Transition = null;
		}

		public void Update()
		{
			if (currentState == null) {
				SelectState(defaultStateName);
				return;
			}

			if (Transition != null) {
				string nextStateName;
				if (Transition.Name == DirectTransition.Name) {
					nextStateName = (string)Transition.Parameters[0];
					Transition = DirectTransition.Params(Transition.Parameters.Skip(1));
				}
				else if (!transitions.TryGetValue(Tuple.Create(currentState.Name, Transition.Name), out nextStateName)) {
					SelectState(errorStateName);
				}

				try {
					SelectState(nextStateName);
				}
				catch {
					SelectState(errorStateName);
				}

				Transition = null;
			}
			else {
				currentState.Update();
			}
		}

		protected void SelectState(string name)
		{
			if (currentState != null)
				currentState.Exit();

			currentStateName = name;
			currentState = states[name];
			if (Transition != null)
				((State)currentState).Parameters = Transition.Parameters;
			currentState.Enter();
		}
	}

	public class StateNameAttribute : Attribute
	{
		public string name;

		public StateNameAttribute(string name)
		{
			this.name = name;
		}
	}

	public class TransitionAttribute : Attribute
	{
		public string name;
		public string stateName;

		public TransitionAttribute(string name, string stateName)
		{
			this.name = name;
			this.stateName = stateName;
		}
	}
}
