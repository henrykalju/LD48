using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    [SerializeField] float PushForce = 10f;
    [SerializeField] float jumpTime = 5f;

    [SerializeField] float slowForce = 0.01f;
    [SerializeField] float offset = 60f;

    [SerializeField] Rigidbody2D rbody;

    private float lastJump = 0;
    private Vector3 lastJumpDir;
    private int toKill = 1;

    private void Start()
    {
        jumpTime += Random.Range(-1f, 1f);
    }

    private void FixedUpdate()
    {
        if (jumpTime < Time.time - lastJump)
        {
            toKill = 1;
            lastJump = Time.time;

            lastJumpDir = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f),0).normalized * PushForce;
            rbody.AddForce(lastJumpDir);
        }


        Vector3 velo = rbody.velocity;
        float rotation = (float)(Mathf.Atan2(velo.y, velo.x) / (2 * Mathf.PI) * 360);

        transform.rotation = Quaternion.Euler(0, 0, rotation-offset);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag  == "Fish" && toKill != 0)
        {
            Destroy(collision.gameObject);
            toKill = 0;
        }
    }
}
