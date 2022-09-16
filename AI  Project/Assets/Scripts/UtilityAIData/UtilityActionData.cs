using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilityAI
{

    [CreateAssetMenu(fileName = "UtilityActionData", menuName = "Scriptable/utilityAI/UtilityActionData", order = 1)]
    public class UtilityActionData : ScriptableObject
    {
        public List<UtiltyConiderationData.ConsiderationStruct> Coniderations;
        [ContextMenuItem("validate", "Validate")]
        public string BehaviorClass;

        private void Validate()
        {
            var types = System.AppDomain.CurrentDomain.GetTypesWithInterface<IAction>();
        }
    }
}