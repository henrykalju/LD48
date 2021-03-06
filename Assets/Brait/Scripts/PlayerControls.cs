using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private PlayerInput controls;

    [SerializeField] private GameObject fishPrefab;


    private void Start()
    {
        SpawnFish(100);
    }
    private void FixedUpdate()
    {
        Vector3 mousePosWorld = GetMouseInWorld();
        MoveTowards(mousePosWorld);
    }

    private void MoveTowards(Vector3 toPos)
    {
        Vector3 selfPos = transform.position;
        Vector3 direction = toPos - selfPos;

        Vector3 directionNormalized = direction.normalized * maxSpeed;

        Vector3 newPos = Vector3.Lerp(selfPos, selfPos + directionNormalized, speed * Time.deltaTime);

        transform.position = newPos;
    }

    private Vector3 GetMouseInWorld()
    {
        Vector2 mousePos = controls.actions["MouseLocation"].ReadValue<Vector2>();
        Vector3 mousePos3 = new Vector3(mousePos.x, mousePos.y, 10);

        return (Camera.main.ScreenToWorldPoint(mousePos3));
    }

    public void SpawnFish(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject fish = GameObject.Instantiate(fishPrefab, transform.parent);
            fish.transform.position = transform.position;
        }
        GameManager.Instance.NumberOfFish = amount;
    }

    public void KillFish(int amount)
    {
        int fishCount = transform.childCount;

        if (fishCount <= amount)
        {
            Debug.Log("GAME OVER MOTHERFUCKER!");
            amount = fishCount;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject fish = transform.GetChild(0).gameObject;
            GameObject.Destroy(fish);
        }
    }
}
