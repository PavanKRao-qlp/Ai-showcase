public class CheckHealthBelowX : TaskBTNode
{
    private float healthThreshold;
    private float health = -1;
    public CheckHealthBelowX(float threshold)
    {
        healthThreshold = threshold;
    }
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnter()
    {
        health = BT.Blackboard.GetEntity(BT.Agent.Id)?.health ?? -1;
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        health = - 1;
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        return (health <= healthThreshold) ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.FAILED;
    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    }
}
