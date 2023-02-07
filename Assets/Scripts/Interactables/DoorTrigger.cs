using UnityEngine;
using System.Collections;

/// <summary>
/// Script to animate the door opening and closing when the player enters or exits the trigger.
/// </summary>
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform doorTransform;

    [SerializeField] private float doorOpenY = 5f; // Y position of the door when it is open.
    [SerializeField] private float doorCloseY = 0f; // Y position of the door when it is closed.
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float targetY; // Y position target for the door animation.

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player and start the door animation
        if (other.CompareTag("Player"))
        {
            isOpen = true;
            targetY = doorOpenY;
            StartCoroutine(AnimateDoor());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider is the player and start the door animation
        if (other.CompareTag("Player"))
        {
            isOpen = false;
            targetY = doorCloseY;
            StartCoroutine(AnimateDoor());
        }
    }

    private IEnumerator AnimateDoor()
    {
        // Set the start time and calculate the journey length
        float startTime = Time.time;
        float journeyLength = Mathf.Abs(doorTransform.position.y - targetY);

        // Animate the door until it reaches the target position
        while (doorTransform.position.y != targetY)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            doorTransform.position = Vector3.Lerp(doorTransform.position,
                new Vector3(doorTransform.position.x, targetY, doorTransform.position.z), fracJourney);
            yield return null;
        }
    }
}