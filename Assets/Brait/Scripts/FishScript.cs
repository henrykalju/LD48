using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;
    [SerializeField] private Vector2 randomness;
    [SerializeField] private float wanderingSpeed;
    [SerializeField] private float wanderingMaxSpeed;

    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private GameObject school;

    [SerializeField] private Vector3 lastKnownLocation = new Vector3(0,0,10);

    private void Start()
    {
        int rnd = Random.Range(1, 4);
        if (rnd == 1)
        {
            gameObject.layer = 6;
        }


        randomness = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));

        school = GameObject.FindWithTag("SchoolMiddle");

        speed += Random.Range(-1f, 1f);
        maxSpeed += Random.Range(-1f, 1f);

        minDist += Random.Range(-minDist/2, minDist);
        maxDist += Random.Range(-1f, 1f);
    }

    private void FixedUpdate()
    {
        
        float distToSchool = Vector3.Distance(transform.position, school.transform.position);

        if (distToSchool > maxDist)
        {

            if (lastKnownLocation == new Vector3(0, 0, 10))
            {
                lastKnownLocation = transform.position;
            }

            float distToLast = Vector3.Distance(transform.position, lastKnownLocation);

            if (distToLast > minDist)
            {
                Debug.Log(distToLast);
                Vector3 forceToAdd = (lastKnownLocation - transform.position).normalized * wanderingSpeed;
                rbody.AddForce(new Vector3(forceToAdd.x + randomness.x, forceToAdd.y + randomness.y, 0));

                float curSpeed = Vector3.Distance(Vector3.zero, rbody.velocity);
                if (curSpeed > wanderingMaxSpeed)
                {
                    rbody.velocity = rbody.velocity.normalized * wanderingMaxSpeed;
                }
            }
        }

        else if (distToSchool < minDist)
        {
            Debug.Log("Too close");
        }

        else
        {
            lastKnownLocation = new Vector3(0, 0, 10);

            Vector3 forceToAdd = (school.transform.position - transform.position).normalized * speed;
            rbody.AddForce(new Vector3(forceToAdd.x + randomness.x, forceToAdd.y + randomness.y, 0));

            float curSpeed = Vector3.Distance(Vector3.zero, rbody.velocity);
            if (curSpeed > maxSpeed)
            {
                rbody.velocity = rbody.velocity.normalized * maxSpeed;
            }
        }

        transform.rotation = Quaternion.Euler(0, 0, RotationFromDirection(rbody.velocity));
    }

    private float RotationFromDirection(Vector3 direction)
    {
        Vector2 dir = new Vector2(direction.x, direction.y);
        float rotation = (float)(Mathf.Atan2(dir.y, dir.x) / (2 * Mathf.PI) * 360);

        return rotation;
    }
}
