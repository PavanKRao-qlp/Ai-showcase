using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SimultaneousTask : BaseTask
{
    public BaseTask[] Tasks;
    public override async Task<bool> Execute(CancellationToken token)
    {
        if (Tasks != null && Tasks.Length > 0)
        {
            var taskArray = new Task[Tasks.Length];
            for (int i = 0; i < Tasks.Length; i++)
            {
                var task = Tasks[i].Execute(token);
                taskArray[i] = task;
            }
            await Task.WhenAll(taskArray);
        }
        return true;
    }
}
