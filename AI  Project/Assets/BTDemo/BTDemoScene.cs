using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDemoScene : MonoBehaviour
{
    public AIManager AImanager;
    public List<EnemyAi> EnemyAiObjs;

    void Awake()
    {
        foreach (var agent in EnemyAiObjs)
        {
            agent.AIManagerRef = AImanager;
        }
      
    }

    private void Start()
    {
        foreach (var agent in EnemyAiObjs)
        {
            AImanager.AddAgent(agent);
        }     
    }
}
