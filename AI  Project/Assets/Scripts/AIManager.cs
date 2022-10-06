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

    private void Start()
    {
        StartCoroutine("TickActiveBehaviourTreeAgents");
    }

    public void AddAgent(IAgent agent)
    {
        if (typeof(IAgentBT).IsAssignableFrom(agent.GetType()))
        {
            var agentBT = agent as IAgentBT;
            behaviourTreeAgents.Add(agentBT);
        }
    }

    public void LateUpdate()
    {
        //TickActiveBehaviourTreeAgents();
    }
    private IEnumerator TickActiveBehaviourTreeAgents()
    {
        while (true)
        {
            foreach (var agent in behaviourTreeAgents)
            {
                agent.ActiveBehaviorTree?.Tick();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }


}
