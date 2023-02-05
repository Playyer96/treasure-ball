using NUnit.Framework;
using UnityEngine;

public class PlayerMovementTests
{
    private PlayerMovement playerMovement;
    private Rigidbody rigidbody;

    [SetUp]
    public void SetUp()
    {
        rigidbody = new GameObject().AddComponent<Rigidbody>();
        playerMovement = new GameObject().AddComponent<PlayerMovement>();
        playerMovement.Rb = rigidbody;
        playerMovement.CurrentFuel = playerMovement.fuelAmount;
        playerMovement.IsUsingJetpack = 1f;

        playerMovement.speed = 5f;
        playerMovement.jumpForce = 3f;
        playerMovement.jetpackForce = 1f;
        playerMovement.rechargeRate = 1f;
        playerMovement.drainRate = 2f;
        playerMovement.fuelAmount = 1f;
        playerMovement.jetpackStartDelay = 1f;
        playerMovement.minFuelToStart = 0.3f;
    }
    
    [Test]
    public void UpdateFuel_ShouldInvokeFuelChangeEvent()
    {
        bool eventInvoked = false;
        PlayerMovement.fuelChangeEvent += () => { eventInvoked = true; };

        playerMovement.UpdateFuel(0.5f);

        Assert.IsTrue(eventInvoked);
    }

    [Test]
    public void UpdateFuel_ShouldUpdateFuelAmount()
    {
        playerMovement.UpdateFuel(0.5f);

        Assert.AreEqual(0.5f, playerMovement.CurrentFuel);
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
