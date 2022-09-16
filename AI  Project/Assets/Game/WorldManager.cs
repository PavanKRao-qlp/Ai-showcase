using GridDT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Vector2Int GridSize;
    [SerializeField] PerlinParam WallParams;
    public WorldMap WorldMapObject;

    public Grid2D<WorldCell> WorldGrid{ get; private set; }

    [System.Serializable]
    public struct PerlinParam
    {
        public float Scale;
        [Range(0, 1)]
        public float cutoff;
    }

    public void GenerateNewWorld()
    {
        var rndWallOffset = Random.value * 10000;
        WorldGrid = new Grid2D<WorldCell>(GridSize.x, GridSize.y, Traversal.ALL);
        for (int x = 0; x < WorldGrid.Width; x++)
        {
            for (int y = 0; y < WorldGrid.Height; y++)
            {
                WorldGrid[x, y] = new Node2D<WorldCell>(x, y)
                {
                    Data = new WorldCell()
                };

                var sampleX = ((x / (float)WorldGrid.Width) * WallParams.Scale);
                var sampleY = ((y / (float)WorldGrid.Height) * WallParams.Scale);
                var perlinWall = Mathf.PerlinNoise(sampleX + rndWallOffset, sampleY + rndWallOffset);
                // wall on all borders
                var isWall = perlinWall > WallParams.cutoff || x == 0 || y == 0 || x == WorldGrid.Width - 1 || y == WorldGrid.Height - 1 ? true : false;
                WorldGrid[x, y].Data.TraversableState = isWall ? WorldCell.TraversableStateEnum.UNTRAVERSABLE : WorldCell.TraversableStateEnum.TRAVERSABLE;
            }
        }

        for (int x = 1; x < WorldGrid.Width - 1; x++)
        {
            for (int y = 1; y < WorldGrid.Height - 1; y++)
            {
                if (WorldGrid[x, y].Data.TraversableState.HasFlag(WorldCell.TraversableStateEnum.UNTRAVERSABLE)) continue;

                if (!WorldGrid[x - 1, y].Data.TraversableState.HasFlag(WorldCell.TraversableStateEnum.UNTRAVERSABLE)) WorldGrid[x, y].ConnectNode(WorldGrid[x - 1, y]);

                if (!WorldGrid[x, y - 1].Data.TraversableState.HasFlag(WorldCell.TraversableStateEnum.UNTRAVERSABLE)) WorldGrid[x, y].ConnectNode(WorldGrid[x, y - 1]);

                if (!WorldGrid[x - 1, y - 1].Data.TraversableState.HasFlag(WorldCell.TraversableStateEnum.UNTRAVERSABLE)) WorldGrid[x, y].ConnectNode(WorldGrid[x - 1, y - 1]);

                if (!WorldGrid[x - 1, y + 1].Data.TraversableState.HasFlag(WorldCell.TraversableStateEnum.UNTRAVERSABLE)) WorldGrid[x, y].ConnectNode(WorldGrid[x - 1, y + 1]);
            }
        }
        WorldMapObject.DrawWorld(WorldGrid);
    }

    private void OnValidate()
    {
        Camera.main.transform.position = new Vector3(GridSize.x / 2f, GridSize.y / 2f, -10);
        Camera.main.orthographicSize = Mathf.Ceil(GridSize.y / 2.0f);
    }

    public bool TryGetPath(Vector3 startPos, Vector3 goalPos, out Vector2[] path)
    {
        var startCell = WorldMapObject.ConvertWorldToCell(startPos);
        var goalCell = WorldMapObject.ConvertWorldToCell(goalPos);
        var paramIn = new AStarSolver.AStarParamIn<Node2D<WorldCell>>
        {
            StartNode = WorldGrid[startCell.x, startCell.y],
            EndNode = WorldGrid[goalCell.x,goalCell.y],
            CalculateHeuristicCost = (Node2D<WorldCell> node, Node2D<WorldCell> goalNode) =>
            {
                return (goalNode.Position - node.Position).magnitude;
            },
            equalityComparer = EqualityComparer<Node2D<WorldCell>>.Default,
            CalculateEdgeCost = (IEdge<Node2D<WorldCell>> edge) =>
            {
                return edge.EdgeCost;
            }
        };
        var paramOut = AStarSolver.SolveViaAStar(paramIn);
        if (paramOut.FoundPath)
        {
            path = paramOut.Path.Select(x => x.To.Position).ToArray();
            foreach (var item in paramOut.Path)
            {
                Debug.DrawLine(item.From.Position + (Vector2.one * 0.5f) , item.To.Position + (Vector2.one * 0.5f), Color.red, 100);
            }
            return true;
        }
        path = null;
        return false;
    }
}

public struct WorldCell {
    public enum TraversableStateEnum
    {
        TRAVERSABLE,
        UNTRAVERSABLE
    }
    public enum CellType
    {
        PLAIN,
        COLLECTIBLE
    }

    public TraversableStateEnum TraversableState;
    public CellType Type;
}
