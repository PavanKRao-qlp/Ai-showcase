using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorRandomBTNode : CompositeBTNode
{
    private int randomIx = -1;
    public override void Abort()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnter()
    {
        randomIx = Random.Range(0, ChildNodes.Count);
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        return ChildNodes[randomIx].Tick();
    }
}
