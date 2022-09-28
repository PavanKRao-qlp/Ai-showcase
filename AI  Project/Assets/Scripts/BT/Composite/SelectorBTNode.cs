using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorBTNode : CompositeBTNode
{
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnter()
    {
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        foreach (var child in ChildNodes)
        {
            child.Reset();
        }
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

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
        foreach (var child in ChildNodes)
        {
            child.Reset();
        }
    }
}
