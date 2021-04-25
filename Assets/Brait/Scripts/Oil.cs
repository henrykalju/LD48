using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float distance;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("SchoolMiddle");
    }

    public void FixedUpdate()
    {
        if (Mathf.Abs(player.transform.position.y) - Mathf.Abs(transform.position.y) > distance)
        {
            transform.position = new Vector3(0, player.transform.position.y + distance, 0);
        }

        else
        {
            transform.position = new Vector3(0, transform.position.y - (speed * Time.deltaTime), 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
