#define UPDATE
using GridDT;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UtilsExtension;

public class SampleInfluence : MonoBehaviour
{
    public bool seed;
    [System.Serializable]
    public struct PerlinParam
    {
        public float _Scale;
        [Range(0, 1)]
        public float cutoff;
    }

    public struct Tile
    {
        public enum TileState
        {
            PLAIN,
            WALL,
            ORE_IRON,
            FOREST
        }
        public TileState State;
        public NodeObject tileObj;
        public Dictionary<string, float> InfluenceVals;

        public float GetInfluence(string key) => (InfluenceVals != null && InfluenceVals.ContainsKey(key)) ? InfluenceVals[key] : 0;

    }
    public Grid2D<Tile> MapTiles { get; set; }

    [Header("Map")]
    public Tilemap TileMapObject;
    public RuleTile WallTile;
    public TileBase FloorTile;
    [Header("Debug")]
    public Tilemap DebugMap;
    public TileBase DebugTile;

    [Space(10)]


    public NodeObject NodePrefab;
    public Vector2Int Size_;
    public PerlinParam WallParams;
    public PerlinParam ForrestParams;
    public PerlinParam OreParams;

    [Space(10)]
    [Header("Influence")]
    public bool showStart, ShowWall, debugui;
    public float influenceMult;
    public GameObject Scout;


    public static class InfluencType
    {
        public static int FOOD = ("food").GetHashCode();
        public static int WALL = ("wall").GetHashCode();
        public static int VISIBLE = ("visible").GetHashCode();
    }

    Vector3Int start, end;
    float wallOff, forrestOff, ironOff;
    Color C_brown = new Color(0.4f, 0.2f, 0.1f);
    List<Node2D<Tile>> path_;
    bool dirty = true;

    InfluenceMapSource ifSource;
    InfluenceMap InfluenceMap;
    void Start()
    {
        GenerateMap();
        GenerateInfluence();
        do
        {
            start = new Vector3Int(UnityEngine.Random.Range(0, MapTiles.Width), UnityEngine.Random.Range(0, MapTiles.Height), 0);
        } while ((MapTiles[start.x, start.y].Data.State.HasFlag(Tile.TileState.WALL)));
        do
        {
            end = new Vector3Int(UnityEngine.Random.Range(0, MapTiles.Width), UnityEngine.Random.Range(0, MapTiles.Height), 0);
        } while ((MapTiles[end.x, end.y].Data.State.HasFlag(Tile.TileState.WALL)));

        DebugMap.SetTile(start, DebugTile);
        DebugMap.SetTileFlags(start, TileFlags.InstantiateGameObjectRuntimeOnly | TileFlags.LockTransform);
        DebugMap.SetColor(start, Color.cyan);
        DebugMap.SetTile(end, DebugTile);
        DebugMap.SetTileFlags(end, TileFlags.InstantiateGameObjectRuntimeOnly | TileFlags.LockTransform);
        DebugMap.SetColor(end, Color.red);

        Scout.transform.position = start;
        Influence(MapTiles[start.x, start.y], 20, 10, 0.2f, InfluencType.VISIBLE, false);
        PathFind(end.x, end.y);

        GenerateDebugMap();

        InvokeRepeating("TickUpdate", 0, 0.5f);
    }

    void TickUpdate()
    {
        Vector2 pos = QueryMin(start, 20, InfluencType.VISIBLE);
        start.x = (int)pos.x;
        start.y = (int)pos.y;
        Scout.transform.position = start;
        Influence(MapTiles[start.x, start.y], 20, 10, 0.2f, InfluencType.VISIBLE, false);
    }

