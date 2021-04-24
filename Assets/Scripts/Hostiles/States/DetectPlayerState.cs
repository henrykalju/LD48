using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerState : State
{
    protected D_DetectPlayerState stateData;
    protected bool isReadyToInteract;
    public DetectPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectPlayerState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        isReadyToInteract = false;
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

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
