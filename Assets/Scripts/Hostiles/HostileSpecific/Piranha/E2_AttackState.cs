using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_AttackState : AttackState
{
    private Hostile2 hostile;
    private float dashTime;
    public E2_AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_AttackState stateData, Hostile2 hostile) : base(entity, stateMachine, animBoolName, stateData)
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
        if (isTired)
        {
            hostile.fishInRange = null;
            colliderScript.killedCount = 0;
            stateMachine.ChangeState(hostile.moveState);
        }
        else
        {
            //entity.CheckToNotCollideWithWall(new int[] { 0 }, entity.speed);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Chase();
    }

    private void Chase()
    {
        GameObject fish = hostile.fishInRange;
        if (!fish)
        {
            return;
        }
        float angle = entity.GetAngleFromDir(entity.GetVector3DirToFish(fish));
        entity.rb.rotation = angle;
        //entity.rb.AddForce(entity.GetVector3DirToFish(fish));
        entity.SetVelocity(angle, stateData.attackSpeed);
    }
}
