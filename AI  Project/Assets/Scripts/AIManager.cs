using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public Blackboard BlackBoard;
    private List<IAgentBT> behaviourTreeAgents;
    public AIManager()
    {
        BlackBoard = new Blackboard();
        behaviourTreeAgents = new List<IAgentBT>();
    }

    public void AddAgent(IAgent agent)
    {
        if (typeof(IAgentBT).IsAssignableFrom(agent.GetType()))
        {
            var agentBT = agent as IAgentBT;
            this.BlackBoard.AddEntity(agentBT.Id);
            behaviourTreeAgents.Add(agentBT);
        }
    }

    public void Update()
    {
        TickActiveBehaviourTreeAgents();
    }
    private void TickActiveBehaviourTreeAgents()
    {
        foreach (var agent in behaviourTreeAgents)
        {
            agent.ActiveBehaviorTree?.Tick();
        }
    }


}
