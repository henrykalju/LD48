using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] junk;
    [SerializeField] int junkAmount;
    [SerializeField] float horizontalCap;
    [SerializeField] float verticalCap;

    private void Start()
    {
        for (int i = 0; i < junkAmount; i++)
        {
            spawnJunk();
        }
    }

    private void spawnJunk()
    {
        float xPos = Random.Range(-horizontalCap, horizontalCap);
        float yPos = Random.Range(-verticalCap, verticalCap);

        Vector3 newPos = new Vector3(xPos, yPos, 0);
        float rotation = Random.Range(-180f, 180f);

        int ran = Random.Range(0, junk.Length);

        GameObject junkObj = Instantiate(junk[ran], transform);
        junkObj.transform.position = newPos;
        junkObj.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
