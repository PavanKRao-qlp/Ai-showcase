public class SequenceBTNode : CompositeBTNode
{
    public override void OnEnter() {}

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        foreach (var child in ChildNodes)
        {
            child.Reset();
        }
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        foreach (var child in ChildNodes)
        {
            var childStatus = child.Tick();
            if (childStatus != IBTNode.ReturnStatus.SUCCESS)
            {
                return childStatus;
            }
        }
        return IBTNode.ReturnStatus.SUCCESS;
    }

    public override void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
        foreach (var child in ChildNodes)
        {
            child.Reset();
        }
    }
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }
}
