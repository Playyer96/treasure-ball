using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The `PlayerMovement` script adds player movement and jump pack functionality to the player game object 
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Sphere Movement Properties")]

    [Range(0, 10)] public float speed = 5;

    [Range(0, 10)] public float jumpForce = 3;

    [Header("Jetpack Properties")]

    [Range(0, 10)] public float jetpackForce = 1;

    [Range(0, 10)] public float rechargeRate = 1;

    [Range(0, 5)] public float drainRate = 2f;

    [Range(0, 10)] public float fuelAmount = 1;

    [Range(0, 5)] public float jetpackStartDelay = 1f;

    [Range(0, 1)] public float minFuelToStart = 0.3f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private float isUsingJetpack;
    private float jetpackStartTime = 0f;
    private float currentFuel;
    private Vector2 _movement;

    public Rigidbody Rb { get => rb; set => rb = value; }
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public float IsUsingJetpack { get => isUsingJetpack; set => isUsingJetpack = value; }
    public float JetpackStartTime { get => jetpackStartTime; set => jetpackStartTime = value; }
    public float CurrentFuel { get => currentFuel; set => currentFuel = value; }
    public Vector2 Movement { get => _movement; set => _movement = value; }

    public delegate void OnFuelChange();
    public static event OnFuelChange fuelChangeEvent;

    private void UpdateFuel(float newFuel)
    {
        CurrentFuel = newFuel;
        fuelChangeEvent?.Invoke();
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        CurrentFuel = fuelAmount;
    }

    private void FixedUpdate()
    {
        CheckGround();
        MovePlayer();
        Jumppack();
    }

    public void OnMovement(InputValue value) => Movement = value.Get<Vector2>();

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(Movement.x, 0f, Movement.y);

        Rb.MovePosition(Rb.position + (movement * speed * Time.deltaTime));
    }

    public void OnJump(InputValue value) => IsUsingJetpack = value.Get<float>();

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