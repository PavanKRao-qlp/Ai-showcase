public class CheckHealthBelowX : MonitorBTNode
{
    private float healthThreshold;
    private float health = -1;
    public CheckHealthBelowX(float threshold)
    {
        healthThreshold = threshold;
    }
    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
    }
    public override void OnEnter()
    {
        health = BT.Blackboard.GetEntity(BT.Agent.Id)?.health ?? -1;
    }
    public override void OnExit(IBTNode.ReturnStatus status)
    {
        base.OnExit(status);
        health = - 1;
    }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        health = BT.Blackboard.GetEntity(BT.Agent.Id)?.health ?? -1;
        return (health <= healthThreshold) ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILURE;
    }
}
