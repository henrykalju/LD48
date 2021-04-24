using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{

    [SerializeField] private GameObject schoolMiddle;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float standingSpeed;

    [SerializeField] private PlayerInput controls;

    private int screenMidHorizontal;
    private int screenMidVertical;

    private void Start()
    {
        int width = Screen.width;
        int height = Screen.height;
        screenMidHorizontal = (int)(width / 2);
        screenMidVertical = (int)(height / 2);
    }

    private void FixedUpdate()
    {
        Vector2 mousePos = controls.actions["MouseLocation"].ReadValue<Vector2>();
        Vector2 mousePosMod = new Vector2(mousePos.x - screenMidHorizontal, mousePos.y - screenMidVertical);
        Vector2 mouseToWorld = new Vector2(mousePosMod.x / screenMidHorizontal, mousePosMod.y / screenMidVertical);

        float vectDist = Vector3.Distance(Vector3.zero, mouseToWorld);

        if (vectDist > maxSpeed)
        {
            mouseToWorld = mouseToWorld.normalized * maxSpeed;
        }

        Vector3 selfPos = new Vector3(transform.position.x + (mouseToWorld.x*Time.deltaTime*speed), transform.position.y + (mouseToWorld.y*Time.deltaTime*speed), -10);

        transform.position = selfPos;
    }

}
