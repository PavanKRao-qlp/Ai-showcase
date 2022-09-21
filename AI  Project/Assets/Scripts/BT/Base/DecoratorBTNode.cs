using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DecoratorBTNode : BTNode
{
    public BTNode.ReturnStatus Tick()
    {
        throw new System.NotImplementedException();
    }
}
