using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    public float angle { get; private set; }
    private float speed;
    private int sensorCount = 24;
    private float[] distances;
    private float[] radiuses;
    private int noiseX;

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }


    [SerializeField] Transform wallCheck;
    [SerializeField] Transform playerCheck;

    private Vector2 velocityWorkspace;

    public virtual void Start()
    {
        noiseX = 0;
        angle = 0;
        distances = new float[sensorCount];
        speed = entityData.movementSpeed;
        radiuses = new float[] { 0.75f * speed, 1.25f * speed };
        Debug.Log(radiuses[0]);
        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
        for (int i = 0; i < sensorCount; i++)
        {
            var radAngle = (angle + (i * 360 / sensorCount)) * Mathf.Deg2Rad;
            distances[i] = Physics2D.Raycast(wallCheck.position, new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)), entityData.wallCheckDistance * speed, entityData.whatIsGround).distance;
            if (distances[i] == 0)
            {
                distances[i] = entityData.wallCheckDistance * speed;
            }
        }
        DoMove(CheckRadiuses());

    }

    public virtual void SetVelocity()
    {
        float radAngle = angle * Mathf.Deg2Rad;
        velocityWorkspace.Set(Mathf.Cos(radAngle) * speed, Mathf.Sin(radAngle) * speed);
        rb.velocity = velocityWorkspace;
    }
    private int CheckRadiuses()
    {
        for (int j = 0; j < radiuses.Length; j++)
        {
            for (int i = 0; i < sensorCount; i++)
            {
                if (radiuses[j] > distances[i] && distances[i] != 0)
                {
                    return j;
                }
            }
        }
        return -1;

    }
    private void DoMove(int checkFailed)
    {
        if (checkFailed == -1)
        {
            float addangle = (Mathf.PerlinNoise(noiseX * 0.01f, 0.0f) - 0.5f) * 1.5f;
            noiseX++;
            angle = (angle + addangle) % 360;
            rb.rotation = angle;

            SetVelocity();
            aliveGO.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        if (checkFailed == 0)
        {
            aliveGO.GetComponent<SpriteRenderer>().color = Color.green;
            float addangle = FindTurnSide();
            angle = (angle + addangle) % 360;
            rb.rotation = angle;

            SetVelocity();
        }
        if (checkFailed == 1)
        {
            aliveGO.GetComponent<SpriteRenderer>().color = Color.red;
            float addangle = FindTurnSide() * 0.2f;
            angle = (angle + addangle) % 360;
            rb.rotation = angle;

            SetVelocity();
        }
    }
    private int FindTurnSide()
    {
        float leftSide = 0;
        float rightSide = 0;
        for (int i = 0; i < sensorCount; i++)
        {
            if (i < sensorCount / 2)
            {
                leftSide += distances[i];
            }
            else if (i > sensorCount / 2)
            {
                rightSide += distances[i];
            }
        }
        if (leftSide / (distances.Length / 2) < radiuses[0])
        {
            return -10;
        }
        else if (rightSide / (distances.Length / 2) < radiuses[0])
        {
            return 10;
        }
        if (leftSide > rightSide)
        {
            return 4;
        }
        else
        {
            return -4;
        }
    }

    public virtual bool CheckPlayerInRange()
    {
        Debug.Log(Physics2D.Raycast(playerCheck.position, GameObject.FindGameObjectWithTag("Fish").transform.position, entityData.playerDetectRange, entityData.whatIsPlayer).distance);
        return Physics2D.Raycast(playerCheck.position, GameObject.FindGameObjectWithTag("Fish").transform.position, entityData.playerDetectRange, entityData.whatIsPlayer);
    }
}
