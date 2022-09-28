using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBTNode {
    public enum ReturnStatus
    {
        INACTIVE,
        FAILED,
        RUNNING,
        SUCCESS
    }

    public void OnEnter();
    public void OnExit(ReturnStatus status);
    public ReturnStatus OnUpdate();
    public ReturnStatus Tick();
    public BehaviorTree BT { get; set; }
    public void Reset();
    public void Abort();
}
