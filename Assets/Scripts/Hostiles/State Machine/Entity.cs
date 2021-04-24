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

    public bool isAttacking { get; set; }

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
        if (!isAttacking) {
            for (int i = 0; i < sensorCount; i++)
            {
                var radAngle = (angle + (i * 360 / sensorCount)) * Mathf.Deg2Rad;
                distances[i] = Physics2D.Raycast(wallCheck.position, new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)), entityData.wallCheckDistance * speed, entityData.whatIsGround).distance;
                if (distances[i] == 0)
                {
                    distances[i] = entityData.wallCheckDistance * speed;
                }
            }
            speed = entityData.movementSpeed;
            DoMove(CheckRadiuses());
        }

    }

    public virtual void SetVelocity()
    {
        float radAngle = angle * Mathf.Deg2Rad;
        velocityWorkspace.Set(Mathf.Cos(radAngle) * speed, Mathf.Sin(radAngle) * speed);
        rb.velocity = velocityWorkspace;
    }
    public virtual void SetVelocity(float angle, float speed)
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
        }
        if (checkFailed == 0)
        {
            float addangle = FindTurnSide();
            angle = (angle + addangle) % 360;
            rb.rotation = angle;

            SetVelocity();
        }
        if (checkFailed == 1)
        {
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
        if (!GameObject.FindGameObjectWithTag("Fish"))
        {
            return false;
        }
        GameObject[] fishies = GameObject.FindGameObjectsWithTag("Fish");
        for (int i = 0; i < fishies.Length; i++)
        {
            Vector3 dir = fishies[i].transform.position - playerCheck.position;
            if (Physics2D.Raycast(playerCheck.position, dir, entityData.playerDetectRange, entityData.whatIsPlayer))
            {
                return true;
            }
        }
        return false;
    }

    public virtual GameObject getNearestFish()
    {
        if (!GameObject.FindGameObjectWithTag("Fish"))
        {
            return null;
        }
        GameObject[] fishies = GameObject.FindGameObjectsWithTag("Fish");
        GameObject nearestFish = null;
        float nearestFishDistance = entityData.playerDetectRange;
        for (int i = 0; i < fishies.Length; i++)
        {

            Vector3 dir = fishies[i].transform.position - playerCheck.position;
            float fishDistance = Physics2D.Raycast(playerCheck.position, dir, entityData.playerDetectRange, entityData.whatIsPlayer).distance;

            if (fishDistance < nearestFishDistance && fishDistance != 0)
            {
                nearestFish = fishies[i];
                nearestFishDistance = fishDistance;
            }
        }
        return nearestFish;
    }

    public virtual void CheckIfIsAttacking()
    {
        GameObject fish = getNearestFish();
        if (!fish) {
            isAttacking = false;
            return;
        }
        isAttacking = true;
    }
    public virtual Vector3 GetVector3DirToFish(GameObject fish){
        Vector3 dir = fish.transform.position - aliveGO.transform.position;
        return dir;
    }
    public virtual float GetAngleFromDir(Vector3 dir){
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    } 
}
