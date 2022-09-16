using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(fileName = "UtilityAgentData", menuName = "Scriptable/utilityAI/UtilityAgentData", order = 1)]
    public class UtilityAgentData : ScriptableObject
    {
        public List<UtilityActionData> Actions;
    }
}