using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorBTNode : CompositeBTNode
{
    public override BTNode.ReturnStatus Tick()
    {
        foreach (var child in ChildNodes)
        {
            var childStatus = child.Tick();
            if (childStatus != BTNode.ReturnStatus.FAILED)
            {
                return childStatus;
            }
        }
        return BTNode.ReturnStatus.FAILED;
    }
}