    public Vector2 QueryMin(Vector3Int center, int range, int type)
    {
        int xMin = (int)Mathf.Max(0, (center.x - range));
        int xMax = (int)Mathf.Min(InfluenceMap.Width - 1, (center.x + range));
        int yMin = (int)Mathf.Max(0, (center.y - range));
        int yMax = (int)Mathf.Min(InfluenceMap.Height - 1, (center.y + range));
        Vector2 pos = Vector2.one * -1;
        float min = Mathf.Infinity;
        for (int x = xMin; x < xMax; x++)
        {
            for (int y = yMin; y < yMax; y++)
            {
                if (MapTiles[x, y].Data.State.HasFlag(Tile.TileState.WALL)) continue;
                var val = InfluenceMap.GetValueAt(x,y,type);
                if (val < min)
                {
                    min = val;
                    pos = new Vector2(x, y);
                }
            }
        }
        return pos;
    }


    void GenerateMap()
    {
        UnityEngine.Random.InitState(seed ? 42069 : (int)System.DateTime.Now.Ticks);
        wallOff = UnityEngine.Random.value * 10000;
        forrestOff = UnityEngine.Random.value * 10000;
        ironOff = UnityEngine.Random.value * 10000;

        MapTiles = new Grid2D<Tile>(Size_.x, Size_.y, GridDT.Traversal.ALL);
        for (int x = 0; x < MapTiles.Width; x++)
        {
            for (int y = 0; y < MapTiles.Height; y++)
            {

                MapTiles[x, y] = new Node2D<Tile>(x, y)
                {
                    Data = new Tile()
                    {
                        InfluenceVals = new Dictionary<string, float>(),
                        //tileObj = Instantiate(NodePrefab, new Vector3(x, y), Quaternion.identity, this.transform)
                    }
                };


                var sampleXPos = ((x / (float)Size_.x) * WallParams._Scale);
                var sampleYPos = ((y / (float)Size_.y) * WallParams._Scale);
                float rndWall = Mathf.PerlinNoise(sampleXPos + wallOff, sampleYPos + wallOff);

                sampleXPos = ((x / (float)Size_.x) * ForrestParams._Scale);
                sampleYPos = ((y / (float)Size_.y) * ForrestParams._Scale);
                float rndForrest = Mathf.PerlinNoise(sampleXPos + forrestOff, sampleYPos + forrestOff);

                sampleXPos = ((x / (float)Size_.x) * OreParams._Scale);
                sampleYPos = ((y / (float)Size_.y) * OreParams._Scale);
                float rndOre = Mathf.PerlinNoise(sampleXPos + ironOff, sampleYPos + ironOff);

                bool isWall = rndWall > WallParams.cutoff || x == 0 || y == 0 || x == Size_.x - 1 || y == Size_.y - 1 ? true : false;
                bool isForrest = rndForrest > ForrestParams.cutoff;
                bool isOre = rndOre > OreParams.cutoff;


                if (isWall)
                {
                    MapTiles[x, y].Data.State = Tile.TileState.WALL;
                    TileMapObject.SetTile(new Vector3Int(x, y, 0), WallTile);
                    //TileMapObject.SetColor(new Vector3Int(x, y, 0), Color.red);
                    //MapTiles[x, y].Data.tileObj.Image_.color = C_brown;
                }
                else
                {
                    TileMapObject.SetTile(new Vector3Int(x, y, 0), FloorTile);
                    if (isForrest)
                    {
                        //MapTiles[x, y].Data.tileObj.Image_.color = Color.green;
                    }
                    else if (isOre)
                    {
                        MapTiles[x, y].Data.State = Tile.TileState.ORE_IRON;
                        //MapTiles[x, y].Data.tileObj.Image_.color = Color.gray;
                    }
                    else
                    {
                        // MapTiles[x, y].Data.tileObj.Image_.color = Color.white;

                    }
                }
            }
        }

        for (int x = 1; x < MapTiles.Width - 1; x++)
        {
            for (int y = 1; y < MapTiles.Height - 1; y++)
            {
                if (MapTiles[x, y].Data.State.HasFlag(Tile.TileState.WALL)) continue;

                if (!MapTiles[x - 1, y].Data.State.HasFlag(Tile.TileState.WALL)) MapTiles[x, y].ConnectNode(MapTiles[x - 1, y]);

                if (!MapTiles[x, y - 1].Data.State.HasFlag(Tile.TileState.WALL)) MapTiles[x, y].ConnectNode(MapTiles[x, y - 1]);

                if (!MapTiles[x - 1, y - 1].Data.State.HasFlag(Tile.TileState.WALL)) MapTiles[x, y].ConnectNode(MapTiles[x - 1, y - 1]);

                if (!MapTiles[x - 1, y + 1].Data.State.HasFlag(Tile.TileState.WALL)) MapTiles[x, y].ConnectNode(MapTiles[x - 1, y + 1]);
            }
        }
    }

