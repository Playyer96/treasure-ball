using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script is responsible for handling the movement of the player in a 3D game.
/// It uses Unity's Input System and Rigidbody component to move the player based on the inputs from the user.
/// </summary>

// Require the PlayerInput component to be attached to the same game object as this script
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    // Properties for sphere movement
    [Header("Sphere Movement Properties")] [Range(0, 10)]
    public float speed = 5; // Speed of movement

    [Range(0, 10)] public float jumpForce = 3; // Force applied when jumping

    // Properties for jetpack
    [Header("Jetpack Properties")] [Range(0, 10)]
    public float jetpackForce = 1; // Force applied when using the jetpack

    [SerializeField, Range(0, 10)] private float rechargeRate = 1; // Rate at which the jetpack recharges
    [SerializeField, Range(0, 5)] private float drainRate = 2f; // Rate at which the jetpack drains fuel
    [SerializeField, Range(0, 10)] private float fuelAmount = 1; // Total fuel available for the jetpack

    [SerializeField, Range(0, 5)] private float jetpackStartDelay = 1f; // Delay before the jetpack can be used after jumping
    [SerializeField, Range(0, 1)] private float minFuelToStart = 0.3f; // Minimum fuel needed to start the jetpack

    // Variables to store the current state of the player
    private Rigidbody rb; // Rigidbody component of the player
    private bool isGrounded = true; // Whether the player is on the ground
    private float isUsingJetpack; // Whether the jetpack is being used
    private float jetpackStartTime = 0f; // Time when the jetpack was started
    private float currentFuel; // Current fuel available for the jetpack
    private Vector2 _movement; // Current movement input

    // Properties to access the private variables
    public Rigidbody Rb
    {
        get => rb;
        set => rb = value;
    }

    public bool IsGrounded
    {
        get => isGrounded;
        set => isGrounded = value;
    }

    public float IsUsingJetpack
    {
        get => isUsingJetpack;
        set => isUsingJetpack = value;
    }

    public float JetpackStartTime
    {
        get => jetpackStartTime;
        set => jetpackStartTime = value;
    }

    public float CurrentFuel
    {
        get => currentFuel;
        set => currentFuel = value;
    }

    public Vector2 Movement
    {
        get => _movement;
        set => _movement = value;
    }
    
    public float FuelAmount
    {
        get => fuelAmount;
        set => fuelAmount = value;
    }

    public float JetpackStartDelay
    {
        get => jetpackStartDelay;
        set => jetpackStartDelay = value;
    }

    // Delegate for events related to fuel change
    public delegate void OnFuelChange();

    public static event OnFuelChange fuelChangeEvent;

    // Method to update the fuel and invoke the fuel change event
    public void UpdateFuel(float newFuel)
    {
        CurrentFuel = newFuel;
        fuelChangeEvent?.Invoke();
    }

    private void Start()
    {
        // Get the Rigidbody component and set the initial fuel
        Rb = GetComponent<Rigidbody>();
        CurrentFuel = fuelAmount;
    }

    public void FixedUpdate()
    {
        CheckGround();
        MovePlayer();
        Jumppack();
    }

    /// <summary>
    /// This function updates the movement vector for the player.
    /// </summary>
    /// <param name="value">The input value containing the new movement vector.</param>
    public void OnMovement(InputValue value) => Movement = value.Get<Vector2>();

    /// <summary>
    /// This function moves the player based on the current movement vector and camera's facing direction.
    /// </summary>
    public void MovePlayer()
    {
        // Get the camera's facing direction
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        // Calculate the movement direction based on the camera's facing direction and current movement input
        Vector3 movement = cameraForward * Movement.y + Camera.main.transform.right * Movement.x;

        Rb.MovePosition(Rb.position + (movement * speed * Time.deltaTime));
    }


    /// <summary>
    /// This function updates the jetpack activation state.
    /// </summary>
    /// <param name="value">The input value containing the activation state.</param>
    public void OnJump(InputValue value) => IsUsingJetpack = value.Get<float>();

    /// <summary>
    /// This function checks if the player is grounded by performing a raycast below them.
    /// </summary>
    public void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1f))
        {
            IsGrounded = hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground");
        }
        else
        {
            IsGrounded = false;
        }
    }

    /// <summary>
    /// This function implements the jetpack functionality.
    /// If the jetpack is activated and the player is grounded, they will jump.
    /// If the jetpack is activated and the player is not grounded, they will be propelled upwards.
    /// If the jetpack is deactivated, the fuel will recharge.
    /// </summary>
    public void Jumppack()
    {
        if (IsUsingJetpack > 0f)
        {
            if (IsGrounded)
            {
                if (CurrentFuel >= fuelAmount * minFuelToStart)
                {
                    Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    IsGrounded = false;
                }
            }
            else
            {
                if (Time.time - JetpackStartTime >= jetpackStartDelay && CurrentFuel > 0)
                {
                    Rb.AddForce(Vector3.up * jetpackForce, ForceMode.Acceleration);
                    CurrentFuel -= drainRate * Time.deltaTime;
                    UpdateFuel(CurrentFuel);
                }

            }
        }
        else
        {
            CurrentFuel += rechargeRate * Time.deltaTime;
            CurrentFuel = Mathf.Min(CurrentFuel, fuelAmount);
            UpdateFuel(CurrentFuel);
            jetpackStartTime = 0;
        }
    }
}