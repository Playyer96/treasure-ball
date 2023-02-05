using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int coinValue = 10;
    [SerializeField] private int keyValue = 50;
    [SerializeField] private int totalPoints;
    
    private bool treasureOpened = false;

    private float startTime;
    private float currentTime;
    private int coinsCollected;
    private int keysCollected;
    public int totalCollected;

    [SerializeField]private UIManager _uiManager;

    private Coin[] coins;
    private Key[] keys;
    
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    private Leaderboard _leaderboard;

    private void Start()
    {
        if (!_uiManager)
            _uiManager = FindObjectOfType<UIManager>();
        
        coins = FindObjectsOfType<Coin>();
        keys = FindObjectsOfType<Key>();
        
        startTime = Time.time;
        totalPoints = (coinValue * coins.Length) + (keyValue * keys.Length);
    }

    private void Update()
    {
        currentTime = Time.time - startTime;

        if (keysCollected >= keys.Length)
        {
            OpenTreasure();
        }
    }

    public void CollectCoin()
    {
        coinsCollected += coinValue;
        totalCollected = coinsCollected + keysCollected;
    }

    public void CollectKey()
    {
        keysCollected += keyValue;
        totalCollected = coinsCollected + keysCollected;
    }

    public void OpenTreasure()
    {
        if (treasureOpened) return;

        treasureOpened = true;
        Time.timeScale = 0;

        if (!_uiManager)
            _uiManager = FindObjectOfType<UIManager>();

        if (totalCollected >= _uiManager.Leaderboard.HighestScore)
        {
            _uiManager.ShowInputNewScore(true);
            _uiManager.SummitNewScore(totalCollected, currentTime);
        }
        else
        {
            
        }
    }
}
