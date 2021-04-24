using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{

    [SerializeField] private GameObject schoolMiddle;

    [SerializeField] private float distance;
    [SerializeField] private float speed;

    [SerializeField] private PlayerInput controls;

    public void FixedUpdate()
    {
        Vector3 selfPos = new Vector3(transform.position.x, transform.position.y, 0);
        float dist = Vector3.Distance(selfPos, schoolMiddle.transform.position);

        if (dist > distance)
        {
            Vector3 newPos = Vector3.Lerp(selfPos, schoolMiddle.transform.position, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, -10);
        }
    }
}
