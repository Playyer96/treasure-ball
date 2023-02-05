using System.Collections;
using System.Collections.Generic;
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

    [Range(0, 10)] public float rechargeRate = 1; // Rate at which the jetpack recharges
    [Range(0, 5)] public float drainRate = 2f; // Rate at which the jetpack drains fuel
    [Range(0, 10)] public float fuelAmount = 1; // Total fuel available for the jetpack
    [Range(0, 5)] public float jetpackStartDelay = 1f; // Delay before the jetpack can be used after jumping
    [Range(0, 1)] public float minFuelToStart = 0.3f; // Minimum fuel needed to start the jetpack

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

    private void FixedUpdate()
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
    /// This function moves the player based on the current movement vector.
    /// </summary>
    public void MovePlayer()
    {
        Vector3 movement = new Vector3(Movement.x, 0f, Movement.y);

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