[System.Serializable]
public class RootBTNode : IBTNode
{
    public CompositeBTNode ChildNode; //todo change to ibtnode
    public BehaviorTree BT { get { return behaviorTree; } set { behaviorTree = value; } }
    [System.NonSerialized] private BehaviorTree behaviorTree;
    protected IBTNode.ReturnStatus status = IBTNode.ReturnStatus.INACTIVE;
    public System.Action<IBTNode.ReturnStatus> OnTick { get; set; }
    public string Name { get; set; }

    public IBTNode.ReturnStatus Tick()
    {
        if (status == IBTNode.ReturnStatus.INACTIVE)
        {
            OnEnter();
            status = IBTNode.ReturnStatus.RUNNING;
        }
        if (status != IBTNode.ReturnStatus.RUNNING && status != IBTNode.ReturnStatus.INACTIVE) return status;
        var curStatus = status = OnUpdate();
        if (status != IBTNode.ReturnStatus.RUNNING) OnExit(status);
#if UNITY_EDITOR
        OnTick?.Invoke(curStatus);
#endif
        UnityEngine.Debug.Log($"Pvn {this.GetType().Name} {this.Name}  {curStatus}");
        return curStatus;
    }

    public void SetChild(CompositeBTNode node)
    {
        ChildNode = node;
    }

    public void OnEnter() { }
    public void OnExit(IBTNode.ReturnStatus status) {
        //Reset();
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
