public class SequenceBTNode : CompositeBTNode
{
    private bool resetOnExit = false;
    public override void OnEnter() {}

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        if (resetOnExit)
        {
            resetOnExit = false;
            Reset();
        }
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        int childIx = 0;
        foreach (var child in ChildNodes)
        {
            var childStatus = child.Tick();


            if (childStatus != IBTNode.ReturnStatus.SUCCESS)
            {
                if (currentRunningNodeIx != -1 && currentRunningNodeIx > childIx)
                {
                    ChildNodes[currentRunningNodeIx].Abort();
                }
                currentRunningNodeIx = childIx;
                return childStatus;
            }
            childIx++;
        }
        return IBTNode.ReturnStatus.SUCCESS;
    }

    public override void Abort()
    {
        if(currentRunningNodeIx > -1) ChildNodes[currentRunningNodeIx].Abort();
        this.status = IBTNode.ReturnStatus.ABORTED;
        resetOnExit = true;
    }
}
