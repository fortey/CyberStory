using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dialogue
{
	[NodeTint("#CCFFCC")]
	public class Chat : DialogueBaseNode
	{

		public CharacterInfo character;
		[TextArea] public string text;
#pragma warning disable CS0618 // Type or member is obsolete
		[Output(instancePortList = true)] [System.Obsolete] public List<Answer> answers = new List<Answer>();
#pragma warning restore CS0618 // Type or member is obsolete

		[System.Serializable]
		public class Answer
		{
			public string text;
			public int cost;
			//public AudioClip voiceClip;
			public Feature requirement;
		}

		[System.Obsolete]
		public void AnswerQuestion(int index)
		{
			NodePort port = null;
			if (answers.Count == 0)
			{
				port = GetOutputPort("output");
			}
			else
			{
				if (answers.Count <= index) return;
				port = GetOutputPort("answers " + index);
			}

			if (port == null) return;
			for (int i = 0; i < port.ConnectionCount; i++)
			{
				NodePort connection = port.GetConnection(i);
				(connection.node as DialogueBaseNode).Trigger();
			}
		}

		public override void Trigger()
		{
			(graph as DialogueGraph).current = this;
		}
	}
}