using GridDT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMap : MonoBehaviour
{
    [Space(10)]
    public Tilemap TilemapLayer_Base;
    public Transform Layer_Units;
    public TileBase TileSprite_Grass;
    public TileBase TileSprite_Wall;

    [Header("debug")]
    public Tilemap TilemapLayer_Debug;
    public TileBase TileSprite_Debug;

    public void DrawWorld(Grid2D<WorldCell> worldGrid)
    {
        for (int x = 0; x < worldGrid.Width; x++)
        {
            for (int y = 0; y < worldGrid.Height; y++)
            {
                switch (worldGrid[x, y].Data.TraversableState)
                {
                    case WorldCell.TraversableStateEnum.TRAVERSABLE:
                        TilemapLayer_Base.SetTile(new Vector3Int(x, y, 0), TileSprite_Grass);
                        break;
                    case WorldCell.TraversableStateEnum.UNTRAVERSABLE:
                        TilemapLayer_Base.SetTile(new Vector3Int(x, y, 0), TileSprite_Wall);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public Vector3Int ConvertWorldToCell(Vector3 position)
    {
        return TilemapLayer_Base.WorldToCell(position);
    }

   

    public void Update()
    {
        //if (Input.GetMouseButton(1))
        //{
        //    var WMPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    WMPos.z = 0;
        //    var CPos = TilemapLayer_Base.WorldToCell(WMPos);
        //    Debug.Log(CPos);
        //    TilemapLayer_Base.SetTileFlags(CPos, TileFlags.InstantiateGameObjectRuntimeOnly | TileFlags.LockTransform);
        //    TilemapLayer_Base.SetColor(CPos, Color.red);
        //}
    }
}
