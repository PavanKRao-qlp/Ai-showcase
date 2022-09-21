using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RootBTNode : BTNode
{
    public CompositeBTNode ChildNode;
    public BTNode.ReturnStatus Tick()
    {
        if (ChildNode == null) Debug.Log("Child Node null");
        return ChildNode?.Tick() ?? BTNode.ReturnStatus.FAILED;
    }
    public void AddChild(CompositeBTNode node)
    {
        ChildNode = node;
    }
}
