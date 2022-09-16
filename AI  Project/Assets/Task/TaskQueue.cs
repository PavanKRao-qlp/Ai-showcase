using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class TaskQueue
{
    private string trackingId;
    private Queue<BaseTask> tasks;
    private CancellationTokenSource taskQueueCTS;

    public TaskQueue()
    {
        trackingId = "";
        tasks = new Queue<BaseTask>();
    }
    public TaskQueue(string trackingId)
    {
        this.trackingId = trackingId;
        tasks = new Queue<BaseTask>();
    }
    public int Count => tasks.Count;
    public void Enqueue(BaseTask task)
    {
        tasks.Enqueue(task);
    }
    public BaseTask Dequeue()
    {
        return tasks.Dequeue();
    }
    public BaseTask GetTask()
    {
        return tasks.Peek();
    }

    public void Cancel()
    {
        taskQueueCTS.Cancel();
    }
}
