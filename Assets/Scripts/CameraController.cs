using UnityEngine;

/// <summary>
/// The CameraController class controls the position and rotation of the camera in the game.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField, Range(0f, 20f)]
    private float distance = 10f; // The distance of the camera from the target

    public float Distance
    {
        get => distance;
        set => distance = value;
    }

    public Transform Target
    {
        get => target;
        set => target = value;
    }

    private Transform target; // The target (player) for the camera to follow

    public void Start()
    {
        // Find the player game object with the tag "Player"
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // Calculate the target position for the camera
        float targetZ = target.position.z - distance;
        float targetX = target.position.x;
        float targetY = target.position.y + distance;
        
        // Smoothly move the camera towards the target position
        Vector3 targetPosition = new Vector3(targetX, targetY, targetZ);
        float speed = (targetPosition - transform.position).magnitude * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);

        // Make the camera look at the target
        transform.LookAt(target);
    }
}