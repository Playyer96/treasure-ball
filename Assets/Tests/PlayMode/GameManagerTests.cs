using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class GameManagerTests
{
    private GameManager _gameManager;

    [SetUp]
    public void Setup()
    {
        _gameManager = new GameManager();
    }

    [Test]
    public void CollectCoin_ShouldIncreaseCoinsCollected()
    {
        // Arrange
        int expectedCoinsCollected = 10;
        _gameManager.CoinValue = 10;

        // Act
        _gameManager.CollectCoin();

        // Assert
        Assert.AreEqual(expectedCoinsCollected, _gameManager.CoinsCollected);
    }

    [Test]
    public void CollectKey_ShouldIncreaseKeysCollected()
    {
        // Arrange
        int expectedKeysCollected = 50;
        _gameManager.KeyValue = 50;

        // Act
        _gameManager.CollectKey();

        // Assert
        Assert.AreEqual(expectedKeysCollected, _gameManager.KeysCollected);
    }

    [Test]
    public void CollectCoinAndKey_ShouldIncreaseTotalCollected()
    {
        // Arrange
        int expectedTotalCollected = 60;
        _gameManager.CoinValue = 10;
        _gameManager.KeyValue = 50;

        // Act
        _gameManager.CollectCoin();
        _gameManager.CollectKey();

        // Assert
        Assert.AreEqual(expectedTotalCollected, _gameManager.totalCollected);
    }
}