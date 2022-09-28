using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelBTNode : CompositeBTNode
{
    public int MinNumberofSuccess;
    private int currentIndex, failureCount, runningCount;

    public ParallelBTNode()
    {
        currentIndex = 0;
    }

    public override void Abort()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnter()
    {
        failureCount = 0;
        runningCount = 0;
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        failureCount = 0;
        runningCount = 0;
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        foreach (var child in ChildNodes)
        {
            var childStatus = child.Tick();
            if (childStatus == IBTNode.ReturnStatus.FAILED)
            {
                failureCount++;
            }
            else if (childStatus == IBTNode.ReturnStatus.RUNNING)
            {
                runningCount++;
            }
        }
        return (ChildNodes.Count - failureCount) >= MinNumberofSuccess ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILED;

    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    }
}
