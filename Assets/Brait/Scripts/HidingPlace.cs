using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.layer = 10;
        collision.transform.tag = "FishHidden";
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.layer = 10;
        collision.transform.tag = "FishHidden";
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.layer = 6;
        collision.transform.tag = "Fish";
    }
}
