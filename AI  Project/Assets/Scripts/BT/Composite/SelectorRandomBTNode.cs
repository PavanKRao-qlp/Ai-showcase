using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorRandomBTNode : CompositeBTNode
{
    private int randomIx = -1;
    public override void Abort()
    {
        if (currentRunningNodeIx > -1)
        {
            ChildNodes[currentRunningNodeIx].Abort();
            this.status = IBTNode.ReturnStatus.ABORTED;
        }
    }

    public override void OnEnter()
    {
        randomIx = Random.Range(0, ChildNodes.Count);
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        currentRunningNodeIx = ChildNodes[randomIx].status == IBTNode.ReturnStatus.RUNNING ? randomIx : -1;
        return ChildNodes[randomIx].Tick();
    }
}
