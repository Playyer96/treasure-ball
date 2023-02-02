using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float speed = 500;

    [SerializeField, Range(0, 10)] private float jumpForce = 3;

    [SerializeField, Range(0, 10)] private float jetpackForce = 1;

    [SerializeField, Range(0, 10)] private float rechargeRate = 1;

    [SerializeField, Range(0, 5)] private float drainRate = 2f;

    [SerializeField, Range(0, 10)] private float fuelAmount = 1;

    [SerializeField, Range(0, 5)] private float jetpackStartDelay = 1f;

    [SerializeField, Range(0, 100)] private float minFuelToStart = 30;

    private Rigidbody rb;
    private bool isGrounded = true;
    private float isUsingJetpack;
    private float jetpackStartTime = 0f;
    public float currentFuel;

    Vector2 _movement;

    public float CurrentFuel { get => currentFuel; set => currentFuel = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = fuelAmount;
    }

    private void FixedUpdate()
    {
        CheckGround();
        MovePlayer();
        Jumppack();
    }

    public void OnMovement(InputValue value) => _movement = value.Get<Vector2>();

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(_movement.x, 0f, _movement.y);

        rb.MovePosition(rb.position + (movement * speed * Time.deltaTime));
    }

    public void OnJump(InputValue value) => isUsingJetpack = value.Get<float>();

    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1f))
        {
            isGrounded = hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground");
        }
        else
        {
            isGrounded = false;
        }
    }

    void Jumppack()
    {
        if (isUsingJetpack > 0f)
        {
            if (isGrounded)
            {
                if (currentFuel >= fuelAmount * 0.3f)
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    isGrounded = false;
                    currentFuel -= drainRate * Time.deltaTime;
                }
            }
            else if (Time.time - jetpackStartTime >= jetpackStartDelay && currentFuel > 0)
            {
                rb.AddForce(Vector3.up * jetpackForce, ForceMode.Acceleration);
                currentFuel -= drainRate * Time.deltaTime;
            }
            else
            {
                jetpackStartTime = Time.time;
            }
        }
        else
        {
            currentFuel += rechargeRate * Time.deltaTime;
            currentFuel = Mathf.Min(currentFuel, fuelAmount);
        }
    }
}
