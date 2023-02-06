using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class PlayerMovementTests
{
    private PlayerMovement playerMovement;
    private Rigidbody rigidbody;
    private Camera _camera;

    [SetUp]
    public void SetUp()
    {
        playerMovement = new GameObject().AddComponent<PlayerMovement>();
        rigidbody = playerMovement.gameObject.AddComponent<Rigidbody>();
        _camera = new GameObject().AddComponent<Camera>();
        playerMovement.Rb = rigidbody;
        playerMovement.CurrentFuel = playerMovement.FuelAmount;
        playerMovement.IsUsingJetpack = 1f;

        playerMovement.FuelAmount = 1f;
        playerMovement.JetpackStartDelay = 1f;
    }
    
    [TearDown]
    public void TearDown()
    {
        Object.Destroy(playerMovement);
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
        playerMovement.JetpackStartTime = Time.time - playerMovement.JetpackStartDelay + 1f;
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

        Assert.AreEqual(playerMovement.CurrentFuel, playerMovement.FuelAmount);
    }
}

