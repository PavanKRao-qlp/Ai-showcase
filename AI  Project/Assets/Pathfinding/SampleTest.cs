using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsExtension;

public class SampleTest : MonoBehaviour
{
    public NodeObject NodeObject_;
    public Vector2Int Size_;
    public float range_ = 50;
    [Range(0, 1)]
    public float cutoff;
    Grid2D_ Grid2D_;
    private Texture2D tex;
    Vector2Int start, end;
    void Start()
    {
        UnityEngine.Random.InitState((int) System.DateTime.Now.Ticks);
        var rand = UnityEngine.Random.value;
        Grid2D_ = new Grid2D_(Size_, NodeObject_, this.transform);

        for (int i = 0; i < Size_.x; i++)
        {
            for (int j = 0; j < Size_.y; j++)
            {
                float value = Mathf.PerlinNoise(((i + rand) / (float)Size_.x) * range_, ((j + rand) / (float)Size_.y) * range_);
                bool isWall = (value > cutoff) || i == 0 || j == 0 || i == Size_.x - 1 || j == Size_.y - 1 ? true : false;
                if (isWall) Grid2D_.Grid_[i, j].State_ |= Node2D_.TileState.wall;
                Grid2D_.Grid_[i, j].Update();
                if (i > 0 && !Grid2D_.Grid_[i, j].State_.HasFlag(Node2D_.TileState.wall) && !Grid2D_.Grid_[i - 1, j].State_.HasFlag(Node2D_.TileState.wall)) Grid2D_.Grid_[i, j].ConnectNode(Grid2D_.Grid_[i - 1, j]);
                if (j > 0 && !Grid2D_.Grid_[i, j].State_.HasFlag(Node2D_.TileState.wall) && !Grid2D_.Grid_[i , j - 1].State_.HasFlag(Node2D_.TileState.wall)) Grid2D_.Grid_[i, j].ConnectNode(Grid2D_.Grid_[i, j -1]);
            }
        }
        do {
            start = new Vector2Int(UnityEngine.Random.Range(0, Size_.x), UnityEngine.Random.Range(0, Size_.y));
        } while (!(Grid2D_.Grid_[start.x, start.y].State_.HasFlag(Node2D_.TileState.wall)));
        do {
            end = new Vector2Int(UnityEngine.Random.Range(0, Size_.x), UnityEngine.Random.Range(0, Size_.y));
        } while ((Grid2D_.Grid_[end.x, end.y].State_.HasFlag(Node2D_.TileState.wall)));
        start = new Vector2Int(15, 15);
        Grid2D_.Grid_[start.x, start.y].NodeObject.Image_.color = Color.green;
        Grid2D_.Grid_[end.x, end.y].NodeObject.Image_.color = Color.red;

        
         StartCoroutine("AStar");
         //StartCoroutine("DFS");
    }

    // Update is called once per frame
        

    IEnumerator BFS()
    {
        var q = new Queue<Node2D_>();
        var visted = new List<Node2D_>();
        var nodeStart = Grid2D_.Grid_[start.x, start.y];
        var parentMap = new Dictionary<Node2D_, Node2D_>();
        var path = new Stack<Node2D_>();
        Node2D_ nodeCur= new Node2D_();
        q.Enqueue(nodeStart);
        while (q.Count > 0)
        {
            var node = q.Dequeue();
            Debug.Log($"node {node.Positions.ToString()}");
            CheckNode(node);
           visted.Add(node); 
            if (node.Positions == end)
            {
                nodeCur = node;
                break;
            }
            foreach (Node2D_ child in node.GetConnectedNodes())
            {

                if (!visted.Contains(child) && !q.Contains(child))
                {
                    CheckNodeNext(child);
                    parentMap.Add(child, node);
                    q.Enqueue(child);

                }
            }
            if(UnityEngine.Random.Range(0,90) > 50) yield return null;
        }
        while (nodeCur.Positions != nodeStart.Positions)
        {
            nodeCur = parentMap[nodeCur];
            nodeCur.NodeObject.Image_.color = Color.yellow;
        }
    }

    IEnumerator DFS()
    {
        var q = new Stack<Node2D_>();
        var visted = new List<Node2D_>();
        var nodeStart = Grid2D_.Grid_[start.x, start.y];
        var parentMap = new Dictionary<Node2D_, Node2D_>();
        var path = new Stack<Node2D_>();
        Node2D_ nodeCur = new Node2D_();

        q.Push(nodeStart);
        while (q.Count > 0)
        {
            var node = q.Pop();
            Debug.Log($"node {node.Positions.ToString()}");
            CheckNode(node);
            visted.Add(node);
            if (node.Positions == end)
            {
                nodeCur = node;
                break;
            }
            foreach (Node2D_ child in node.GetConnectedNodes())
            {

                if (!visted.Contains(child) && !q.Contains(child))
                {
                    CheckNodeNext(child); 
                    parentMap.Add(child, node);
                    q.Push(child);

                }
            }
            if (UnityEngine.Random.Range(0, 90) > 50) yield return null;

        } 
        while (nodeCur.Positions != nodeStart.Positions)
        {
            nodeCur = parentMap[nodeCur];
            nodeCur.NodeObject.Image_.color = Color.yellow;
        }
    }

    IEnumerator AStar()
    {
        var paramIn = new AStarSolver.AStarParamIn<Node2D_>
        {
            StartNode = Grid2D_.Grid_[start.x, start.y],
            EndNode = Grid2D_.Grid_[end.x, end.y],
            CalculateHeuristicCost = (Node2D_ node, Node2D_ goalNode) =>
            {
                return (goalNode.Positions - node.Positions).magnitude;
            },
            equalityComparer = EqualityComparer<Node2D_>.Default,
            CalculateEdgeCost = (IEdge<Node2D_> edge) =>
            {
                return 1;
            }
        };
        var paramOut = AStarSolver.SolveViaAStar(paramIn);
        if (paramOut.FoundPath)
        {
            while (paramOut.Path.Count != 0)
            {
                var edge = paramOut.Path.Pop();
                edge.From.NodeObject.Image_.color = Color.yellow;
            }
        }
        yield return null;
    }


    IEnumerator AStarJPS()
    {
        yield return null;
    }

    private void CheckForForcedNeigbours(Vector2 positions)
    {
        
    }

    void CheckNode(Node2D_ node)
    {
        
        node.NodeObject.Image_.color = Color.blue;
    }
    void CheckNode(ComparableHeapNode<Node2D_> node)
    {

        node.data_.NodeObject.Image_.color = Color.Lerp(Color.blue , Color.green , node.GetValue() /(Size_.x * 1.414f));
    }
    void CheckNodeNext(Node2D_ node)
    {

        node.NodeObject.Image_.color = Color.magenta;
    }

    
}
