using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSolver
{
    public struct AStarParamIn<T> where T : INode<T>
    {
        public T StartNode;
        public T EndNode;
        public System.Func<T, T, float> CalculateHeuristicCost;
        public System.Func<IEdge<T>, float> CalculateEdgeCost;
        public IEqualityComparer equalityComparer;
    }
    public struct AStarParamOut<T> where T : INode<T>
    {
        public bool FoundPath;
        public Stack<IEdge<T>> Path;
        public float PathCost;
    }
    public class AStarHeapnode<T> : ComparableHeapNode<T>, System.IEquatable<AStarHeapnode<T>> where T : INode<T>
    {
        public float HCost = 0, GCost = 0;
        public AStarHeapnode(T data, float gcost, float hcost) : base(data, 0)
        {
            this.GCost = gcost;
            this.HCost = hcost;
        }
        public AStarHeapnode(T data, float priority) : base(data, priority)
        {
            GCost = priority;
        }
        public override float GetValue() => HCost + GCost;
        public override int CompareTo(Iheapable other)
        {
            if (other == null) return 1; // 1 greater -1  smaller 0 equals 
            return this.GetValue().CompareTo(other.GetValue());
        }
        public bool Equals(AStarHeapnode<T> other)
        {
            return data_.GetUID() == other.data_.GetUID();
        }
    }
    public static AStarParamOut<T> SolveViaAStar<T>(AStarParamIn<T> paramIn) where T : INode<T>
     {
        MinHeap<AStarHeapnode<T>> exploreSet = new MinHeap<AStarHeapnode<T>>(5);
        var visitedSet = new Dictionary<int, AStarHeapnode<T>>();
        var path = new Stack<IEdge<T>>();
        var parentMap = new Dictionary<int, IEdge<T>>();
        exploreSet.Add(new AStarHeapnode<T>(paramIn.StartNode, 0, 0)); //add start node to explore 
        #region traversal

        while (exploreSet.Count > 0)
        {
            var reHeapify = false;
            if (exploreSet.Count == 0)
            {
                break;
            } // all no connected nodes let to check
            var heapNode = exploreSet.Poll(); //get lowest f cost option of exploreSet from min heap
            var currentExploringNode = heapNode.data_;
            visitedSet.Add(currentExploringNode.GetUID(), heapNode);
            if (paramIn.equalityComparer.Equals(currentExploringNode, paramIn.EndNode)) // using equalityComparer because sometimes we may need aprox equals as good enough  
            {
                path.Push(parentMap[currentExploringNode.GetUID()]);
                break;
            }
            foreach (var connectedEdge in currentExploringNode.GetEdges())
            {
                var connectedNode = connectedEdge.To;
                AStarHeapnode<T> childHeapNode;
                var newDist = heapNode.GCost + paramIn.CalculateEdgeCost(connectedEdge);
                if (visitedSet.ContainsKey(connectedNode.GetUID()))
                {
                    childHeapNode = visitedSet[connectedNode.GetUID()];
                    if (childHeapNode.GCost > newDist)
                    {
                        childHeapNode.GCost = newDist;
                        reHeapify = true;
                        parentMap[connectedNode.GetUID()] = connectedEdge;
                    }
                }
                else
                {
                    float HCost = paramIn.CalculateHeuristicCost(connectedNode, paramIn.EndNode);
                    childHeapNode = new AStarHeapnode<T>(connectedNode, newDist, HCost);
                }

                if (!visitedSet.ContainsKey(connectedNode.GetUID()))
                {
                    if (exploreSet.Contains(childHeapNode))
                    {
                        int ix = exploreSet.TryGetIndexOf(childHeapNode);
                        if (ix >= 0)
                        {
                            var prevNode = exploreSet.GetItemAt(ix);
                            if (prevNode.GetValue() > childHeapNode.GetValue())
                            {
                                prevNode.GCost = childHeapNode.GCost;
                                prevNode.HCost = childHeapNode.HCost;
                                parentMap[connectedNode.GetUID()] = connectedEdge;
                                reHeapify = true;
                            }
                        }
                    }
                    else
                    {
                        exploreSet.Add(childHeapNode);
                        parentMap[connectedNode.GetUID()] = connectedEdge;
                    }
                    // CheckNodeNext(connectedNode);
                }
            }
            if (reHeapify)
            {
                exploreSet.HeapifyUp();
            }
            //   yield return null; //TODO ENUMERATE ASYNC
        }
        #endregion
        #region path
        //recreate the path
        AStarParamOut<T> paramOut = new AStarParamOut<T>();
        paramOut.Path = new Stack<IEdge<T>>();
        if (path.Count == 0)
        {
            paramOut.FoundPath = false;
        }
        else
        {
            paramOut.FoundPath = true;
            paramOut.PathCost = 0;
            while (path.Peek().From.GetUID() != paramIn.StartNode.GetUID())
            {
                var edge = parentMap[path.Peek().From.GetUID()];
                var parent = edge.From;//visitedSet[parentId].data_;
                paramOut.PathCost += visitedSet[parent.GetUID()].HCost;
                path.Push(edge);
            }
            paramOut.Path = path;
        }
        #endregion

        Debug.LogError("ASTAR Visited " + visitedSet.Count);
        return paramOut;
    }
}
