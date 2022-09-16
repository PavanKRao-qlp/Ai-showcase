using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTree 
{
}

public struct ResourceNode : INode<ResourceNode>
{
    public int Id;
    public float TargetValue { get; set; }
    public float CurrentValue { get; set; }

    public void AddConnectedEdge(IEdge<ResourceNode> edge)
    {
        throw new System.NotImplementedException();
    }

    public List<ResourceNode> GetConnectedNodes()
    {
        throw new System.NotImplementedException();
    }

    public List<IEdge<ResourceNode>> GetEdges()
    {
        throw new System.NotImplementedException();
    }

    public int GetUID()
    {
        return Id;
    }
}
