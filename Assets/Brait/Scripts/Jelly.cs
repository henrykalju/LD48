using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    [SerializeField] float PushForce = 10f;
    [SerializeField] float jumpTime = 10f;

    [SerializeField] float slowForce = 0.01f;
    [SerializeField] float offset = 60f;

    [SerializeField] Rigidbody2D rbody;

    private float lastJump = 0;
    private Vector3 lastJumpDir;

    private void FixedUpdate()
    {
        if (jumpTime < Time.time - lastJump)
        {
            lastJump = Time.time;

            lastJumpDir = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f),0).normalized * PushForce;
            rbody.AddForce(lastJumpDir);
        }


        Vector3 velo = rbody.velocity;
        float rotation = (float)(Mathf.Atan2(velo.y, velo.x) / (2 * Mathf.PI) * 360);

        transform.rotation = Quaternion.Euler(0, 0, rotation-offset);
    }
}
