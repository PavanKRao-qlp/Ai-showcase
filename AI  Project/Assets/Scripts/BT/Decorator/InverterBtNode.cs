public class InverterBTNode : DecoratorBTNode
{
    public override void OnEnter()
    {
    }
    public override void OnExit(IBTNode.ReturnStatus status)
    {
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        var childStatus = ChildNode.Tick();
        if (childStatus != IBTNode.ReturnStatus.RUNNING)
        {
            if (childStatus == IBTNode.ReturnStatus.SUCCESS)
                return IBTNode.ReturnStatus.FAILURE;
            else if (childStatus == IBTNode.ReturnStatus.FAILURE)
                return IBTNode.ReturnStatus.SUCCESS;
        }
        return childStatus;
    }

    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
        ChildNode.Abort();
    }
}
