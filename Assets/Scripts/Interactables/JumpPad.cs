using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                // Add an upward force to the player's rigidbody with an impulse force mode.
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                
                StartCoroutine(CleanForces(rb));
            }
        }
    }

    IEnumerator CleanForces(Rigidbody rb)
    {
        // Wait for 1 second.
        yield return new WaitForSeconds(1f);
        
        // Reset the velocity and angular velocity of the player's rigidbody.
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}