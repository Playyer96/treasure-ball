using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject fuelbarObject;
    // Image component to display the fuel bar
    [SerializeField] private Image fuelBar;

    [SerializeField] private GameObject inputNewScoreUI;
    [SerializeField] private GameObject gameoverScreen;
    public delegate void OnScoreUpdate();
    public static OnScoreUpdate onScoreUpdate;

    public TextMeshProUGUI scoreText;
    private int score = 0;

    
    public Image FuelBar
    {
        get => fuelBar;
        set => fuelBar = value;
    }

    public GameObject InputNewScoreUI
    {
        get => inputNewScoreUI;
        set => inputNewScoreUI = value;
    }

    public GameObject LeaderboardUI
    {
        get => leaderboardUI;
        set => leaderboardUI = value;
    }

    public Button SummitButton1 => summitButton;

    public Color[] FuelColors
    {
        get => fuelColors;
        set => fuelColors = value;
    }

    public PlayerMovement PlayerMovement
    {
        get => playerMovement;
        set => playerMovement = value;
    }

    public int Score => _score;

    [SerializeField] private GameObject leaderboardUI;
    [SerializeField] private Leaderboard _leaderboard;

    public Leaderboard Leaderboard => _leaderboard;

    [SerializeField] private Button summitButton;
    [SerializeField] private Button restartButton;

    // Array of colors to use for different fuel levels
    [SerializeField] private Color[] fuelColors;

    // Reference to the player movement script
    [SerializeField] private PlayerMovement playerMovement;

    private int _score;
    private float _time;

    public float time => _time;

    private void Start()
    {
        if (!playerMovement)
        {
            // Get the player movement component from the object with "Player" tag
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
        
        summitButton.onClick.AddListener(SummitButton);
        restartButton.onClick.AddListener(RestartButton);
        
        ShowFuelbar(true);
        ShowGameOverScreen(false);
        ShowCursor(false, CursorLockMode.Locked);
    }

    private void OnEnable()
    {
        // Subscribe to the fuel change event
        PlayerMovement.fuelChangeEvent += UpdateFuelBar;
        onScoreUpdate += UpdateScore;
    }

    private void OnDisable()
    {
        // Unsubscribe from the fuel change event
        PlayerMovement.fuelChangeEvent -= UpdateFuelBar;
        onScoreUpdate -= UpdateScore;
    }

    private void UpdateScore()
    {
        score = GameManager.Instance.totalCollected;
        scoreText.text = "Score: " + score;

        // Show effect when player gets a point
        StartCoroutine(PlayIncrementEffect());
    }

    public void UpdateFuelBar()
    {
        // Calculate the current fuel percentage
        float currentFuelPercent = playerMovement.CurrentFuel / playerMovement.FuelAmount;

        // Update the fill amount of the fuel bar
        fuelBar.fillAmount = currentFuelPercent;

        // Determine the color to lerp to based on fuel percentage
        int colorIndex = Mathf.FloorToInt(currentFuelPercent * fuelColors.Length);
        colorIndex = Mathf.Clamp(colorIndex, 0, fuelColors.Length - 1);
        Color colorToLerpTo = fuelColors[colorIndex];

        // Lerp the color of the fuel bar to the determined color
        fuelBar.color = Color.Lerp(fuelBar.color, colorToLerpTo, Time.deltaTime * 5f);
    }

    public void ShowFuelbar(bool value)
    {
        fuelbarObject.SetActive(value);
    }
    
    public void ShowInputNewScore(bool value)
    {
        inputNewScoreUI.SetActive(value);
        ShowCursor(true, CursorLockMode.None);
    }

    public void ShowLeaderboard(bool value)
    {
        leaderboardUI.SetActive(value);
    }

    public void SummitNewScore(int score, float time)
    {
        _score = score;
        _time = time;
    }

    public void SummitButton()
    {
        ShowInputNewScore(false);
        _leaderboard.UpdateUI();
        ShowLeaderboard(true);
        _leaderboard.AddNewScore(_score, _time);
    }

    private IEnumerator PlayIncrementEffect()
    {
        yield return new WaitForEndOfFrame();
        float originalSize = 72;
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(originalSize, originalSize * 1.5f, elapsedTime / duration);
            scoreText.fontSize = (int)scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(originalSize * 1.5f, originalSize, elapsedTime / duration);
            scoreText.fontSize = (int)scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scoreText.fontSize = (int)originalSize;
    }

    public void ShowGameOverScreen(bool value)
    {
        gameoverScreen.SetActive(value);
        ShowCursor(true, CursorLockMode.None);
    }
    
    public void ShowCursor(bool value, CursorLockMode lockMode)
    {
        Cursor.visible = value;
        Cursor.lockState = lockMode;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
