using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentBT : IAgent, IBlackBoardEntity
{
    public abstract BehaviorTree ActiveBehaviorTree { get;}
    public abstract GameObject GameObject { get; }
}
