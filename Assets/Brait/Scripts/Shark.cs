using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    [SerializeField] Rigidbody2D rbody;
    [SerializeField] LayerMask playerCast;
    [SerializeField] LayerMask wallCast;
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float attackSpeed;
    [SerializeField] float detectDistance;
    [SerializeField] float cooldown;
    [SerializeField] int maxKills;

    private float seed;

    private void Start()
    {
        seed = Random.Range(float.MinValue, float.MaxValue);
    }

    private void FixedUpdate()
    {
        
    }

    private void IdleMove()
    {
        Vector3 moveDirection = new Vector3((Mathf.PerlinNoise(Time.time + seed, 0) - 0.5f) * 2, (Mathf.PerlinNoise(0, Time.time + seed) - 0.5f) * 2).normalized * speed;


    }


}
