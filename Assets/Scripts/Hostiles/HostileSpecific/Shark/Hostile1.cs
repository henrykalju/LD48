using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile1 : Entity
{
    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_DetectPlayerState detectPlayerState { get; private set; }
    public E1_AttackState AttackState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_DetectPlayerState detectPlayerData;
    [SerializeField] private D_AttackState attackData;

    public override void Start()
    {
        base.Start();

        moveState = new E1_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E1_IdleState(this, stateMachine, "idle", idleStateData, this);
        detectPlayerState = new E1_DetectPlayerState(this, stateMachine, "detected", detectPlayerData, this);
        AttackState = new E1_AttackState(this,stateMachine, "attack",attackData,this);

        stateMachine.Initialize(moveState);
    }
}
