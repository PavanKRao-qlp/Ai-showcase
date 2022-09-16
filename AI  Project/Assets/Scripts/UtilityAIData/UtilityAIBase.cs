using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    public class AIAgentContext
    {
        private UtilityAgentData agentData;
        private List<IAction> Actions;
        private List<IInput> Inputs;
        public AIAgentContext(UtilityAgentData agentData)
        {
            this.agentData = agentData;
            Actions = new List<IAction>();
            Inputs = new List<IInput>();
        }
    }

    public class Action : IAction {
        private UtilityActionData actionData;

        public Action(UtilityActionData actionData)
        {
            this.actionData = actionData;
            new WaitForSeconds(3);
        }

        void IAction.Do()
        {
        }
        float IAction.GetScore()
        {
            return 0;
        }
    };
}
