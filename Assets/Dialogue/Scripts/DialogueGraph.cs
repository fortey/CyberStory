using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
//using XNodeEditor;

namespace Dialogue
{
	[CreateAssetMenu(menuName = "Dialogue/Graph", order = 0)]
	public class DialogueGraph : NodeGraph
	{
		[HideInInspector]
		public DialogueBaseNode current;

		public void Restart()
		{
			//Find the first DialogueNode without any inputs. This is the starting node.
			//current = nodes.Find(x => x is Chat && x.Inputs.All(y => !y.IsConnected)) as Chat;
			var startNode = nodes.FirstOrDefault(x => x is StartNode && x.Inputs.All(y => !y.IsConnected)) as StartNode;
			if (startNode != null)
				startNode.Trigger();
		}

		public Chat AnswerQuestion(int i)
		{
			var chat = current as Chat;
#pragma warning disable CS0612 // Type or member is obsolete
			chat.AnswerQuestion(i);
#pragma warning restore CS0612 // Type or member is obsolete
			//Story.instance.RefreshChat();
			return chat;
		}

		public void Continue()
		{
			if (current is Chat)
			{
				AnswerQuestion(0);
			}
			else if (current is GettingItem)
				(current as GettingItem).Continue();
		}
	}
}