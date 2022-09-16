using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAgent : MonoBehaviour
{
    // Start is called before the first frame update
    public UtilityAI.UtilityAgentData AgentData;
    UtilityAI.AIAgentContext aIAgentBase_;

    void Start()
    {
        aIAgentBase_ = new UtilityAI.AIAgentContext(AgentData); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
