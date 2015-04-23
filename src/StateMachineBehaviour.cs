using System;
using System.Collections.Generic;
using System.Reflection;
using SystemEx;
using UnityEngine;
using UnityEngineEx;
using UnityEngineEx.Attributes;

namespace StateMachineEx
{
	public class StateMachineBehaviour : MonoBehaviour, IConstructable
	{
		private class ContextFieldLink
		{
			public void LinkField(FieldInfo context, FieldInfo state)
			{
			}
		}

		private StateMachine stateMachine;

		[SerializeField] private TextAsset stateMachineScript;
		[SerializeField] private StateMachineContextBehaviour stateMachineContext;

		private Dictionary<FieldInfo, bool> stateMachineContextFields = new Dictionary<FieldInfo, bool>();
		private bool currentUsed = true;
		private Dictionary<IState, ContextFieldLink> stateMachineContextLinks = new Dictionary<IState, ContextFieldLink>(LambdaComparer.Create((IState a, IState b) => { return a.Name.CompareTo(b.Name); }));

		public void Constructor(params object[] args)
		{
			// stateMachine =
		}

		private void Awake()
		{
			foreach (var contextField in stateMachineContext.GetType().GetFields<ContexFieldAttribute>()) {
				stateMachineContextFields.Add(contextField, false);
			}

			stateMachine.Load(stateMachineScript.text);

			foreach (var state in stateMachine.States) {
				stateMachineContextLinks.Add(state, InitState((State)state));
			}
		}

		private ContextFieldLink InitState(State state)
		{
			ContextFieldLink link = new ContextFieldLink();

			foreach (var contextField in state.GetType().GetFields<ContexFieldAttribute>()) {
				bool used;
				if (!stateMachineContextFields.TryGetValue(contextField, out used))
					throw new Exception("field not in context {0}".format(contextField));
				if (used == currentUsed)
					throw new Exception("field is used {0}".format(contextField));

				//link.LinkField()
			}

			return link;
		}
	}
}