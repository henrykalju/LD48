using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfSectionScript : MonoBehaviour
{

    private GameObject map;

    private void Start()
    {
        map = GameObject.FindWithTag("Map");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        ((MapGen)map.GetComponent(typeof(MapGen))).UpdateMap();
    }
}
