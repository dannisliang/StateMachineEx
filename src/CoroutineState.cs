using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachineEx
{
	public class CoroutineState : State, IState
	{
		Func<IEnumerable> func;
		IEnumerator curentEn;

		public CoroutineState(string name, Func<IEnumerable> func)
			: base(name)
		{
			this.func = func;
		}

		public void Enter()
		{
			curentEn = func().GetEnumerator();
		}

		public void Exit()
		{
			curentEn = null;
		}

		public void Update()
		{
			if (curentEn.MoveNext()) {
				object o = curentEn.Current;
			}
			else {
				curentEn = func().GetEnumerator();
				curentEn.MoveNext();
			}
		}

		public void Dispose()
		{
			curentEn = null;
		}
	}
}
