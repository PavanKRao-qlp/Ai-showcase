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
    private Dictionary<string, dynamic> data; 
    public Blackboard()
    {
        data = new Dictionary<string, dynamic>();
    }
    public void Start()
    {
        //var personData = new JObject() { { "Name", "Pavan" }, { "age", 25 }, { "level", 50 } };
        //var personData2 = new JObject() { { "Name", "Pavan" }, { "age", 25 }, { "level", 50 } };
        //var weaponData = new JObject() { { "Name", "Sword" }, { "damage", "50" }, {"levelReq" , 20 } };
        //data.Add("Pavan", personData);
        //data.Add("Pavan2", personData2);
        //data.Add("Sword", weaponData);
        //string Json = JsonConvert.SerializeObject(data);
        //var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(Json);
        //File.WriteAllText(Application.dataPath + "/data.json", Json);
        //bool isTrue = data["Pavan"].Equals(data["Pavan2"]);
        //Debug.Log(isTrue);     
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
    /// <summary>
    /// Very expensive , use with caution!
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "";//  return JsonConvert.SerializeObject(data, Formatting.Indented);
    }
}
