using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilityAI
{
    [CreateAssetMenu(fileName = "UtiltyConiderationData", menuName = "Scriptable/utilityAI/UtiltyConiderationData", order = 1)]
    public class UtiltyConiderationData : ScriptableObject
    {
        [System.Serializable]
        public struct ConsiderationStruct
        {
            public UtiltyConiderationData Conideration;
            public float Multiplier;
            public AnimationCurve Curve;
        }
        public List< string> Inputs;
    }
}