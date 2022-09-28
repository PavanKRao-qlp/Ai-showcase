using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDemoScene : MonoBehaviour
{
    public AIManager AImanager;
    public EnemyAi EnemyAiObj;
    // Start is called before the first frame update
    void Awake()
    {
        EnemyAiObj.AIManagerRef = AImanager;
        AImanager.AddAgent(EnemyAiObj);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
