using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_AttackState : AttackState
{
    private Hostile1 hostile;
    public E1_AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_AttackState stateData, Hostile1 hostile) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.hostile = hostile;
        
    }

    public override void Enter()
    {
        hostile.isAttacking = true;
        base.Enter();
    }
    public override void Exit()
    {
        hostile.isAttacking = false;
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isTired){
            colliderScript.killedCount = 0;
            stateMachine.ChangeState(hostile.moveState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //AJA KALA TAGa
        hostile.GoToFish();

    }
}
