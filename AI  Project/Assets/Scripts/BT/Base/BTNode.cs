using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BTNode {
    public enum ReturnStatus
    {
        FAILED,
        RUNNING,
        SUCCESS
    }
    public ReturnStatus Tick();
}
