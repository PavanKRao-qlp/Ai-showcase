using Newtonsoft.Json.Linq;
using UnityEngine;

public class SetRandomPositionNearby : TaskBTNode
{
    public float DistanceRange = 1;
    Vector3 position;
    public override void OnEnter() {}
    public override void OnExit(IBTNode.ReturnStatus status) {}
    public override IBTNode.ReturnStatus OnUpdate()
    {
        if (BT?.Agent?.GameObject == null || BT?.Blackboard == null) return IBTNode.ReturnStatus.FAILURE;
        var randomPos = Random.insideUnitSphere * DistanceRange;
        position =  new Vector3(randomPos.x, BT.Agent.GameObject.transform.position.y, randomPos.z);
        var arrayPos = new float[] { position.x, position.y, position.z };
        BT.Blackboard.GetEntity(BT.Agent.Id).goToPos = position;
        return IBTNode.ReturnStatus.SUCCESS;
    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    }
    public override void Abort()
    {
        BT.Blackboard.GetEntity(BT.Agent.Id).goToPos = Vector3.zero;
        this.status = IBTNode.ReturnStatus.ABORTED;
    }
}
