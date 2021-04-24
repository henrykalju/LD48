using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;
    [SerializeField] private float randomness;

    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private GameObject school;

    private void Start()
    {
        school = GameObject.FindWithTag("SchoolMiddle");

        speed += Random.Range(-1f, 1f);
        maxSpeed += Random.Range(-1f, 1f);

        minDist += Random.Range(-1f, 1f);
        maxDist += Random.Range(-1f, 1f);
    }

    private void FixedUpdate()
    {
        float distToSchool = Vector3.Distance(transform.position, school.transform.position);

        if (distToSchool > maxDist)
        {
            Debug.Log("Too far");
            return;
        }

        else if (distToSchool < minDist)
        {
            Debug.Log("Too close");
            return;
        }

        else
        {
            Vector3 forceToAdd = (school.transform.position - transform.position).normalized * speed;
            rbody.AddForce(forceToAdd);

            float curSpeed = Vector3.Distance(Vector3.zero, rbody.velocity);
            if (curSpeed > maxSpeed)
            {
                rbody.velocity = rbody.velocity.normalized * maxSpeed;
            }
        }
    }
}
