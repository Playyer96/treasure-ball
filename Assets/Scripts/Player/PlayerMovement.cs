using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float speed = 5;

    [SerializeField, Range(0, 10)] private float jumpForce = 3;

    [SerializeField, Range(0, 10)] private float jetpackForce = 1;

    [SerializeField, Range(0, 10)] private float rechargeRate = 1;

    [SerializeField, Range(0, 5)] private float drainRate = 2f;

    [SerializeField, Range(0, 10)] private float fuelAmount = 1;

    [SerializeField, Range(0, 5)] private float jetpackStartDelay = 1f;

    [SerializeField, Range(0, 1)] private float minFuelToStart = 0.3f;

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
    public float Speed { get => speed; set => speed = value; }
    public float JumpForce { get => jumpForce; set => jumpForce = value; }
    public float JetpackForce { get => jetpackForce; set => jetpackForce = value; }
    public float RechargeRate { get => rechargeRate; set => rechargeRate = value; }
    public float DrainRate { get => drainRate; set => drainRate = value; }
    public float FuelAmount { get => fuelAmount; set => fuelAmount = value; }
    public float JetpackStartDelay { get => jetpackStartDelay; set => jetpackStartDelay = value; }
    public float MinFuelToStart { get => minFuelToStart; set => minFuelToStart = value; }

    public delegate void OnFuelChange();
    public static event OnFuelChange fuelChangeEvent;

    // Rest of the code

    private void UpdateFuel(float newFuel)
    {
        CurrentFuel = newFuel;
        fuelChangeEvent?.Invoke();
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        CurrentFuel = FuelAmount;
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

        Rb.MovePosition(Rb.position + (movement * Speed * Time.deltaTime));
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
                if (CurrentFuel >= FuelAmount * MinFuelToStart)
                {
                    Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                    IsGrounded = false;
                }
            }
            else
            {
                if (Time.time - JetpackStartTime >= JetpackStartDelay && CurrentFuel > 0)
                {
                    Rb.AddForce(Vector3.up * JetpackForce, ForceMode.Acceleration);
                    CurrentFuel -= DrainRate * Time.deltaTime;
                    UpdateFuel(CurrentFuel);
                }

            }
        }
        else
        {
            CurrentFuel += RechargeRate * Time.deltaTime;
            CurrentFuel = Mathf.Min(CurrentFuel, FuelAmount);
            UpdateFuel(CurrentFuel);
            JetpackStartTime = 0;
        }
    }
}
