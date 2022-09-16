using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using StateSet = GoapWorldState;

class GOAPGraph : Graph<GOAPStateNode>
{
    public Dictionary<string, List<ActionGOAP>> actionSatisfySet = new Dictionary<string, List<ActionGOAP>>();
}
public class GOAPStateNode : IEqualityComparer<GOAPStateNode>,INode<GOAPStateNode>
{
    public struct GOAPStateEdge : IEdge<GOAPStateNode>
    {
        public ActionGOAP Action;
        public GOAPStateNode From { get; set; }
        public GOAPStateNode To { get; set; }
        public float EdgeCost => Action.Cost;
    }

    public StateSet State;
    readonly List<ActionGOAP> availableActions;
    List<IEdge<GOAPStateNode>> doableActions;

    readonly string uid;
    public GOAPStateNode(List<ActionGOAP> _actions , StateSet _state)
    {
        State = _state;
        availableActions = _actions;
        doableActions = null;
        uid =  "state-" + string.Join(":",State.OrderBy(x => x.ToString())); 
    }

    public bool TryGetEdge(GOAPStateNode node, out GOAPStateEdge action)
    {
        if (doableActions != null)
        {
            foreach (GOAPStateEdge edge in doableActions)
            {
                if (edge.To.GetUID() == node.GetUID())
                {
                    action = edge;
                    return true;
                }
            }
        }
        action = new GOAPStateEdge();
        return false;
    }

    public int GetUID()
    {

        return uid.GetHashCode();
    }

    public void AddConnectedEdge(IEdge<GOAPStateNode> edge)
    {
        doableActions.Add(edge);
    }

    public List<IEdge<GOAPStateNode>> GetEdges()
    {
        if (doableActions == null)
        {
            doableActions = new List<IEdge<GOAPStateNode>>();
            foreach (ActionGOAP action in availableActions)
            {
                var nextAction = action;
                if (!string.IsNullOrEmpty(action.DynamicKey))
                {
                    object value;
                    if (State.TryFind(action.DynamicKey, out value)) {
                        nextAction = new ActionGOAP(action._ActionName + value.ToString())
                        {
                            Cost = action.Cost,
                            Requires = action.Requires,
                            Satisfies = action.Satisfies,
                            DynamicKey = null
                        };
                        nextAction.Satisfies.UpdateValue(action.DynamicKey, value);
                    }                 

                    availableActions.Add(nextAction);
                }

                if (nextAction.Satisfies.Overlaps(State))
                {
                    var newState = State.Except(nextAction.Satisfies);
                    newState = newState.Union(nextAction.Requires);
                    GOAPStateNode newNode = new GOAPStateNode(availableActions, new StateSet(newState));
                    GOAPStateEdge edge = new GOAPStateEdge
                    {
                        Action = nextAction,
                        From = this,
                        To = newNode
                    };
                    doableActions.Add(edge);
                }
            }
        }
        foreach(var action in doableActions)
        {
            Debug.Log($"pvn: action {((GOAPStateEdge)action).Action._ActionName}");
        }
        return doableActions;
    }

    public List<GOAPStateNode> GetConnectedNodes()
    {
        return null;
    }

   public bool Equals(GOAPStateNode x, GOAPStateNode y)
    {
        return x.State.SetEquals(y.State);
    }

    public int GetHashCode(GOAPStateNode obj)
    {
        return string.Join(":", State.OrderBy(x => x)).GetHashCode();
    }
}
public class ActionGOAPEqualityComparer : IEqualityComparer
{
    private static ActionGOAPEqualityComparer comparer_;
    public static ActionGOAPEqualityComparer Comparer
    {
        get
        {
            if (comparer_ == null) comparer_ = new ActionGOAPEqualityComparer();
            return comparer_;
        }
    }

    public new bool Equals(object current, object goal)
    {
        var currentNode = (GOAPStateNode)current;
        var goalNode = (GOAPStateNode)goal;
        var isEqual = goalNode.State.IsSupersetOf(currentNode.State);
        return isEqual;
    }

    public int GetHashCode(object obj)
    {
        return ((ActionGOAP)obj).GetHashCode();
    }
}

public class PlannerGOAP 
{
    public static List<ActionGOAP> GetPlan(in Goal goal, in List<ActionGOAP> actions_, in StateSet our_state)
    {

        List<ActionGOAP> validActions = new List<ActionGOAP>();
        foreach (ActionGOAP action in actions_)
        {
            if (action.IsValid() && !validActions.Contains(action))
            {
                validActions.Add(action);
            }
        }

        GOAPGraph graph = new GOAPGraph();
        GOAPStateNode playerState = new GOAPStateNode(_actions: validActions, _state: our_state);
        GOAPStateNode goalState   = new GOAPStateNode(_actions: validActions, _state: goal.GoalState);
        graph.AddNode(playerState);
        graph.AddNode(goalState);
        List<ActionGOAP> plan = new List<ActionGOAP>();
        var paramIn = new AStarSolver.AStarParamIn<GOAPStateNode>
        {
            StartNode = goalState,
            EndNode = playerState,
            CalculateHeuristicCost = (GOAPStateNode node, GOAPStateNode goalNode) =>
            {
                int commonElements = goalNode.State.Except(node.State).Count();
                return commonElements;
            },
            CalculateEdgeCost = (IEdge<GOAPStateNode> toEdge) =>
            {
               GOAPStateNode.GOAPStateEdge edge = (GOAPStateNode.GOAPStateEdge)toEdge;
               return edge.EdgeCost;
            },
            equalityComparer = ActionGOAPEqualityComparer.Comparer
        };
        var paramOut = AStarSolver.SolveViaAStar(paramIn);
        if (paramOut.FoundPath)
        {
            foreach (var edge in paramOut.Path)
            {
                var action = ((GOAPStateNode.GOAPStateEdge)edge).Action;
                plan.Add(action);
            }
            return plan;

        }
        return null;
    }
}

