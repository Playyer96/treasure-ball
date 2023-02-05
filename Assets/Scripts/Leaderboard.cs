using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Leaderboard class manages the storage and display of high scores.
/// </summary>
public class Leaderboard : MonoBehaviour
{
    public int maxScores = 10;
    private string leaderboardKey = "leaderboard";
    [SerializeField] private TMP_InputField nameInputField;
    public TextMeshProUGUI[] scoreTexts;

    private List<Score> scores;

    public int HighestScore { get; private set; }    // The highest score in the leaderboard.

    private string _name;

    private void Start()
    {
        scores = new List<Score>(maxScores);
        LoadScores();
    }

    /// <summary>
    /// Loads scores from the player prefs.
    /// </summary>
    private void LoadScores()
    {
        string json = PlayerPrefs.GetString(leaderboardKey, string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            Score[] loadedScores = JsonUtility.FromJson<ScoreList>(json).scores;
            scores.AddRange(loadedScores);
            HighestScore = scores[0].score;
        }
    }

    /// <summary>
    /// Adds a new score to the leaderboard.
    /// </summary>
    /// <param name="score">The score to be added.</param>
    /// <param name="time">The time associated with the score.</param>
    public void AddNewScore(int score, float time)
    {
         name = nameInputField.text;

        // Truncate name if it's too long
        if (name.Length > 10)
        {
            name = name.Substring(0, 10);
        }

        // Create new score object
        Score newScore = new Score(name, score, time);

        // Add the new score to the list
        scores.Add(newScore);

        // Sort the list of scores
        scores.Sort((x, y) => y.score.CompareTo(x.score));

        // Remove extra scores if we have more than maxScores
        if (scores.Count > maxScores)
        {
            scores.RemoveRange(maxScores, scores.Count - maxScores);
        }

        // Save the scores to player prefs
        string json = JsonUtility.ToJson(new ScoreList(scores.ToArray()));
        PlayerPrefs.SetString(leaderboardKey, json);

        // Update the UI
        UpdateUI();
    }

    /// <summary>
    /// Updates the scores displayed in the UI.
    /// </summary>
    public void UpdateUI()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < scores.Count)
            {
                scoreTexts[i].text = string.Format("{0}. {1} - {2} ({3:F2}s)", i + 1, scores[i].name, scores[i].score,
                    scores[i].time);
                scoreTexts[i].gameObject.SetActive(true);
            }
            else
            {
                scoreTexts[i].gameObject.SetActive(false);
            }
        }
    }

    [System.Serializable]
    private class Score
    {
        public string name;
        public int score;
        public float time;

        public Score(string name, int score, float time)
        {
            this.name = name;
            this.score = score;
            this.time = time;
        }
    }

    [System.Serializable]
    private class ScoreList
    {
        public Score[] scores;

        public ScoreList(Score[] scores)
        {
            this.scores = scores;
        }
    }
}