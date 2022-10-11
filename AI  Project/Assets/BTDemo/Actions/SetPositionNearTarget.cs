using UnityEngine;

public class SetPositionNearTarget : TaskBTNode
{
    public float DistanceRange = 1;
    Vector3 position;
    public override void OnEnter() { }
    public override void OnExit(IBTNode.ReturnStatus status) { }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        if (BT?.Agent?.GameObject == null || BT?.Blackboard == null) return IBTNode.ReturnStatus.FAILURE;
        Vector3 tagetPos = BT.Blackboard.GetEntity(BT.Agent.Id).targetPos;
        tagetPos = tagetPos - (tagetPos.normalized * 2f);
        BT.Blackboard.GetEntity(BT.Agent.Id).goToPos = tagetPos;
        BT.Agent.GameObject.transform.LookAt(BT.Blackboard.GetEntity(BT.Agent.Id).targetPos);
        return IBTNode.ReturnStatus.SUCCESS;
    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    }
    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
    }
}
