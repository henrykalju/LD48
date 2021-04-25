using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_AttackState : AttackState
{
    private Hostile1 hostile;
    private float dashTime;
    public E1_AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_AttackState stateData, Hostile1 hostile) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.hostile = hostile;

    }

    public override void Enter()
    {
        hostile.isAttacking = true;
        Dash();
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
            colliderScript.killedCount = 0;
            hostile.fishInRange = null;
            stateMachine.ChangeState(hostile.moveState);
        }
        else
        {
            entity.CheckToNotCollideWithWall(new int[] { 0 }, entity.speed);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (Time.time > dashTime + stateData.dashDuration) {
            Follow();
        }

    }

    private void Follow()
    {
        GameObject fish = hostile.fishInRange;
        if (!fish)
        {
            return;
        }
        float angle = entity.GetAngleFromDir(entity.GetVector3DirToFish(fish));
        entity.rb.rotation = angle;
        //entity.rb.AddForce(entity.GetVector3DirToFish(fish));
        entity.SetVelocity(angle, entity.speed);
    }

    private void Dash()
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
        dashTime = Time.time;
    }
}
