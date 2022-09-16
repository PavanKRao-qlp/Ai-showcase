using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using StateSet = GoapWorldState;
public class ActionGOAP
{
    public string _ActionName;
    public StateSet Satisfies;
    public StateSet Requires;
    public int Cost {  set; get; } = 0;
    public string DynamicKey { set; get; } = null;

   

    private List<ActionGOAP> requiredAction;

    public ActionGOAP(string name)
    {
        _ActionName = name;
        Satisfies = new StateSet();
        Requires = new StateSet();
        requiredAction = new List<ActionGOAP>();
    }

    public virtual bool IsValid()
    {
        return true;
    }

    public string GetUID()
    {
        return _ActionName;
    }

    public void ResetConnections()
    {
        requiredAction.Clear();
    }

    public void AddConnections(ActionGOAP action)
    {
        requiredAction.Add(action);
    }


   
}
