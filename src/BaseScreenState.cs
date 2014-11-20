using UnityEngine;
using UnityEngineEx;
using UnityInput;

namespace Code.GameStates
{
	public class BaseScreenState : State
	{
		protected GameObject screen;

		public BaseScreenState(GameObject screen)
		{
			this.screen = screen;
		}

		public virtual void Exit()
		{
			screen.transform.Clear();
			InputManager.instance.Clear();
		}
	}
}