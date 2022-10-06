using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelBTNode : CompositeBTNode
{
    public int MinNumberofSuccess = 1;
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
        failureCount = 0;
        runningCount = 0;
        foreach (var child in ChildNodes)
        {
            var childStatus = child.Tick();
            if (childStatus == IBTNode.ReturnStatus.FAILURE)
            {
                failureCount++;
            }
            else if (childStatus == IBTNode.ReturnStatus.RUNNING)
            {
                runningCount++;
            }
        }
        if (runningCount > 0)
            return IBTNode.ReturnStatus.RUNNING;

        return (ChildNodes.Count - failureCount) >= MinNumberofSuccess + runningCount? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILURE;

    }
}
