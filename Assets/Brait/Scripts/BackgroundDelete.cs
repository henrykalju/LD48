using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundDelete : MonoBehaviour
{

    [SerializeField] float distance = 10f;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("SchoolMiddle");
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(new Vector3(0, transform.position.y, 0), new Vector3(0, player.transform.position.y, 0)) > distance && player.transform.position.y < transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
