using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;

    public LeaderboardEntry(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}

[System.Serializable]
public class Leaderboard
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;
    private string leaderboardFilePath;
    public Leaderboard leaderboard = new Leaderboard();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        leaderboardFilePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        LoadLeaderboard();
    }

    public void AddEntry(string playerName, int totalMoney)
    {
    leaderboard.entries.Add(new LeaderboardEntry(playerName, totalMoney));
    leaderboard.entries = leaderboard.entries.OrderByDescending(e => e.score).ToList();
    if (leaderboard.entries.Count > 10) // Keep only the top 10 entries
        leaderboard.entries.RemoveRange(10, leaderboard.entries.Count - 10);
    SaveLeaderboard();
    }


    public void RemovePlayerEntries(string playerName)
    {
        // Remove all entries for a specific player
        leaderboard.entries.Clear();
        SaveLeaderboard();

        Debug.Log("Removed all entries for player: " + playerName);
    }

   private void SaveLeaderboard()
{
    string json = JsonUtility.ToJson(leaderboard, true);
    File.WriteAllText(leaderboardFilePath, json);
    Debug.Log("Leaderboard saved: " + json);
}

private void LoadLeaderboard()
{
    if (File.Exists(leaderboardFilePath))
    {
        string json = File.ReadAllText(leaderboardFilePath);
        leaderboard = JsonUtility.FromJson<Leaderboard>(json);
        Debug.Log("Leaderboard loaded: " + json);
    }
    else
    {
        Debug.LogError("Leaderboard file not found");
    }
}

public void RefreshDisplay(TMPro.TextMeshProUGUI displayText)
{
    string output = "";
    foreach (var entry in leaderboard.entries)
    {
        output += $"{entry.playerName}: {entry.score}\n";
    }
    displayText.text = output; // Update the display text
}



  public void DisplayLeaderboard(TMPro.TextMeshProUGUI displayText)
{
    string output = "";
    foreach (var entry in leaderboard.entries)
    {
        output += $"{entry.playerName}: {entry.score}\n";
    }
    displayText.text = output;
}

}
