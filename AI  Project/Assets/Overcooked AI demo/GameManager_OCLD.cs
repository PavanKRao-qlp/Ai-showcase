using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager_OCLD : MonoBehaviour
{
    public RecipeData RecipeData;
    public LevelMap LevelMap;
    public CookAI CookAgent;
    public Vector2Int GoToPos;
    public CookWorld World;

    void Start()
    {
        this.World = new CookWorld();
        LevelMap.GenerateMap();
        LevelMap.SpawnSmartObjects();
        LevelMap.SpawnPlayer();
        StartGame();
    }

    private void StartGame()
    {
        var order = RecipeData.GetRandomRecipe();

    }
    void GenerateGoalState(RecipeData.Recipe recipe)
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var pathCells = this.LevelMap.FindPathToCell(CookAgent.CellPos, GoToPos);
            var path = pathCells.Select(x => new Vector3(x.Position.x,0,x.Position.y)).ToArray();
            var taskQ = new TaskQueue("moveAlongPath");
            taskQ.Enqueue(new FollowPathTask() { Transform = CookAgent.transform, Path = path, Speed = 5 });
            taskQ.Enqueue(new InteractTask() { interactor = CookAgent, interactableObject = LevelMap.Item3 });
            TaskScheduler.Start(taskQ, (success) => {
            });
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    public void OnDestroy()
    {
        TaskScheduler.KillAllTasks();
    }
}
