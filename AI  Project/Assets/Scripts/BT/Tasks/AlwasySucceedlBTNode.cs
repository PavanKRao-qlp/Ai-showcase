

public class AlwasySucceedlBTNode : TaskBTNode
{
    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
    }
    public override void OnEnter() { }
    public override void OnExit(IBTNode.ReturnStatus status) { }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        return IBTNode.ReturnStatus.SUCCESS;
    }
}
