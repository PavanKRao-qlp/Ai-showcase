using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToPosition : TaskBTNode
{
    private NavMeshAgent navMeshAgent;
    private Vector3 goToPos;
    private bool foundPos = false;

    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
        if (navMeshAgent) navMeshAgent.destination = navMeshAgent.transform.position;
    }

    public override void OnEnter()
    {
        navMeshAgent = BT?.Agent?.GameObject?.GetComponent<NavMeshAgent>() ?? null;
        goToPos = BT.Blackboard.GetEntity(BT.Agent.Id).goToPos;
        foundPos = true;
        if (navMeshAgent) navMeshAgent.destination = goToPos;
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        foundPos = false;
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        if (navMeshAgent == null || !foundPos) return IBTNode.ReturnStatus.FAILURE;
        goToPos = BT.Blackboard.GetEntity(BT.Agent.Id).goToPos;
        if (navMeshAgent && navMeshAgent.destination != goToPos)
            navMeshAgent.destination = goToPos;
        Debug.DrawLine(goToPos + Vector3.up, BT.Agent.GameObject.transform.position + Vector3.up, Color.cyan,2);
        return (BT.Agent.GameObject.transform.position - navMeshAgent.destination).magnitude <= 0.01f ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.RUNNING;
    }
}
