using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_DetectPlayerState : DetectPlayerState
{
    private Hostile1 hostile;

    public E1_DetectPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectPlayerState stateData, Hostile1 hostile) : base(entity, stateMachine, animBoolName, stateData)
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
        if (isReadyToInteract) {
            // TODO: change state to angery.
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
