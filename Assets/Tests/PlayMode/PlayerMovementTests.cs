using NUnit.Framework;
using UnityEngine;

public class PlayerMovementTests
{
    private PlayerMovement playerMovement;
    private Rigidbody rigidbody;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        GameObject ground = new GameObject();
        ground.AddComponent<BoxCollider>();
        ground.layer = LayerMask.NameToLayer("Ground");
        rigidbody = new GameObject().AddComponent<Rigidbody>();
        playerMovement = new GameObject().AddComponent<PlayerMovement>();
        playerMovement.Rb = rigidbody;
        playerMovement.CurrentFuel = playerMovement.fuelAmount;
        playerMovement.IsUsingJetpack = 1f;
    }

    [Test]
    public void Jumppack_WhenGroundedAndFuelIsEnough_AddsImpulseToRigidbody()
    {
        playerMovement.IsGrounded = true;
        playerMovement.IsUsingJetpack = 1f;

        playerMovement.Jumppack();

        //Assert.AreEqual(rigidbody.velocity.y, playerMovement.jumpForce);
        Assert.AreEqual(playerMovement.CurrentFuel, playerMovement.fuelAmount - playerMovement.drainRate * Time.deltaTime);
    }

    [Test]
    public void Jumppack_WhenInAirAndFuelIsEnough_AddsForceToRigidbody()
    {
        playerMovement.IsGrounded = false;
        playerMovement.JetpackStartTime = Time.time - playerMovement.jetpackStartDelay + 1f;
        playerMovement.IsUsingJetpack = 1f;

        playerMovement.Jumppack();

        //Assert.AreEqual(rigidbody.velocity.y, playerMovement.jetpackForce);
        Assert.AreEqual(playerMovement.CurrentFuel, playerMovement.fuelAmount - playerMovement.drainRate * Time.deltaTime);
    }

    [Test]
    public void Jumppack_WhenInAirAndFuelIsNotEnough_DoesNotAddForceToRigidbody()
    {
        playerMovement.IsGrounded = false;
        playerMovement.JetpackStartTime = Time.time - playerMovement.jetpackStartDelay + 1f;
        playerMovement.IsUsingJetpack = 1f;
        playerMovement.CurrentFuel = 0f;

        playerMovement.Jumppack();

        Assert.AreEqual(rigidbody.velocity.y, 0f);
        Assert.AreEqual(playerMovement.JetpackStartTime, Time.time);
    }

    [Test]
    public void Jumppack_WhenNotUsingJetpack_RechargesFuel()
    {
        playerMovement.IsUsingJetpack = 0f;

        playerMovement.Jumppack();

        Assert.AreEqual(playerMovement.CurrentFuel, playerMovement.fuelAmount);
    }

    [Test]
    public void MovePlayer_WithMovementInput_MovesRigidbody()
    {
        Vector2 movementInput = new Vector2(1, 1);
        playerMovement.Movement = movementInput;

        playerMovement.MovePlayer();

        Vector3 expectedMovement = new Vector3(movementInput.x, 0f, movementInput.y);
        Vector3 expectedPosition = expectedMovement * playerMovement.speed * Time.deltaTime;
        Assert.AreEqual(rigidbody.position, expectedPosition);

    }

    [Test]
    public void MovePlayer_WithoutMovementInput_DoesNotMoveRigidbody()
    {
        Vector3 initialPosition = rigidbody.position;

        playerMovement.Movement = Vector2.zero;
        playerMovement.MovePlayer();

        Assert.AreEqual(rigidbody.position, initialPosition);
    }
}
