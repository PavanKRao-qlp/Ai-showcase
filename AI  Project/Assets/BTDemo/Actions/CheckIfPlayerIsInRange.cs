using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPlayerIsInRange : MonitorBTNode
{
    float range = 0;
    public CheckIfPlayerIsInRange(float range)
    {
        this.range = range;
    }
    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
    }

    public override void OnEnter() { }

    public override void OnExit(IBTNode.ReturnStatus status) {
        base.OnExit(status);
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        return BT.Blackboard.GetEntity(BT.Agent.Id).targetDist <= range ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILURE;
    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    } 

}
