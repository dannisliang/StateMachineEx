using UnityEditor;
using UnityEngine;

namespace StateMachineEx.UnityEditor
{
	[CustomPropertyDrawer(typeof(StateMachine))]
	public class PropertyDrawer_StateMachine : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			StateMachine obj = (StateMachine)fieldInfo.GetValue(property.serializedObject.targetObject);

			EditorGUILayout.Popup(0, new string[] { obj.State.Name });
		}
	}
}