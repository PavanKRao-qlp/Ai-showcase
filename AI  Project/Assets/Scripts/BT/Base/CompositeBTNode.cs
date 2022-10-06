using System.Collections.Generic;

[System.Serializable]
public abstract class CompositeBTNode : IBTNode
{
    public List<IBTNode> ChildNodes = new List<IBTNode>();
    [System.NonSerialized] private BehaviorTree behaviorTree;
    public BehaviorTree BT { get { return behaviorTree; } set { behaviorTree = value; } }
    public IBTNode ParentNode { get; set; }

    public System.Action<IBTNode.ReturnStatus> OnTick { get; set; }
    public string TagName { get; set; }
    public IBTNode.ReturnStatus status { get; set; } = IBTNode.ReturnStatus.INACTIVE;
    protected int currentRunningNodeIx =-1;
    public abstract void OnEnter();
    public abstract void OnExit(IBTNode.ReturnStatus status);
    public abstract IBTNode.ReturnStatus OnUpdate();
    public IBTNode.ReturnStatus Tick()
     {
        if(status == IBTNode.ReturnStatus.ABORTED)
        {
            Reset();
        }
        if (status == IBTNode.ReturnStatus.INACTIVE)
        {
            OnEnter();
            status = IBTNode.ReturnStatus.RUNNING;
        }
        var curStatus = status = OnUpdate();
        if (status != IBTNode.ReturnStatus.RUNNING) OnExit(status);
        return curStatus;
    }
    public void Add(IBTNode node)
    {
        ChildNodes.Add(node);
    }
    public virtual void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
        foreach (var child in ChildNodes)
        {
            child.Reset();
        }
        currentRunningNodeIx = -1;
    }
    public abstract void Abort();
}
