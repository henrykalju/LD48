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
            if (other.gameObject.layer == 10) return;
            if (other.gameObject.layer == 6){
                killedCount++;
                Destroy(other.gameObject);
            }
        } 

    }

    void OnDestroy() {
        Destroy(transform.parent.gameObject);
    }
}