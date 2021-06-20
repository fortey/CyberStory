#if UNITY_EDITOR
using Dialogue;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DialogueEditor
{
	[CustomNodeEditor(typeof(Dialogue.StartNode))]
	public class StartEditor : NodeEditor
	{

		public override void OnBodyGUI()
		{
			serializedObject.Update();

			Dialogue.StartNode node = target as Dialogue.StartNode;
			NodeEditorGUILayout.PortField(target.GetInputPort("input"), GUILayout.Width(100));
			NodeEditorGUILayout.PortField(target.GetOutputPort("output"));
			NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("characters"));
			EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();
		}

		public override int GetWidth()
		{
			return 336;
		}
	}
}
#endif