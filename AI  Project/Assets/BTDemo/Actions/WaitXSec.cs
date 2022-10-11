using System;
using UnityEngine;

public class WaitXSec : TaskBTNode
{
    private float timeElapsed;
    private float waitDuration;
    private float startTime;

    public WaitXSec(float duration)
    {
        timeElapsed = 0;
        waitDuration = duration;
    }

    public override void OnEnter()
    {
        startTime = Time.time;
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        timeElapsed = 0;
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        timeElapsed = Time.time - startTime;
        return waitDuration <= timeElapsed ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.RUNNING;
    }

    public override void Abort()
    {
        OnExit(IBTNode.ReturnStatus.ABORTED);
        this.status = IBTNode.ReturnStatus.ABORTED;
    }
}
