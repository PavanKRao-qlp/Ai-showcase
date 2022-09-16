using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantTest
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => instance_;
        private static GameManager instance_;
        public PlantTest.TestAgent Agent;
        World World;
        
        public void Start()
        {
            if (instance_) {
                Destroy(this);
            }
            else
            {
                instance_ = this;
                World = new World();
                StartCoroutine("FindGoal");
            }
        }
        public void UpdateWorldState(string key, object value)
        {
            World.SetState(key, value);
        }

        public IEnumerator FindGoal()
        {
            yield return new WaitForSeconds(5f);
            Goal goal = new Goal()
            {
                Name = "Harvest",
                GoalState = new GoapWorldState() {
                    (WorldStateKeys.PLAYER_HAS , "FRUIT"),
                }
            };
            var plan = PlannerGOAP.GetPlan(in goal, in Agent.Actions, World.WorldStateSet);
            if (plan != null)
            {
                foreach (var action in plan)
                {
                    Debug.Log($"------Action " + action._ActionName);
                }
            }
            else
                StartCoroutine("FindGoal");
        }
    }
    public class WorldStateKeys
    {
        public static string PLANT_STATE = "PLANT_STATE";
        public static string PLAYER_HAS = "PLAYER_HAS";
    };
}