using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceBTNode : CompositeBTNode
{
    private int currentIndex;
    public SequenceBTNode()
    {
        currentIndex = 0;
    }
    public override BTNode.ReturnStatus Tick()
    {
        foreach (var child in ChildNodes)
        {
            var childStatus = child.Tick();
            if (childStatus != BTNode.ReturnStatus.SUCCESS)
            {
                return childStatus;
            }
        }
        return BTNode.ReturnStatus.SUCCESS;
    }
}
