using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float wallCheckDistance = 10f;
    public float movementSpeed = 5f;
    public float playerDetectRange = 6f;


    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

}
