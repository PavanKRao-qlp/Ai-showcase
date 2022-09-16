using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantTest
{
   public class NpcActionPlantSeed : ActionGOAP {
       public NpcActionPlantSeed() : base("NpcActionPlantSeed") {
        
        }
    }
    public class NpcActionWaterSeedling : ActionGOAP
    {
        public NpcActionWaterSeedling() : base("NpcActionWaterSeedling")
        {

        }
    }
    public class NpcActionHarvestPlant : ActionGOAP
    {
        public NpcActionHarvestPlant() : base("NpcActionHarvestPlant")
        {
            Requires.Add(WorldStateKeys.PLANT_STATE, Plant.PlantState.FRUIT);
            Satisfies.Add(WorldStateKeys.PLAYER_HAS, "FRUIT");
            Satisfies.Add(WorldStateKeys.PLANT_STATE, Plant.PlantState.GROWN);
        }
    }
}