using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.layer = 10;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.layer = 6;
    }
}
