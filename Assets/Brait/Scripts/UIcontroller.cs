using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIcontroller : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private void Start()
    {
        GameManager.Instance.onFishChange += ChangeText;
    }

    private void ChangeText()
    {
        text.text = GameManager.Instance.NumberOfFish.ToString();
    }

    public void OnDestroy()
    {
        GameManager.Instance.onFishChange -= ChangeText;
    }
}
