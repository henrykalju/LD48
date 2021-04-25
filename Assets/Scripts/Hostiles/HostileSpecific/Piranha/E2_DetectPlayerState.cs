using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DetectPlayerState : DetectPlayerState
{
    private Hostile2 hostile;

    public E2_DetectPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectPlayerState stateData, Hostile2 hostile) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.hostile = hostile;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (timedOut)
        {
            hostile.fishInRange = null;
            stateMachine.ChangeState(hostile.moveState);
        }
        else if (isReadyToInteract)
        {
            stateMachine.ChangeState(hostile.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
