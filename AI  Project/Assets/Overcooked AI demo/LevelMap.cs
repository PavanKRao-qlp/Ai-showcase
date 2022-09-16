using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridDT;

using LevelNode = GridDT.Node2D<LevelMap.LevelTile>;
public class LevelMap : MonoBehaviour
{
    public GameObject FloorTile;
    public GameObject WallTile;
    public CookAI Agent;
    public BaseInteractable Item1, Item2, Item3;

    public Grid2D<LevelTile> LevelTileMap;

    public void SpawnSmartObjects()
    {
        Item1.transform.position = new Vector3(11, 1, 11);
        Item2.transform.position = new Vector3(1, 1, 1);
        Item3.transform.position = new Vector3(23, 1, 23);
    }

    public void SpawnPlayer()
    {
        var posX = UnityEngine.Random.Range(0, 25);
        var posY = UnityEngine.Random.Range(0, 25);

        while (LevelTileMap[posX, posY].Data.TileType == LevelTile.LevelTileType.WALL)
        {
            posX = UnityEngine.Random.Range(0, 25);
            posY = UnityEngine.Random.Range(0, 25);

        }
        Agent.CellPos = new Vector2Int(posX, posY);
        Agent.transform.position = new Vector3(posX, 1.5f, posY);
    }

    public List<LevelNode> FindPathToCell(Vector2Int from, Vector2Int to)
    {
        List<LevelNode> path = null;
        var paramIn = new AStarSolver.AStarParamIn<LevelNode>
        {
            StartNode = LevelTileMap[from.x, from.y],
            EndNode = LevelTileMap[to.x, to.y],
            CalculateHeuristicCost = (LevelNode node, LevelNode goalNode) =>
            {
                return (goalNode.Position - node.Position).magnitude;
            },
            equalityComparer = EqualityComparer<LevelNode>.Default,
            CalculateEdgeCost = (IEdge<LevelNode> edge) =>
            {
                return edge.EdgeCost * 2;
            }
        };
        var solution =  AStarSolver.SolveViaAStar<LevelNode>(paramIn);
        if (solution.FoundPath)
        {
            path = new List<LevelNode>();
            foreach (var node in solution.Path)
            {
                var fromPos = new Vector3(node.From.Position.x, 1.5f, node.From.Position.y);
                var toPos = new Vector3(node.To.Position.x, 1.5f, node.To.Position.y);
                Debug.DrawLine(fromPos, toPos, Color.yellow, 10);
                path.Add(node.To);
            }            
        }
        return path;
    }

    private List<GameObject> LevelTileMapObjects;
    void Start()
    {
       
    }

    public void GenerateMap()
    {
        var walledTile = new List<Vector2>{
            new Vector2(9,12),
            new Vector2(10,12),
            new Vector2(11,12),
            new Vector2(12,12),
            new Vector2(13,12),
            new Vector2(14,12),
            new Vector2(15,12),
            new Vector2(12,9),
            new Vector2(12,10),
            new Vector2(12,11),
            new Vector2(12,12),
            new Vector2(12,13),
            new Vector2(12,14),
            new Vector2(12,15),
            new Vector2(20,15),
            new Vector2(21,15),
            new Vector2(23,15),
            new Vector2(5,15),
            new Vector2(5,16),
            new Vector2(5,17),
            new Vector2(5,18),
            new Vector2(20,5),
            new Vector2(20,6),
            new Vector2(20,7),
            new Vector2(20,8),
            new Vector2(5,5),
            new Vector2(4,5),
            new Vector2(3,5),
            new Vector2(2,5),
        };

        LevelTileMap = new Grid2D<LevelTile>(25, 25, GridDT.Traversal.ALL);
        LevelTileMapObjects = new List<GameObject>(25 * 25);
        for (int x = 0; x < 25; x++)
        {
            for (int y = 0; y < 25; y++)
            {
                LevelTileMap[x, y] = new Node2D<LevelTile>(x, y)
                {
                    Data = new LevelTile()
                };
                if (x == 0 || x == 24 || y == 0 || y == 24 || walledTile.Contains(new Vector2(x,y)))
                {
                    LevelTileMap[x, y].Data.TileType = LevelTile.LevelTileType.WALL;
                }
                else
                {
                    LevelTileMap[x, y].Data.TileType = LevelTile.LevelTileType.PLAIN;
                }
                var prefab = LevelTileMap[x, y].Data.TileType == LevelTile.LevelTileType.PLAIN ? FloorTile : WallTile;
                var height = LevelTileMap[x, y].Data.TileType == LevelTile.LevelTileType.PLAIN ? 0 : 1;
                var tileObject = Instantiate(prefab, new Vector3(x, height, y), Quaternion.identity, this.transform);
            }
        }
        for (int x = 1; x < 24; x++)
        {
            for (int y = 1; y < 24; y++)
            {
                if (LevelTileMap[x, y].Data.TileType.HasFlag(LevelTile.LevelTileType.WALL)) continue;

                if (!LevelTileMap[x - 1, y].Data.TileType.HasFlag(LevelTile.LevelTileType.WALL)) LevelTileMap[x, y].ConnectNode(LevelTileMap[x - 1, y]);

                if (!LevelTileMap[x, y - 1].Data.TileType.HasFlag(LevelTile.LevelTileType.WALL)) LevelTileMap[x, y].ConnectNode(LevelTileMap[x, y - 1]);

                if (!LevelTileMap[x - 1, y - 1].Data.TileType.HasFlag(LevelTile.LevelTileType.WALL)) LevelTileMap[x, y].ConnectNode(LevelTileMap[x - 1, y - 1]);

                if (!LevelTileMap[x - 1, y + 1].Data.TileType.HasFlag(LevelTile.LevelTileType.WALL)) LevelTileMap[x, y].ConnectNode(LevelTileMap[x - 1, y + 1]);
            }
        }
    }
    public void PlaceItems()
    {

    }

    public struct LevelTile
    {
        public enum LevelTileType
        {
            PLAIN,
            WALL
        }

        public LevelTileType TileType;
    }
}
