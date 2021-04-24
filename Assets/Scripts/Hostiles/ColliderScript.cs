using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
    [SerializeField] D_AttackState stateData;
    public int killedCount {get;set;}

    // Start is called before the first frame update
    void Start()
    {
        killedCount = 0;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (stateData.maxKillCount>killedCount)
        {
            if (other.gameObject.tag.Equals("Fish")){
                killedCount++;
                Destroy(other.gameObject);
            }
        } 

    }
}