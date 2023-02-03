using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Image component to display the fuel bar
    [SerializeField] private Image fuelBar;

    // Array of colors to use for different fuel levels
    [SerializeField] private Color[] fuelColors;

    // Reference to the player movement script
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        if (!playerMovement)
        {
            // Get the player movement component from the object with "Player" tag
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    private void OnEnable()
    {
        // Subscribe to the fuel change event
        PlayerMovement.fuelChangeEvent += UpdateFuelBar;
    }

    private void OnDisable()
    {
        // Unsubscribe from the fuel change event
        PlayerMovement.fuelChangeEvent -= UpdateFuelBar;
    }

    private void UpdateFuelBar()
    {
        // Calculate the current fuel percentage
        float currentFuelPercent = playerMovement.CurrentFuel / playerMovement.fuelAmount;

        // Update the fill amount of the fuel bar
        fuelBar.fillAmount = currentFuelPercent;

        // Determine the color to lerp to based on fuel percentage
        int colorIndex = Mathf.FloorToInt(currentFuelPercent * fuelColors.Length);
        colorIndex = Mathf.Clamp(colorIndex, 0, fuelColors.Length - 1);
        Color colorToLerpTo = fuelColors[colorIndex];

        // Lerp the color of the fuel bar to the determined color
        fuelBar.color = Color.Lerp(fuelBar.color, colorToLerpTo, Time.deltaTime * 5f);
    }
}
