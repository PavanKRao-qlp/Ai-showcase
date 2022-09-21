using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class TaskNode : BTNode
{
    public abstract BTNode.ReturnStatus Tick();
}
