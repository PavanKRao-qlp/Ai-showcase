using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{

    [System.Serializable]
    public struct PerlinParam
    {
        public float _Scale;
        [Range(0, 1)]
        public float cutoff;
    }


    [Header("Worldbuilding")]
    [SerializeField] string Seed = null;
    [SerializeField] WorldManager WorldManager = null;

    [Header("temp")]
    [SerializeField] TestBrain t_Brain = null;
    [SerializeField] NPC_Agent workerPrefab = null;
    [SerializeField] GameObject collectPrefab = null;


    private static GameManager instance;
    public static GameManager Instance => instance;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        UnityEngine.Random.InitState(!string.IsNullOrEmpty(Seed) ? Seed.GetHashCode() : (int)System.DateTime.Now.Ticks);
        StartGame();
    }

    void StartGame()
    {
        WorldManager.GenerateNewWorld();
        //AIManager initialize
        //t_Brain.WorldManagerRef = WorldManager;
        StartScenario();

        //t GetRandomStaring();
        //t CreateAgent();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var Cpos = WorldManager.WorldMapObject.ConvertWorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Cpos.z = 0;
            WorldManager.WorldGrid[Cpos.x, Cpos.y].Data.Type = WorldCell.CellType.COLLECTIBLE;
            GameObject.Instantiate(collectPrefab, Cpos, Quaternion.identity, WorldManager.WorldMapObject.Layer_Units);
        }
    }

    private void StartScenario()
    {
        var start = Vector3Int.zero;
        do
        {
             start = new Vector3Int(UnityEngine.Random.Range(0, WorldManager.WorldGrid.Width), UnityEngine.Random.Range(0, WorldManager.WorldGrid.Height), 0);
        } while ((WorldManager.WorldGrid[start.x, start.y].Data.TraversableState.HasFlag(WorldCell.TraversableStateEnum.UNTRAVERSABLE)));
        var newWorker = GameObject.Instantiate(workerPrefab, start , Quaternion.identity,  WorldManager.WorldMapObject.Layer_Units);
       

    }
}
