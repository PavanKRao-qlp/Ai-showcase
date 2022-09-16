using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct InfluenceParams
{
    public float Amount;
    public float Decay;
    public float Momentum;
}

public class InfluenceMapSource
{
    public int Type;
    public float Radius;
    public float Decay;
    public Vector2 Center;
    public float Amount;
    public Dictionary<int, float> InfluenceVals;
}

public struct InfluenceCell
{
    public Vector2 Position;
    private Dictionary<int, float> influences { get; set; }
    public float GetInfluence(int type)
    {
        if(influences != null)
        {
            return influences.ContainsKey(type) ? influences[type] : 0;
        }
        return 0;
    }
    public void SetInfluence(int type , float amount)
    {
        if (influences == null) influences = new Dictionary<int, float>();
        influences[type] = amount;
    }
    public void AddInfluence(int type, float amount)
    {
        if (influences == null) influences = new Dictionary<int, float>();
        if (influences.ContainsKey(type)) { influences[type] += amount; 
        }
        else influences[type] = amount;
    }

    public void ClearInfluence()
    {
        if (influences != null)
            influences.Clear();
    }

    public void DecayInfluence()
    {
        if (influences != null)
        {
            for (int i = 0; i < influences.Count; i++)
            {
                var x = influences.ElementAt(i).Key; 
                influences[x] = Mathf.Lerp(influences[x], 0,0.75f) ;
                if (influences[x] < 0.01f) influences[x] = 0;
            }

           
        }
    }
}

public class InfluenceMap
{
    public int Width { get; private set; }
    public int Height{ get; private set; }
    private InfluenceCell[,] Map; 
    private InfluenceCell[,] StaticMap;
    public bool isDirty = false;

    public InfluenceMap(int width, int height)
    {
        Width = width;
        Height = height;
        Map = new InfluenceCell[width, height];
        StaticMap = new InfluenceCell[width, height];
    }


  public  void AddInfluence(int x, int y, int type, float amount)
    {
        Map[x, y].AddInfluence(type, amount);
    }

    public void AddInfluenceStatic(int x, int y, int type, float amount)
    {
        if(amount > 0) StaticMap[x, y].AddInfluence(type, amount);
    }

    public void ClearMap()
    {
        for (int x_ = 0; x_ < Width; x_++)
        {
            for (int y_ = 0; y_ < Height; y_++)
            {
                Map[x_, y_].ClearInfluence();
            }
        }
    }
    
    public float GetValueAt(int x, int y , int type)
    {
        return Map[x, y].GetInfluence(type) + StaticMap[x, y].GetInfluence(type);
    }

    private InfluenceCell this[int x,int y]
    {
        get {
            return Map[x,y];
        }
    }
}
