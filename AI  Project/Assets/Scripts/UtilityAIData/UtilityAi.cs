using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    public interface IAction
    {
        float GetScore();
        void Do();
    }

    public interface IInput
    {
    }

    public enum ConsiderationType
    {
        Boolean,
        Linear,
        InverseLinear,
        Exponent,
        InverseExponent
    }

    internal interface IConsideration
    {
       
        ConsiderationType GetConsiderationType();
        float GetConiderationValue(IUtilityAgent agentContext);
    }

    internal interface IUtilityAgent
    {
        List<IAction> GetActions();
    }

    class UtilityAI
    {
        public static IAction GetBestAction(List<IAction> actionList)
        {
            IAction bestAction = null;
            float bestScore = -1;
            foreach (IAction action_ in actionList)
            {
                float actionScore = action_.GetScore();
                if (bestScore < actionScore)
                {
                    bestScore = actionScore;
                    bestAction = action_;
                }
                Debug.Log($"Action {action_.GetType().Name } : [{actionScore}]");
            }
            Debug.Log($"Chose action {bestAction.GetType().Name } : {bestScore}");
            return bestAction;
        }
    }
}