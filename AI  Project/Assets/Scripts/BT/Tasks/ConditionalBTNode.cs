using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalBTNode : TaskBTNode
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
        throw new System.NotImplementedException();
    }
    public override void OnEnter() { }
    public override void OnExit(IBTNode.ReturnStatus status)
    {
        this.status = IBTNode.ReturnStatus.INACTIVE;
    }
    public override IBTNode.ReturnStatus Tick()
    {
        return status = OnUpdate();
    }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        return IBTNode.ReturnStatus.RUNNING;
    }
}
