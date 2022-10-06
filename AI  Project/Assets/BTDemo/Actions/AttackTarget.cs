using System;
using UnityEngine;

public class AttackTarget : TaskBTNode
{
    private EnemyAi enemyComponent;
    public AttackTarget() { }
    public override void OnEnter()
    {
        enemyComponent = BT?.Agent?.GameObject?.GetComponent<EnemyAi>() ?? null;
        enemyComponent.TryAttack();
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    { }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        if (enemyComponent == null) return IBTNode.ReturnStatus.FAILURE;
        return IBTNode.ReturnStatus.SUCCESS;
    }

    public override void Abort()
    {
        throw new NotImplementedException();
    }
}
