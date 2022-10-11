using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfTargetIsAlive : ConditionalBTNode
{
    private string targetId;
    public CheckIfTargetIsAlive()
    {
    }
    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
    }

    public override void OnEnter() {
        targetId = BT.Blackboard.GetEntity(BT.Agent.Id).targetId;
    }

    public override void OnExit(IBTNode.ReturnStatus status) {
        base.OnExit(status);
        targetId = string.Empty;
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        if (string.IsNullOrEmpty(targetId))
            return IBTNode.ReturnStatus.FAILURE;

        return BT.Blackboard.GetEntity(targetId).health > 0 ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILURE;
    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    } 

}
