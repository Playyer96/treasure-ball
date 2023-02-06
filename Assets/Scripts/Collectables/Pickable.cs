using UnityEngine;

/// <summary>
/// The Pickable class represents a pickable object in the game.
/// </summary>
public abstract class Pickable : MonoBehaviour
{
    [SerializeField] private float spinSpeedMin = 50f;
    [SerializeField] private float spinSpeedMax = 200f;

    private float spinSpeed = 0f;

    private void Start()
    {
        spinSpeed = Random.Range(spinSpeedMin, spinSpeedMax);
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    /// <summary>
    /// The Pickup method collects the coin and destroys the game object.
    /// </summary>
    public virtual void Pickup()
    {
        // Get the instance of the game manager
        GameManager gameManager = GameManager.Instance;

        // Call the CollectCoin method from the game manager
        gameManager.CollectCoin();
        UIManager.onScoreUpdate();

        // Destroy the game object
        Destroy(this.gameObject);
    }

    /// <summary>
    /// The OnTriggerEnter method detects if the pickable object collides with the player.
    /// If the player collides with the pickable object, the Pickup method is called.
    /// </summary>
    /// <param name="other">The Collider component of the object that collided with the pickable object.</param>
    public virtual void OnTriggerEnter(Collider other)
    {
        // Check if the collider is tagged as "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Call the Pickup method
            Pickup();
        }
    }
}
