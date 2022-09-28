using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CompositeBTNode : IBTNode
{
    public List<IBTNode> ChildNodes = new List<IBTNode>();
    [System.NonSerialized] private BehaviorTree behaviorTree;
    public BehaviorTree BT { get { return behaviorTree; } set { behaviorTree = value; } }
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
        status = OnUpdate();
        if (status != IBTNode.ReturnStatus.RUNNING) OnExit(status);
        return status;
    }
    public void Add(IBTNode node)
    {
        ChildNodes.Add(node);
    }
    public abstract void Reset();
    public abstract void Abort();
}
