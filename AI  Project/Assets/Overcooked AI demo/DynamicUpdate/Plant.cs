using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public enum PlantState
    {
        SEED,
        SAPLING,
        GROWN,
        FRUIT,
        DEAD
    }
    public float GrowthRate = 0;
    [SerializeField]
    private float sapplingStateGrowthRequired, grownStateGrowthRequired, fruitStateGrowthRequired;
    private PlantState plantState;
    private float growth;

    private void Start()
    {
        plantState = PlantState.SEED;
        growth = 0;
    }
    // Update is called once per frame
    void Update()
    {
        growth += GrowthRate * Time.deltaTime;
        CheckAndUpdatePlantState();
    }

    void CheckAndUpdatePlantState()
    {
        if (plantState == PlantState.DEAD) return;
        if (plantState == PlantState.SEED && growth > sapplingStateGrowthRequired)
        {
            SetPlantStateAndUpdateWorld(PlantState.SAPLING);
            return;
        }
        if (plantState == PlantState.SAPLING && growth > grownStateGrowthRequired)
        {
            SetPlantStateAndUpdateWorld(PlantState.GROWN);
            return;
        }
        if (plantState == PlantState.GROWN && growth > fruitStateGrowthRequired)
        {
            SetPlantStateAndUpdateWorld(PlantState.FRUIT);
            return;
        }
    }

    public void SetPlantStateAndUpdateWorld(PlantState state)
    {
        plantState = state;
        PlantTest.GameManager.Instance.UpdateWorldState(PlantTest.WorldStateKeys.PLANT_STATE, state);
    }
}
