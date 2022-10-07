//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using UnityEngine;

public class Blackboard 
{

#if UNITY_EDITOR
    public Dictionary<string, ExpandoObject> Data => data;
#endif

    private Dictionary<string, ExpandoObject> data; 
    public Blackboard()
    {
        data = new Dictionary<string, ExpandoObject>();
    }

    public dynamic GetEntity(string id)
    {
        if (data.ContainsKey(id))
        {
            return data[id];
        }
        else return null;
    }

    public void AddEntity(string Id)
    {
        data.Add(Id, new ExpandoObject());
    }
    public override string ToString()
    {
        return "";//  return JsonConvert.SerializeObject(data, Formatting.Indented);
    }
}
