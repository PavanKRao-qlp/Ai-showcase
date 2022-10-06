

public class AlwasySucceedlBTNode : TaskBTNode
{
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }
    public override void OnEnter() { }
    public override void OnExit(IBTNode.ReturnStatus status) { }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        return IBTNode.ReturnStatus.SUCCESS;
    }
}
