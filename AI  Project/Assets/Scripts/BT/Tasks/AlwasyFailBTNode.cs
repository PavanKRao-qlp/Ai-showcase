

public class AlwasyFailBTNode : TaskBTNode
{
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }
    public override void OnEnter() { }
    public override void OnExit(IBTNode.ReturnStatus status) { }
    public override IBTNode.ReturnStatus OnUpdate()
    {
        return IBTNode.ReturnStatus.FAILED;
    }
    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
    }
}
