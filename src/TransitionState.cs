using System;

namespace StateMachineEx
{
	public class TransitionState : State, IState
	{
		public Action enterFunc;
		public Action exitFunc;

		public TransitionState(string name)
			: base(name)
		{
			this.enterFunc = () => {};
			this.exitFunc = () => { };
		}

		public TransitionState(string name, Action enterFunc)
			: base(name)
		{
			this.enterFunc = enterFunc;
			this.exitFunc = () => { };
		}

		public TransitionState(string name, Action enterFunc, Action exitFunc)
			: base(name)
		{
			this.enterFunc = enterFunc;
			this.exitFunc = exitFunc;
		}

		public void Enter()
		{
			enterFunc();
		}

		public void Exit()
		{
			exitFunc();
		}

		public void Update()
		{
		}

		public void Dispose()
		{
		}
	}
}