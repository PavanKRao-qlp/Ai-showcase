using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorBTNode : TaskBTNode
{
    public enum InteruptRules
    {
        NONE,
        SIBLINGS,
        LOWER_PRIORITY,
        BOTH_SIBLINGS_AND_LOWER_PRIORITY
    }
    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
    }
    public override void OnEnter() { 
    
    }
    public override void OnExit(IBTNode.ReturnStatus status)
    {
    }
    public override IBTNode.ReturnStatus Tick()
    {
        if (status == IBTNode.ReturnStatus.ABORTED)
        {
            Reset();
        }
        if (status == IBTNode.ReturnStatus.INACTIVE)
        {
            OnEnter();
            status = IBTNode.ReturnStatus.RUNNING;
        }
        var curStatus = status = OnUpdate();
        if (status != IBTNode.ReturnStatus.RUNNING)
            OnExit(status);
        return curStatus;
    }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        return IBTNode.ReturnStatus.RUNNING;
    }
}
