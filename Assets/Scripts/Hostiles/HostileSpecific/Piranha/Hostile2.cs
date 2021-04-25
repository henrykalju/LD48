using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile2 : Entity
{
    public E2_MoveState moveState { get; private set; }
    public E2_DetectPlayerState detectPlayerState { get; private set; }
    public E2_AttackState AttackState { get; private set; }
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_DetectPlayerState detectPlayerData;
    [SerializeField] private D_AttackState attackData;

    public override void Start()
    {
        base.Start();

        moveState = new E2_MoveState(this, stateMachine, "move", moveStateData, this);
        detectPlayerState = new E2_DetectPlayerState(this, stateMachine, "detected", detectPlayerData, this);
        AttackState = new E2_AttackState(this,stateMachine, "attack",attackData,this);

        stateMachine.Initialize(moveState);
    }
}
