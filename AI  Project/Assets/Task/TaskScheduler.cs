using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class TaskScheduler
{
    public delegate void TaskCallback(bool success);
    private static TaskScheduler instance;
    private List<TaskQueue> RunningTasks;
    private CancellationTokenSource mainCTS;
    private TaskScheduler()
    {
        RunningTasks = new List<TaskQueue>();
        mainCTS = new CancellationTokenSource();
    }
    public  static TaskScheduler Instance
    {
        get
        {
            if (instance == null) { instance = new TaskScheduler(); }
            return instance;
        }
    }



    public static void Start(TaskQueue taskQueue, TaskCallback callback) {
        Instance.RunningTasks.Add(taskQueue);
        _ = Schedule(taskQueue, callback);
    }

    public static async Task Schedule(TaskQueue tasks, TaskCallback callback)
    {
        while (tasks.Count > 0)
        {
            var task = tasks.GetTask();
            try
            {
                if (await task.Execute(instance.mainCTS.Token))
                {
                    tasks.Dequeue();
                }
                else
                {
                    callback(false);
                }
            }
            catch (System.Exception e)
            {
                callback(false);
                throw e;
            }
        }
        callback(true);
    }

    public static void KillAllTasks()
    {
        instance.mainCTS.Cancel();
        instance.mainCTS.Dispose();

    }
}
