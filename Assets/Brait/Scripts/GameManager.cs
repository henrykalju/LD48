using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Starting,
    Playing,
    Over
}

public delegate void OnGameStateChange();
public delegate void OnFishChange();

public class GameManager : MonoBehaviour
{
    public event OnGameStateChange onStateChange;
    public event OnFishChange onFishChange;

    private GameState game;

    public GameState Game
    {
        set
        {
            if (value != game)
            {
                GameStateChange();
                game = value;
            }
        }
        get
        {
            return game;
        }
    }


    public static GameManager Instance
    {
        get; private set;
    }

    private int numberOfFish = 0;

    public int NumberOfFish
    {
        set
        {

            numberOfFish += value;
            onFishChange();
            if (value < 0)
            {
                CheckFishies();
            }
        }
        get
        {
            return numberOfFish;
        }
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void CheckFishies()
    {
        if (NumberOfFish < 1)
        {
            Game = GameState.Over;
        }
    }

    private void GameStateChange()
    {
        Debug.Log("Game is over");
    }
}
