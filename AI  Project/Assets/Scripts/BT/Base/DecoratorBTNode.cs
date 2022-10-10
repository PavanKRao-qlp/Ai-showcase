using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DecoratorBTNode : IBTNode
{
    public IBTNode ChildNode;
    [System.NonSerialized] private BehaviorTree behaviorTree;
    public BehaviorTree BT { get { return behaviorTree; } set { behaviorTree = value; } }
    public IBTNode ParentNode { get; set; }
    public IBTNode.ReturnStatus status { get; set; } = IBTNode.ReturnStatus.INACTIVE;
    public System.Action<IBTNode.ReturnStatus> OnTick { get; set; }
    public string TagName { get; set; }
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
        var curStatus = status = OnUpdate();
        if (status != IBTNode.ReturnStatus.RUNNING) OnExit(status);
        return curStatus;
    }
    public virtual void Reset()
    {
        status = IBTNode.ReturnStatus.INACTIVE;
        ChildNode.Reset();
    }
    public abstract void Abort();
}
