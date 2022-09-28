using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BehaviorTree
{
    [System.NonSerialized]public IAgentBT Agent;
    public RootBTNode RootNode;
    public Blackboard Blackboard { get; set; }
    public BehaviorTree()
    {
        RootNode = new RootBTNode();
    }
    public void Tick() {
        RootNode.Tick();
    }
}
