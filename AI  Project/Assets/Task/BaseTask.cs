using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public  abstract class BaseTask {
    public abstract Task<bool> Execute(CancellationToken token);
}

