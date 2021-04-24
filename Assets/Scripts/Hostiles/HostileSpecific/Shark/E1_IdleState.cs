using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_IdleState : IdleState
{
    private Hostile1 hostile;
    public E1_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Hostile1 hostile) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(hostile.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
