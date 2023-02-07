using UnityEngine;

/// <summary>
/// GameManager class handles the game logic and events.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager; // Reference to UIManager component
    [SerializeField] private int coinValue = 10;
    [SerializeField] private int keyValue = 50;
    [SerializeField] private int totalPoints;

    private bool canOpenTheTreasure = false;
    private bool treasureOpened = false; // Flag to check if the treasure has been opened
    private float startTime;
    private float currentTime; // Current time elapsed since the start of the game
    private int coinsCollected;
    private int keysCollected;
    public int totalCollected;
    private Coin[] coins; // Array of coins in the scene
    private Key[] keys; // Array of keys in the scene
    private Leaderboard _leaderboard; // Reference to the leaderboard component    
    private static GameManager _instance; // Singleton instance of the GameManager class
    
    public int CoinValue
    {
        get => coinValue;
        set => coinValue = value;
    }

    public int KeyValue
    {
        get => keyValue;
        set => keyValue = value;
    }

    public int CoinsCollected
    {
        get => coinsCollected;
        set => coinsCollected = value;
    }

    public int KeysCollected
    {
        get => keysCollected;
        set => keysCollected = value;
    }
    
    // Gets the singleton instance of the GameManager class
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

    private void Start()
    {
        Time.timeScale = 1;
        
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
            canOpenTheTreasure = true;
        }
    }
    
    // Increments the count of collected coins by `coinValue`
    public void CollectCoin()
    {
        coinsCollected ++;
        totalCollected += coinValue;
    }

    // Updates the keys that the player collected
    public void CollectKey()
    {
        keysCollected ++;
        totalCollected += keyValue;
    }

    /// <summary>
    /// OpenTreasure is a public function that opens the treasure if it's not already opened.
    /// It sets the value of `treasureOpened` to `true` to make sure the treasure is not opened again.
    /// It also sets the `Time.timeScale` to 0, effectively pausing the game.
    /// If the `_uiManager` is not assigned, it finds the `UIManager` in the scene.
    /// If the total collected (`totalCollected`) is greater than or equal to the highest score in the leaderboard (`_uiManager.Leaderboard.HighestScore`),
    /// it shows the input field to submit a new score and calls the `SummitNewScore` function on the `_uiManager` to submit the new score.
    /// </summary>
    public void OpenTreasure()
    {
        if(!canOpenTheTreasure) return;
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
            if (!_uiManager)
                _uiManager = FindObjectOfType<UIManager>();
            
            _uiManager.ShowGameOverScreen(true);
        }
    }
}
