using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CompositeBTNode : BTNode
{
    public List<BTNode> ChildNodes = new List<BTNode>();
    public abstract BTNode.ReturnStatus Tick();

    public void Add(BTNode node)
    {
        ChildNodes.Add(node);
    }
    
}
