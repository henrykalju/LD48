using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] backgrounds = new GameObject[2];
    [SerializeField] float[] lengths = new float[2];
    [SerializeField] float[] offsets = new float[2];
    [SerializeField] float newSpawnDistance = 3;

    private float prevLength;
    private float prevPos;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("SchoolMiddle");
        int ran = Random.Range(0, backgrounds.Length);
        float newPos = 0;
        GameObject bacgrndObj = Instantiate(backgrounds[ran], transform);
        bacgrndObj.transform.position = new Vector3(offsets[ran], newPos, 5);
        prevLength = lengths[ran];
        prevPos = newPos;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(new Vector3(0, player.transform.position.y, 0 ), new Vector3(0, prevPos, 0)) > newSpawnDistance && prevPos > player.transform.position.y)
        {
            SpawnBackground();
        }
    }
    private void SpawnBackground()
    {
        int ran = Random.Range(0, backgrounds.Length);
        float newPos = prevPos - prevLength / 2 - lengths[ran] / 2;
        GameObject bacgrndObj = Instantiate(backgrounds[ran], transform);
        bacgrndObj.transform.position = new Vector3(offsets[ran], newPos, 5);
        prevLength = lengths[ran];
        prevPos = newPos;
    }

}
