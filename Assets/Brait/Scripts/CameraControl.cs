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

    /*public void FixedUpdate()
    {
        Vector3 selfPos = new Vector3(transform.position.x, transform.position.y, 0);
        float dist = Vector3.Distance(selfPos, schoolMiddle.transform.position);

        if (dist > distance)
        {
            Vector3 newPos = Vector3.Lerp(selfPos, schoolMiddle.transform.position, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, -10);
        }
    }*/

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
        if (vectDist < standingSpeed)
        {
            return;
        }

        if (vectDist < minSpeed)
        {
            mouseToWorld = mouseToWorld.normalized * minSpeed;
        }

        Vector3 selfPos = new Vector3(transform.position.x + (mouseToWorld.x*Time.deltaTime*speed), transform.position.y + (mouseToWorld.y*Time.deltaTime*speed), -10);

        transform.position = selfPos;
    }

}
