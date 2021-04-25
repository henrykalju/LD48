using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;

    // 0 - no 1 - front 2 - right 3 - left
    protected int isDetectingWall;
    protected bool isPlayerInRange;
    protected bool isTired;




    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        isTired = true;
        base.Enter();
        entity.SetVelocity(entity.angle, entity.speed);
        isPlayerInRange = entity.CheckPlayerInRange();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.tiredDuration)
        {
            isTired = false;
        }
        entity.CheckToNotCollideWithWall(new int[] { -1, 0, 1, 2, 3 }, entity.speed);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        isPlayerInRange = entity.CheckPlayerInRange();
    }
}
