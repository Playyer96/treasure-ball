using UnityEngine;

/// <summary>
/// The Pickable class represents a pickable object in the game.
/// </summary>
public abstract class Pickable : MonoBehaviour
{
    /// <summary>
    /// The Pickup method collects the coin and destroys the game object.
    /// </summary>
    public virtual void Pickup()
    {
        // Get the instance of the game manager
        GameManager gameManager = GameManager.Instance;

        // Call the CollectCoin method from the game manager
        gameManager.CollectCoin();

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