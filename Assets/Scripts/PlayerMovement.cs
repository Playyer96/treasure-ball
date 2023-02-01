using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float speed = 1;

    [SerializeField, Range(0, 10)] private float jumpForce = 3;

    [SerializeField, Range(0, 10)] private float jetpackForce = 1;

    [SerializeField, Range(0, 10)] private float rechargeRate = 1;

    [SerializeField, Range(0, 5)] private float drainRate = 2f;

    [SerializeField, Range(0, 10)] private float fuelAmount = 1;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isUsingJetpack = false;
    private float currentFuel;

    public float CurrentFuel { get => currentFuel; set => currentFuel = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = fuelAmount;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isUsingJetpack)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        else if (Input.GetKey(KeyCode.Space) && !isUsingJetpack && currentFuel > 0)
        {
            rb.AddForce(Vector3.up * jetpackForce, ForceMode.Acceleration);
            currentFuel -= drainRate * Time.deltaTime;
        }
        else
        {
            currentFuel += rechargeRate * Time.deltaTime;
            currentFuel = Mathf.Min(currentFuel, fuelAmount);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}