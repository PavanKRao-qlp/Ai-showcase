[System.Serializable]
public class RootBTNode : IBTNode
{
    public IBTNode ChildNode; //todo change to ibtnode
    public BehaviorTree BT { get { return behaviorTree; } set { behaviorTree = value; } }
    [System.NonSerialized] private BehaviorTree behaviorTree;
    public IBTNode.ReturnStatus status { get; set; } = IBTNode.ReturnStatus.INACTIVE;
    public System.Action<IBTNode.ReturnStatus> OnTick { get; set; }
    public string TagName { get; set; }
    public IBTNode ParentNode { get; set; }

    private bool reset = false;

    public IBTNode.ReturnStatus Tick()
    {
        if (reset)
            Reset();
        if (status == IBTNode.ReturnStatus.INACTIVE)
        {
            OnEnter();
            status = IBTNode.ReturnStatus.RUNNING;
        }
        if (status != IBTNode.ReturnStatus.RUNNING && status != IBTNode.ReturnStatus.INACTIVE) return status;
        var curStatus = status = OnUpdate();
        if (status != IBTNode.ReturnStatus.RUNNING) OnExit(status);
        return curStatus;
    }

    public void SetChild(CompositeBTNode node)
    {
        ChildNode = node;
    }

    public void OnEnter() { }
    public void OnExit(IBTNode.ReturnStatus status) {
        reset = true;
    }
    public IBTNode.ReturnStatus OnUpdate()
    {       
        return ChildNode?.Tick() ?? IBTNode.ReturnStatus.FAILURE;
    }

    public void Reset()
    {
        reset = false;
        status = IBTNode.ReturnStatus.INACTIVE;
        ChildNode.Reset();
    }

    public void Abort()
    {
        ChildNode.Abort();
    }
}
