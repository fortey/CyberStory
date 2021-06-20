using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dialogue
{
	[NodeTint("#CCFF70")]
	public class GettingItem : DialogueBaseNode
	{
		public Item item;

		public override void Trigger()
		{
			(graph as DialogueGraph).current = this;
			Story.instance.ShowGettingItem(item);
		}

		public void Continue()
		{
			NodePort port = GetOutputPort("output");

			if (port == null) return;
			for (int i = 0; i < port.ConnectionCount; i++)
			{
				NodePort connection = port.GetConnection(i);
				(connection.node as DialogueBaseNode).Trigger();
			}
		}
	}
}
