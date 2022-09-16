using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode<T_Node> where T_Node : INode<T_Node>
{
    int GetUID();
    void AddConnectedEdge(IEdge<T_Node> edge);
    List<IEdge<T_Node>> GetEdges();
    List<T_Node> GetConnectedNodes();

}

public interface IEdge<T> where T : INode<T>
{
    T From { get;}
    T To { get;}
    float EdgeCost { get;}
}


public interface IGraph<T> where T : INode<T>
{
    int Count { get; }
    List<T> Nodes { get; }
    void AddNode(T node);
    void AddEdge(T node_from, T node_to, float cost, bool isOneWay = true);
    float GetDistanceBetween(T node_from, T node_to);
    void RemoveNode(T node);
    void RemoveEdge(T node_from, T node_to, float cost, bool isOneWay = true);
}

public class Graph<T> : IGraph<T> where T : INode<T>
{
    public int Count { get; private set; }
    public List<T> Nodes { get; private set; }
    public Graph()
    {
        Nodes = new List<T>();
    }
    public void AddEdge(T node_from, T node_to, float cost, bool isOneWay = true)
    {
       
    }

    public virtual void AddNode(T node)
    {
        if (!Nodes.Contains(node)) Nodes.Add(node);
        // else todo warn
    }

    public float GetDistanceBetween(T node_from, T node_to)
    {
        return 1;
    }

    public void RemoveEdge(T node_from, T node_to, float cost, bool isOneWay = true)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveNode(T node)
    {
        throw new System.NotImplementedException();
    }
}