[System.Serializable]
public class RootBTNode : IBTNode
{
    public CompositeBTNode ChildNode; //todo change to ibtnode
    public BehaviorTree BT { get { return behaviorTree; } set { behaviorTree = value; } }
    [System.NonSerialized] private BehaviorTree behaviorTree;
    protected IBTNode.ReturnStatus status = IBTNode.ReturnStatus.INACTIVE;

    public IBTNode.ReturnStatus Tick()
    {
        if (status == IBTNode.ReturnStatus.INACTIVE)
        {
            OnEnter();
            status = IBTNode.ReturnStatus.RUNNING;
        }
        if (status != IBTNode.ReturnStatus.RUNNING && status != IBTNode.ReturnStatus.INACTIVE) return status;
        status = OnUpdate();
        if (status != IBTNode.ReturnStatus.RUNNING) OnExit(status);
        return status;
    }

    public void SetChild(CompositeBTNode node)
    {
        ChildNode = node;
    }

    public void OnEnter() { }
    public void OnExit(IBTNode.ReturnStatus status) {
        Reset();
    }
    public IBTNode.ReturnStatus OnUpdate()
    {       
        return ChildNode?.Tick() ?? IBTNode.ReturnStatus.FAILED;
    }

    public void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
        ChildNode.Reset();
    }

    public void Abort()
    {
        ChildNode.Abort();
    }
}
