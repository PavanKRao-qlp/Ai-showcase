using System.Collections.Generic;

[System.Serializable]
public abstract class CompositeBTNode : IBTNode
{
    public List<IBTNode> ChildNodes = new List<IBTNode>();
    [System.NonSerialized] private BehaviorTree behaviorTree;
    public BehaviorTree BT { get { return behaviorTree; } set { behaviorTree = value; } }

    public System.Action<IBTNode.ReturnStatus> OnTick { get; set; }
    public string Name { get; set; }

    protected IBTNode.ReturnStatus status = IBTNode.ReturnStatus.INACTIVE;
    public abstract void OnEnter();
    public abstract void OnExit(IBTNode.ReturnStatus status);
    public abstract IBTNode.ReturnStatus OnUpdate();
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
    public void Add(IBTNode node)
    {
        ChildNodes.Add(node);
    }
    public abstract void Reset();
    public abstract void Abort();
}
