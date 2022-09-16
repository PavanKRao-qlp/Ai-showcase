using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ActionBase
{
    private NPC_Agent actingAgent;
    public System.Action Completed;
    public System.Action Failed;
    
    public NPC_ActionBase(NPC_Agent agent)
    {
        actingAgent = agent;
    }

    public virtual void StartAction()
    {

    }

}
