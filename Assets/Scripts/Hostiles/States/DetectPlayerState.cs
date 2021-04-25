using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerState : State
{
    protected D_DetectPlayerState stateData;
    protected bool isReadyToInteract;
    protected bool timedOut;
    public DetectPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectPlayerState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        timedOut = false;
        isReadyToInteract = false;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        entity.CheckToNotCollideWithWall(new int[] { -1, 0, 1 }, entity.speed);
        if (isReadyToInteract && !entity.CheckPlayerInRange()) {
            timedOut = true;
        }
        if (Time.time >= startTime + stateData.standByTime)
        {
            isReadyToInteract = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
