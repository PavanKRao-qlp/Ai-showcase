using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDemoScene : MonoBehaviour
{
    public AIManager AImanager;
    public EnemyAi EnemyAiObj;

    void Awake()
    {
        EnemyAiObj.AIManagerRef = AImanager;
      
    }

    private void Start()
    {
        AImanager.AddAgent(EnemyAiObj);
    }

    void Update()
    {
        
    }
}
