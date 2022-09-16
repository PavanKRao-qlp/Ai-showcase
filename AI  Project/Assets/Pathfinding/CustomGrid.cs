using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace GridDT
{
    public enum Traversal
    {
        STRAIGHT,
        DIAGONAL,
        ALL
    }
    public class Grid2D<T> 
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        Traversal traversal;
        public Node2D<T>[,] gridData;

        public Grid2D(int _width, int _height, Traversal _traversal)
        {
            Width = _width;
            Height = _height;
            gridData = new Node2D<T>[Width, Height];
            traversal = _traversal;
        }

        public Node2D<T> this[int x , int y]
        {
            get {
                if (gridData == null) throw new System.NullReferenceException("gridData null!!");
                return gridData[x, y];
            }
             set
            {
                if (gridData == null) throw new System.NullReferenceException("gridData null!!");
                gridData[x, y] = value;
            }
        }

    }

    public class Node2D<T_Data> : INode<Node2D<T_Data>>
    {
        public Vector2 Position; 
        public T_Data Data;
        public List<Node2D<T_Data>> conectedNodes;
        public List<IEdge<Node2D<T_Data>>> edges;
        public int id = 0;
        public Node2D(int x , int y)
        {
            Position = new Vector2(x,y);
            conectedNodes = new List<Node2D<T_Data>>();
            edges = new List<IEdge<Node2D<T_Data>>>();
            id = x * 1000 + y;
        }

        public void AddConnectedEdge(IEdge<Node2D<T_Data>> edge)
        {
            if (!edges.Contains(edge))
                edges.Add(edge);
            if (!conectedNodes.Contains(edge.To))
                conectedNodes.Add(edge.To);
        }

        public List<Node2D<T_Data>> GetConnectedNodes()
        {
            return conectedNodes;
        }

        public List<IEdge<Node2D<T_Data>>> GetEdges()
        {
            if (edges == null) edges = new List<IEdge<Node2D<T_Data>>>();
            return edges;
        }

        public int GetUID()
        {
            return id;
        }

        public struct Node2DEdge : IEdge<Node2D<T_Data>>
        {
            public Node2DEdge(Node2D<T_Data> from, Node2D<T_Data> to, float cost)
            {
                From = from;
                To = to;
                EdgeCost = cost;
            }

            public Node2D<T_Data> From { get; set; }

            public Node2D<T_Data> To { get; set; }

            public float EdgeCost { get; set; }
        }
    }
    public static class GridUtils
    {
        public static void ConnectNode<T>(this Node2D<T> node , Node2D<T> toNode)
        {
            var edgeDist = (toNode.Position - node.Position).magnitude;
            node.AddConnectedEdge(new Node2D<T>.Node2DEdge()
            {
                EdgeCost = edgeDist,
                From = node,
                To = toNode
            });
            toNode.AddConnectedEdge(new Node2D<T>.Node2DEdge()
            {
                EdgeCost = edgeDist,
                From = toNode,
                To = node
            });
        }
        public static void FindNeighbour<T>(this Grid2D<T> grid, int x , int y)
        {
        }
    }

}
    public class Grid2D_
    {

        Node2D_[,] grid;
        public Node2D_[,] Grid_ => grid;

        public Grid2D_(Vector2Int size, NodeObject nodePrefab, Transform parent)
        {
            grid = new Node2D_[size.x, size.y];
            for (int i = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    grid[i, j] = new Node2D_
                    {
                        Positions = new Vector2(i, j),
                        NodeObject = GameObject.Instantiate(nodePrefab, new Vector2((-size.x / 2.0f) + i, (-size.y / 2.0f) + j), Quaternion.identity, parent),
                        conectedNodes = new List<Node2D_>(),
                        edges = new List<IEdge<Node2D_>>()
                    };
                }
            }
        }
    }



    public struct Node2DEdge : IEdge<Node2D_>
    {
        public Node2D_ From { get; private set; }

        public Node2D_ To { get; private set; }

        public float EdgeCost => 1;
        public Node2DEdge(Node2D_ _from, Node2D_ _to)
        {
            From = _from;
            To = _to;
        }
    }
    public struct Node2D_ : INode<Node2D_>
    {
        [System.Flags] //for easy to see intent
        public enum TileState
        {
            empty = 0,
            wall = 1,
            searched = 2,
            moved = 4,
            found = 8
        }
        public Vector2 Positions;
        public NodeObject NodeObject;
        public TileState State_;
        public List<Node2D_> conectedNodes;
        public List<IEdge<Node2D_>> edges;

        public void Update()
        {
            NodeObject.UpdateUI(State_);
        }
        public int GetUID() => Positions.ToString().GetHashCode();
        public void AddConnectedEdge(IEdge<Node2D_> edge)
        {
            if (edges == null) { edges = new List<IEdge<Node2D_>>(); }
            if (!edges.Contains(edge))
                edges.Add(edge);
            if (!conectedNodes.Contains(edge.To))
                conectedNodes.Add(edge.To);
        }

        public List<IEdge<Node2D_>> GetEdges()
        {
            if (edges == null) edges = new List<IEdge<Node2D_>>();
            return edges;
        }

        public List<Node2D_> GetConnectedNodes()
        {
            if (conectedNodes == null) conectedNodes = new List<Node2D_>();
            return conectedNodes;
        }
    }


    namespace UtilsExtension
    {
        public static class Utils
        {
            public static void ConnectNode(this Node2D_ thisNode_, Node2D_ node_)
            {
                Node2DEdge edge = new Node2DEdge(thisNode_, node_);
                Node2DEdge reverseEdge = new Node2DEdge(node_, thisNode_);
                thisNode_.AddConnectedEdge(edge);
                node_.AddConnectedEdge(reverseEdge);
            }
        }
    }

