using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateSet = GoapWorldState;
public class Goal
{
    public string Name { get;  set; }
    public StateSet GoalState { get;  set; }
    public Goal() { }
    public Goal(string _goalName , StateSet _endState)
    {
        Name = _goalName;
        GoalState = _endState;
    }  

    public virtual float GetPriority()
    {
        return 0;
    }

    public virtual bool  IsValid() {
        return true;
    }
}


public struct WorldState
{
    public StateSet Data;
}

public struct Plan
{
    List<ActionGOAP> Action;
}