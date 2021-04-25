using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private Dictionary<string, GameObject> pages;
    private GameObject currentPage;

    private void Start() {
        pages = new Dictionary<string, GameObject>();
        pages.Add("Main", GameObject.FindGameObjectWithTag("Main"));
        pages.Add("Settings", GameObject.FindGameObjectWithTag("Settings"));
        pages.Add("Credits", GameObject.FindGameObjectWithTag("Credits"));
        currentPage = pages["Main"];
        pages["Settings"].SetActive(false);
        pages["Credits"].SetActive(false);

    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ChangePage(string page) {
        currentPage.SetActive(false);
        pages[page].SetActive(true);
        currentPage = pages[page];

    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
