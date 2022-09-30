using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DecoratorBTNode : IBTNode
{
    public IBTNode ChildNode;
    [System.NonSerialized] private BehaviorTree behaviorTree;
    public BehaviorTree BT { get { return behaviorTree; } set { behaviorTree = value; } }
    protected IBTNode.ReturnStatus status = IBTNode.ReturnStatus.INACTIVE;
    public System.Action<IBTNode.ReturnStatus> OnTick { get; set; }
    public string Name { get; set; }
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
    public abstract void Reset();
    public abstract void Abort();
}
