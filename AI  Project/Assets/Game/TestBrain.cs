using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBrain : MonoBehaviour
{
    PlanScheduler _planScheduler;
    WorldState _worldState;
    Goal TestGoal;

    public NPC_Agent Agent;
    public Vector3 ToMove;
    public bool Move = false;
    // Start is called before the first frame update
    void Start()
    {
        _planScheduler = new PlanScheduler();
        _worldState = new WorldState()
        {
           // Data = new HashSet<KeyValuePair<string, object>>()
        };
        InvokeRepeating("Think", 0.1f, 0.1f);
    }

    void Think()
    {
        // Observer.Update();
        if (Move)
        {
            //TestGoal = new Goal("atX",
            //new HashSet<KeyValuePair<string, object>>() {
            //    new KeyValuePair<string, object>("agentAt", ToMove) 
            //});

            var plan = PlannerGOAP.GetPlan(TestGoal, Agent.Actions, _worldState.Data);
            if(plan != null)
            {
              //  PlanScheduler.EnactPlan(Agent, plan);
            }
        }
       
    }
}
