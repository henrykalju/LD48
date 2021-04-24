using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected D_AttackState stateData;
    protected ColliderScript colliderScript;
    protected bool isTired;
    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_AttackState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.colliderScript = entity.transform.Find("Alive").GetComponent<ColliderScript>();
    }

    public override void Enter()
    {
        base.Enter();
        isTired = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.chaseDuration)
        {
            isTired = true;
        }
        if(colliderScript.killedCount>= stateData.maxKillCount){
            isTired = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
