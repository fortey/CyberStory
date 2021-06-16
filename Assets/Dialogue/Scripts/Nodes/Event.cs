using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using XNode;
namespace Dialogue
{
	[NodeTint("#FFFFAA")]
	public class Event : DialogueBaseNode
	{

		public SerializableEvent[] trigger; // Could use UnityEvent here, but UnityEvent has a bug that prevents it from serializing correctly on custom EditorWindows. So i implemented my own.
		public float delay;

		public override void Trigger()
		{

			for (int i = 0; i < trigger.Length; i++)
			{
				trigger[i].Invoke();
			}

			Story.ContinueWithDelay(Continue);
		}

		public IEnumerator Continue()
		{
			yield return new WaitForSeconds(delay);

			NodePort port = GetOutputPort("output");
			if (port != null)
			{
				for (int i = 0; i < port.ConnectionCount; i++)
				{
					NodePort connection = port.GetConnection(i);
					(connection.node as DialogueBaseNode).Trigger();
				}
			}
			Story.instance.RefreshChat();
		}
	}
}