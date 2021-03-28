using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dialogue
{
    [NodeTint("#CCFFF0")]
    public class StartNode : DialogueBaseNode
    {


        public override void Trigger()
        {
            NodePort port = null;
            port = GetOutputPort("output");

            if (port == null) return;
            for (int i = 0; i < port.ConnectionCount; i++)
            {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }
    }
}
