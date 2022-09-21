using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BehaviorTree
{
    public RootBTNode RootNode;
    public void Tick() {
        RootNode.Tick();
    }
}
