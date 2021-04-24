using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackData", menuName = "Data/State Data/Attack State")]
public class D_AttackState : ScriptableObject
{
    public float chaseDuration = 5.0f;
    public int maxKillCount = 10;


}
