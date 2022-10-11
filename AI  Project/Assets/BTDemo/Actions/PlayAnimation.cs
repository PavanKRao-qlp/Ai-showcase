using System;
using UnityEngine;

public class PlayAnimation : TaskBTNode
{
    private string animationName;
    private float timeElapsed;
    private Animator animator;
    private float animDur;
    private float animStartTime;

    public PlayAnimation(string animName)
    {
        this.animationName = animName;
        timeElapsed = 0;
        animDur = 0;
    }


    public bool CheckIfAnimationCompleted()
    {
        return animDur <= timeElapsed;
    }

    public override void OnEnter()
    {
        animator = this.BT.Agent.GameObject.GetComponentInChildren<Animator>();
        timeElapsed = 0;
        animDur = 0;
        if (animator)
        {
            animator.Play(animationName);
            animDur = 1.5f;
            animStartTime = Time.time;
        }
    }

    public override void OnExit(IBTNode.ReturnStatus status)
    {
        timeElapsed = 0;
        animDur = 0;
    }

    public override IBTNode.ReturnStatus OnUpdate()
    {
        if (animator == null) return IBTNode.ReturnStatus.FAILURE;
        timeElapsed =  Time.time - animStartTime;
        return CheckIfAnimationCompleted() ? IBTNode.ReturnStatus.SUCCESS : IBTNode.ReturnStatus.RUNNING;
    }

    public override void Abort()
    {
        this.status = IBTNode.ReturnStatus.ABORTED;
        animator.StopPlayback();
    }
}