    void GenerateInfluence()
    {
        InfluenceMap = new InfluenceMap(MapTiles.Width, MapTiles.Height);
        for (int x = 1; x < MapTiles.Width - 1; x++)
        {
            for (int y = 1; y < MapTiles.Height - 1; y++)
            {

                if (MapTiles[x, y].Data.State != Tile.TileState.WALL)
                {
                    var noOfWalls = 0;
                    for (int _xIX = x - 1; _xIX <= x + 1; _xIX++)
                    {
                        for (int _yIx = y - 1; _yIx <= y + 1; _yIx++)
                        {
                            if (_xIX == x && _yIx == y) continue;
                            if (MapTiles[_xIX, _yIx].Data.State == Tile.TileState.WALL) ++noOfWalls;
                        }
                    }
                    Influence(MapTiles[x, y], noOfWalls, 2, 0.25f, InfluencType.WALL, true);
                }
            }
        }
    }

    void PathFind(int x, int y)
    {
        var paramIn = new AStarSolver.AStarParamIn<Node2D<Tile>>
        {
            StartNode = MapTiles[start.x, start.y],
            EndNode = MapTiles[x, y],
            CalculateHeuristicCost = (Node2D<Tile> node, Node2D<Tile> goalNode) =>
            {
                return (goalNode.Position - node.Position).magnitude + ((node.Data.GetInfluence("space") + (node.Data.GetInfluence("food") * 2)) * influenceMult);
            },
            equalityComparer = EqualityComparer<Node2D<Tile>>.Default,
            CalculateEdgeCost = (IEdge<Node2D<Tile>> edge) =>
            {
                return edge.EdgeCost;
            }
        };
        var paramOut = AStarSolver.SolveViaAStar(paramIn);
        if (paramOut.FoundPath)
        {
            foreach (var item in paramOut.Path)
            {
                var pos = new Vector3Int((int)item.From.Position.x, (int)item.From.Position.y, 0);
                Debug.DrawLine(item.From.Position, item.To.Position, Color.yellow, 10);
            }
        }
    }
    public void Influence(Node2D<Tile> tile, float amount, float range, float dropOff, int key, bool isStatic)
    {
        if (amount == 0) return;
        var visited = new List<Node2D<Tile>>();
        var toExplore = new Queue<Node2D<Tile>>();
        toExplore.Enqueue(tile);

        while (toExplore.Count > 0)
        {
            var exporeNode = toExplore.Dequeue();
            var dist = (exporeNode.Position - tile.Position).magnitude;
            if (dist > range) continue;
            var newAmt = Mathf.Lerp(amount, 0, dist / range);
            if (isStatic) InfluenceMap.AddInfluenceStatic((int)exporeNode.Position.x, (int)exporeNode.Position.y, key, newAmt);
            else InfluenceMap.AddInfluence((int)exporeNode.Position.x, (int)exporeNode.Position.y, key, newAmt);
            visited.Add(exporeNode);

            foreach (var node in exporeNode.GetConnectedNodes())
            {
                if (!visited.Contains(node) && !toExplore.Contains(node))
                {
                    toExplore.Enqueue(node);
                }
            }
        }
    }

  

