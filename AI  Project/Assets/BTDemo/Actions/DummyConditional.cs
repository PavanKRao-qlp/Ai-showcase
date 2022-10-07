using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyConditional : MonitorBTNode
{
    private int conditionFlag;
    public DummyConditional(int conditionFlag)
    {
        this.conditionFlag = conditionFlag;
    }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        Debug.Log($"dc1 {status}");
        return BT.Blackboard.GetEntity(BT.Agent.Id).condtition == conditionFlag ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILURE;
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        base.OnExit(status);
        Debug.Log($"dc2 {status}");
    }
}
