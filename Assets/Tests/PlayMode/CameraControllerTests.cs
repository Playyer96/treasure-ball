using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class CameraControllerTests
{
    private CameraController cameraController;
    private GameObject cameraObject;
    private GameObject playerObject;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject for the camera
        cameraObject = new GameObject();
        cameraObject.AddComponent<CameraController>();
        cameraController = cameraObject.GetComponent<CameraController>();

        // Create a new GameObject for the player
        playerObject = new GameObject();
        playerObject.tag = "Player";
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the camera and player game objects
        Object.Destroy(cameraObject);
        Object.Destroy(playerObject);
    }

    [Test]
    public void TestInitialDistance()
    {
        // Assert that the initial distance is set to 10
        Assert.AreEqual(10, cameraController.Distance);
    }

    [Test]
    public void TestTargetIsPlayer()
    {
        // Assert that the target of the camera is the player game object
        cameraController.Start();
        
        Assert.AreEqual(playerObject.transform, cameraController.Target);
    }

    [Test]
    public void TestCameraDistance()
    {
        // Set the distance of the camera to 20
        cameraController.Distance = 20;

        // Call the camera controller's Start method
        cameraController.Start();

        // Assert that the camera's distance is 20
        Assert.AreEqual(cameraController.Distance, 20);
    }

    [Test]
    public void Distance_Getter_ReturnsCorrectValue()
    {
        // Arrange
        CameraController cameraController = new CameraController();
        cameraController.Distance = 5f;

        // Act
        float distance = cameraController.Distance;

        // Assert
        Assert.AreEqual(5f, distance);
    }
}