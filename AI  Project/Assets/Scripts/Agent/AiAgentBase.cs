using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgentBase : MonoBehaviour, IAgentGOAP
{
    public List<ActionGOAP> Actions;
    public virtual void EnactPlan()
    {

    }
}
