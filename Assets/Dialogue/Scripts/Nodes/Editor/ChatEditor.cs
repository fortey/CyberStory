#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Dialogue
{
	[CustomNodeEditor(typeof(Chat))]
	public class ChatEditor : NodeEditor
	{

		public override void OnBodyGUI()
		{
			serializedObject.Update();

			Chat node = target as Chat;

			EditorGUILayout.PropertyField(serializedObject.FindProperty("character"), GUIContent.none);
#pragma warning disable CS0612 // Type or member is obsolete
			if (node.answers.Count == 0)
#pragma warning restore CS0612 // Type or member is obsolete
			{
				GUILayout.BeginHorizontal();
				NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
				NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
				GUILayout.EndHorizontal();
			}
			else
			{
				NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"));
			}
			GUILayout.Space(-30);

			NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"), GUIContent.none);
#pragma warning disable CS0618 // Type or member is obsolete
			NodeEditorGUILayout.InstancePortList("answers", typeof(DialogueBaseNode), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override);
#pragma warning restore CS0618 // Type or member is obsolete

			serializedObject.ApplyModifiedProperties();
		}

		public override int GetWidth()
		{
			return 300;
		}

		public override Color GetTint()
		{
			Chat node = target as Chat;
			if (node.character == null) return base.GetTint();
			else
			{
				Color col = node.character.color;
				col.a = 1;
				return col;
			}
		}
	}
}
#endif