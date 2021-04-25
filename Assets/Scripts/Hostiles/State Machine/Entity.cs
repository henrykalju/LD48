using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    public bool isAttacking { get; set; }

    public Rigidbody2D rb { get; private set; }
    public GameObject aliveGO { get; private set; }

    public float angle;
    public float speed;
    public int sensorCount = 24;
    public float[] distances;
    public float[] radiuses;
    public int noiseX;
    public float noiseY;
    [SerializeField] public Transform wallCheck;
    [SerializeField] public Transform playerCheck;

    private Vector2 velocityWorkspace;

    public virtual void Start()
    {
        noiseX = 0;
        noiseY = Random.Range(-10f, 10f);
        angle = 0;
        distances = new float[sensorCount];
        speed = entityData.movementSpeed;
        radiuses = new float[] { 0.75f, 1.25f };
        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();

    }
    public virtual void SetVelocity(float angle, float speed)
    {
        float radAngle = angle * Mathf.Deg2Rad;
        velocityWorkspace.Set(Mathf.Cos(radAngle) * speed, Mathf.Sin(radAngle) * speed);
        rb.velocity = velocityWorkspace;
        rb.rotation = angle;
    }
    public int CheckRadiuses(float speed, int[] radiusesToUse)
    {
        for (int j = 0; j < radiuses.Length; j++)
        {
            for (int i = 0; i < sensorCount; i++)
            {
                if (IsElementInList(j, radiusesToUse) && radiuses[j] * speed > distances[i] && distances[i] != 0)
                {
                    return j;
                }
            }
        }
        if (IsElementInList(-1, radiusesToUse))
        {
            return -1;
        }
        return -2;

    }
    public void DoMove(int checkFailed)
    {
        if (checkFailed == -1)
        {
            float addangle = (Mathf.PerlinNoise(noiseX * 0.01f, noiseY) - 0.5f) * 1.5f;
            noiseX++;
            angle = (angle + addangle) % 360;

            SetVelocity(angle, speed);
        }
        if (checkFailed == 0)
        {
            float addangle = FindTurnSide();
            angle = (angle + addangle) % 360;

            SetVelocity(angle, speed);
        }
        if (checkFailed == 1)
        {
            float addangle = FindTurnSide() * 0.3f;
            angle = (angle + addangle) % 360;

            SetVelocity(angle, speed);
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
            return -12;
        }
        else if (rightSide / (distances.Length / 2) < radiuses[0])
        {
            return 12;
        }
        if (leftSide > rightSide)
        {
            return 5;
        }
        else
        {
            return -5;
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
            if (Physics2D.CircleCast(playerCheck.position, 0.25f, dir, entityData.playerDetectRange, entityData.whatIsPlayer))
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
            float fishDistance = Physics2D.CircleCast(playerCheck.position, 0.25f, dir, entityData.playerDetectRange, entityData.whatIsPlayer).distance;

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
        if (!fish)
        {
            isAttacking = false;
            return;
        }
        isAttacking = true;
    }
    public virtual Vector3 GetVector3DirToFish(GameObject fish)
    {
        Vector3 dir = fish.transform.position - aliveGO.transform.position;
        return dir;
    }
    public virtual float GetAngleFromDir(Vector3 dir)
    {
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
    public virtual void CheckToNotCollideWithWall(int[] radiusesToUse, float speed)
    {
        for (int i = 0; i < sensorCount; i++)
        {
            var radAngle = (angle + (i * 360 / sensorCount)) * Mathf.Deg2Rad;
            distances[i] = Physics2D.Raycast(wallCheck.position, new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)), entityData.wallCheckDistance * speed, entityData.whatIsGround).distance;
            if (distances[i] == 0)
            {
                distances[i] = entityData.wallCheckDistance * speed;
            }
        }
        DoMove(CheckRadiuses(speed, radiusesToUse));
    }
    public virtual bool IsElementInList(int e, int[] l)
    {
        for (int i = 0; i < l.Length; i++)
        {
            if (e == l[i])
            {
                return true;
            }
        }
        return false;
    }
}
