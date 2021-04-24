using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seaweed : MonoBehaviour
{
    [SerializeField] string[] animNames = new string[3];
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] float minColor = 0.4f;
    [SerializeField] float colorMultiplier = 7;

    private void Start()
    {
        int ran = Random.Range(1, 4);

        animator.SetInteger("Random", ran);
        animator.Play(animNames[ran - 1], 0, Random.Range(0f, animator.GetCurrentAnimatorStateInfo(0).length));
        if (transform.parent.transform.localPosition.y >= 0)
        {
            renderer.sortingLayerName = "env front";
        }
        else
        {
            renderer.sortingLayerName = "env behind";
        }

        float depth = transform.parent.localPosition.z * -colorMultiplier;

        if (depth < minColor)
        {
            depth = minColor;
        }

        Color shade = new Color(0 + depth, 0 + depth, 0 + depth);
        renderer.color = shade;
    }
}
