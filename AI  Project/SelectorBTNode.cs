using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorBTNode : CompositeBTNode
{
    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        throw new System.NotImplementedException();
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        foreach (var child in ChildNodes)
        {
            var childStatus = child.Tick();
            if (childStatus != IBTNode.ReturnStatus.FAILED)
            {
                return childStatus;
            }
        }
        return IBTNode.ReturnStatus.FAILED;
    }
}
