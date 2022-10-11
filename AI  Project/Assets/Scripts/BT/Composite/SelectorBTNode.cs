using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorBTNode : CompositeBTNode
{
    public override void Abort()
    {
        if (currentRunningNodeIx > -1)
            ChildNodes[currentRunningNodeIx].Abort();
    }

    public override void OnEnter()
    {
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        int childIx = 0;
        foreach (var child in ChildNodes)
        {
            var childStatus = child.Tick();
            if (childStatus != IBTNode.ReturnStatus.FAILURE)
            {
                if (currentRunningNodeIx != -1 && currentRunningNodeIx > childIx)
                {
                        ChildNodes[currentRunningNodeIx].Abort();
                }
                if (childStatus == IBTNode.ReturnStatus.RUNNING)
                    currentRunningNodeIx = childIx;
                return childStatus;
            }
            childIx++;
        }
        return IBTNode.ReturnStatus.FAILURE;
    }
}
