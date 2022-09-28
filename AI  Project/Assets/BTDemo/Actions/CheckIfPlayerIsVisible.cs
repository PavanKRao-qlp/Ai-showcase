using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPlayerIsVisible : TaskBTNode
{
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnter() { }

    public override void OnExit(IBTNode.ReturnStatus status) {}

    public override IBTNode.ReturnStatus OnUpdate()
    {
        Debug.Log(BT.Blackboard.GetEntity(BT.Agent.Id).canSeeTarget);
        return BT.Blackboard.GetEntity(BT.Agent.Id).canSeeTarget ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILED;
    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    }

}
