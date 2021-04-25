using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIcontroller : MonoBehaviour
{
    [SerializeField] TMP_Text size_text;
    [SerializeField] TMP_Text depth_text;

    [SerializeField] private GameObject pause_btn;
    [SerializeField] private GameObject resume_btn;
    [SerializeField] private GameObject restart_btn;
    [SerializeField] private GameObject menu_btn;

    [SerializeField] private GameObject school_middle;

    private int d = 0;
    
    private void Start()
    {
        GameManager.Instance.onFishChange += ChangeText;
    }

    private void Update()
    {
        print("a");
        d = (int) Math.Max(Math.Floor(-school_middle.transform.position.y), d);
        depth_text.text = d + "m";
    }

    private void ChangeText()
    {
        size_text.text = GameManager.Instance.NumberOfFish.ToString();
    }

    public void PauseGame()
    {
        
        print("pause");
        
        //stop phisycs
        Time.timeScale = 0;
        
        resume_btn.gameObject.SetActive(true);
        restart_btn.gameObject.SetActive(true);
        menu_btn.gameObject.SetActive(true);

        pause_btn.gameObject.SetActive(false);
        depth_text.gameObject.SetActive(false);
        size_text.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        
        print("resume");
        
        //start phisycs
        Time.timeScale = 1;
        
        resume_btn.gameObject.SetActive(false);
        restart_btn.gameObject.SetActive(false);
        menu_btn.gameObject.SetActive(false);

        pause_btn.gameObject.SetActive(true);
        depth_text.gameObject.SetActive(true);
        size_text.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        ResumeGame();
        SceneManager.LoadScene("GameScene");
    }
    

    public void OnDestroy()
    {
        GameManager.Instance.onFishChange -= ChangeText;
    }
}
