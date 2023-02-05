using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class UIManagerTests
    {
        [Test]
        public void TestShowInputNewScore()
        {
            // Arrange
            UIManager uiManager = new UIManager();
            uiManager.InputNewScoreUI = new GameObject();

            // Act
            uiManager.ShowInputNewScore(true);

            // Assert
            Assert.IsTrue(uiManager.InputNewScoreUI.activeSelf);

            // Act
            uiManager.ShowInputNewScore(false);

            // Assert
            Assert.IsFalse(uiManager.InputNewScoreUI.activeSelf);
        }

        [Test]
        public void TestShowLeaderboard()
        {
            // Arrange
            UIManager uiManager = new UIManager();
            uiManager.LeaderboardUI = new GameObject();

            // Act
            uiManager.ShowLeaderboard(true);

            // Assert
            Assert.IsTrue(uiManager.LeaderboardUI.activeSelf);

            // Act
            uiManager.ShowLeaderboard(false);

            // Assert
            Assert.IsFalse(uiManager.LeaderboardUI.activeSelf);
        }

        [Test]
        public void TestSummitNewScore()
        {
            // Arrange
            UIManager uiManager = new UIManager();

            // Act
            uiManager.SummitNewScore(100, 10f);

            // Assert
            Assert.AreEqual(100, uiManager.Score);
            Assert.AreEqual(10f, uiManager.time);
        }

    }
}
