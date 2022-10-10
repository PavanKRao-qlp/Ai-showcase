using UnityEngine;

public class FindSafeArea : TaskBTNode
{
    bool foundSafeArea = false;
    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
    }
    public override void OnEnter()
    {
        string targetId = BT.Blackboard.GetEntity(BT.Agent.Id).targetId;
        if (!string.IsNullOrEmpty(targetId))
        {
            Vector3 targetPos = BT.Blackboard.GetEntity(targetId).pos;
            var safePos = (targetPos - BT.Agent.GameObject.transform.position).normalized * -10;
            Debug.DrawLine(BT.Agent.GameObject.transform.position, safePos, Color.magenta,10);
            BT.Blackboard.GetEntity(BT.Agent.Id).goToPos = safePos;
            foundSafeArea = true;
        }
    }
    public override void OnExit(IBTNode.ReturnStatus status)
    {
        foundSafeArea = false;
    }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        return foundSafeArea ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILURE;
    }
}
