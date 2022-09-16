using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantTest {
    public class TestAgent : MonoBehaviour
    {
        public List<ActionGOAP> Actions;

        public void Start()
        {
            Actions = new List<ActionGOAP>();
            //Actions.Add(new PlantTest.NpcActionPlantSeed());
           // Actions.Add(new PlantTest.NpcActionWaterSeedling());
            Actions.Add(new PlantTest.NpcActionHarvestPlant());
        }
    }

}