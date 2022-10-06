using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBTNode {
    public enum ReturnStatus
    {
        INACTIVE,
        FAILURE,
        RUNNING,
        SUCCESS,
        ABORTED
    }

    public string TagName { get; set; }
    public ReturnStatus status { get; set; }
    public IBTNode ParentNode { get; set; }
    public void OnEnter();
    public void OnExit(ReturnStatus status);
    public ReturnStatus OnUpdate();
    public ReturnStatus Tick();
    public BehaviorTree BT { get; set; }
    public void Reset();
    public void Abort();
}
