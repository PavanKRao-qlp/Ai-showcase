using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPlayerIsVisible : MonitorBTNode
{
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnter() { }

    public override void OnExit(IBTNode.ReturnStatus status) {
        base.OnExit(status);
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        Debug.Log($"can see player {BT.Blackboard.GetEntity(BT.Agent.Id).canSeeTarget}");
        return BT.Blackboard.GetEntity(BT.Agent.Id).canSeeTarget ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILURE;
    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    } 

}