    void RefreshInfluenceMap()
    {
        if (ifSource != null)
        {
            InfluenceMap.ClearMap();

            var visited = new List<Node2D<Tile>>();
            var toExplore = new Queue<Node2D<Tile>>();
            var tile = MapTiles[(int)ifSource.Center.x, (int)ifSource.Center.y];
            if (tile.Data.State == Tile.TileState.WALL) return; 
            toExplore.Enqueue(tile);

            while (toExplore.Count > 0)
            {
                var exporeNode = toExplore.Dequeue();
                var dist = (exporeNode.Position - tile.Position).magnitude;
                if (dist > ifSource.Radius) continue;
                var newAmt = Mathf.Lerp(20, 0, dist / ifSource.Radius);
                if (newAmt > 0) InfluenceMap.AddInfluence((int)exporeNode.Position.x, (int)exporeNode.Position.y, ifSource.Type,newAmt);
                visited.Add(exporeNode);

                foreach (var node in exporeNode.GetConnectedNodes())
                {
                    if (!visited.Contains(node) && !toExplore.Contains(node))
                    {
                        toExplore.Enqueue(node);
                    }
                }
            }
        }
    }



#if UPDATE
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = (int)Mathf.Clamp(( pos.x), 0, MapTiles.Width);
            int y = (int)Mathf.Clamp(( pos.y), 0, MapTiles.Height);
            // Influence(MapTiles[x, y], 20, 8, 0.1f, InfluencType.FOOD);
            ifSource = new InfluenceMapSource()
            {
                Center = MapTiles[x, y].Position,
                InfluenceVals = null,
                Decay = 0,
                Radius = 8,
                Type = InfluencType.FOOD
            };
            dirty = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = (int)Mathf.Clamp(((MapTiles.Width / 2) + pos.x), 0, MapTiles.Width);
            int y = (int)Mathf.Clamp(((MapTiles.Height / 2) + pos.y), 0, MapTiles.Height);
            PathFind(x, y);
            dirty = true;
        }
        
        RefreshInfluenceMap();
       
        if (ifSource != null)
        {
            //ifSource.Center.x =  50 + Mathf.Sin(Time.time) * 25;

            //ifSource.Center.y = 50 + Mathf.Cos(Time.time) * 25;
            //InfluenceMap.isDirty = true;
        }

    }

#endif
    private void LateUpdate()
    {
        if (debugui)
        {

            foreach (var tile in MapTiles.gridData)
            {
                int x = (int)tile.Position.x; int y = (int)tile.Position.y;
                float foodInfluence = InfluenceMap.GetValueAt(x,y, InfluencType.FOOD);
                float wallInfluence = InfluenceMap.GetValueAt(x, y, InfluencType.WALL);
                float ScoutInfluence = InfluenceMap.GetValueAt(x, y, InfluencType.VISIBLE);
                var pos = new Vector3Int(x,y, 0);
                //if(wallInfluence > 0) DebugMap.SetColor(pos, new Color(0, 0, wallInfluence / 5, wallInfluence > 0 ? 1 : 0));
                if (foodInfluence > 0) DebugMap.SetColor(pos, new Color(0, 0, foodInfluence / 20 , foodInfluence> 0? 1 : 0 ));
                if (ScoutInfluence > 0) DebugMap.SetColor(pos, new Color(0, 0, ScoutInfluence / 20, ScoutInfluence > 0 ? 1 : 0));

            }

            // MapTiles[start.x, start.y].Data.tileObj.Image_.color = Color.cyan;
        }


        // path_[path_.Count - 1].Data.tileObj.Image_.color = Color.cyan;
        // path_[0].Data.tileObj.Image_.color = Color.red;

    }


    private void GenerateDebugMap()
    {
        if (debugui)
        {
            for (int x = 0; x < MapTiles.Width; x++)
            {
                for (int y = 0; y < MapTiles.Height; y++)
                {
                    var pos = new Vector3Int(x, y, 0);
                    DebugMap.SetTile(pos, DebugTile);
                    DebugMap.SetTileFlags(pos, TileFlags.InstantiateGameObjectRuntimeOnly | TileFlags.LockTransform);
                    DebugMap.SetColor(pos, Color.clear);
                }
            }
        }
    }
}
