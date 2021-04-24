using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaweedBush : MonoBehaviour
{
    [SerializeField] float verticalBounds = 0.05f;
    [SerializeField] float horizontalBounds = 1f;
    [SerializeField] int leafCount = 40;
    [SerializeField] float maxRotation;
    [SerializeField] GameObject prefabLeaf;

    private Vector3 posToGo;
    private Quaternion rotToGo;

    public void setPos(Vector3 pos, Quaternion rot)
    {
        posToGo = pos;
        rotToGo = rot;
    }

    private void Start()
    {
        for(int i = 0; i < leafCount; i++)
        {
            SpawnLeaf();
        }
        transform.parent.position = posToGo;
        transform.parent.rotation = rotToGo;
    }

    private void SpawnLeaf()
    {
        float xPos = Random.Range(-horizontalBounds, horizontalBounds);
        float yPos = Random.Range(-verticalBounds, verticalBounds);
        float leafRotation = -xPos * maxRotation;

        Quaternion leafRot = Quaternion.Euler(0, 0, leafRotation);
        Vector3 leafPos = new Vector3(xPos, yPos, -yPos-verticalBounds);

        GameObject leaf = Instantiate(prefabLeaf, transform);

        leaf.transform.localPosition = leafPos;
        leaf.transform.rotation = leafRot;
    }
}
