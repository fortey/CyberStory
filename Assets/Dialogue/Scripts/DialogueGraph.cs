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
        public Chat current;

        public void Restart()
        {
            //Find the first DialogueNode without any inputs. This is the starting node.
            //current = nodes.Find(x => x is Chat && x.Inputs.All(y => !y.IsConnected)) as Chat;
            var startNode = nodes.FirstOrDefault(x => x is StartNode) as StartNode;
            if (startNode != null)
                startNode.Trigger();
        }

        public Chat AnswerQuestion(int i)
        {
            current.AnswerQuestion(i);
            return current;
        }
    }
}