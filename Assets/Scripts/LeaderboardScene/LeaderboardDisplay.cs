using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Data;

public class LeaderboardDisplay : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText; // Make sure this is assigned in the inspector.
    public Button mainMenuButton;
    public Button removeButton;
    public Button updateButton;

    void Start()
    {
        mainMenuButton.onClick.AddListener(MainMenu);
        removeButton.onClick.AddListener(RemoveUser);
        updateButton.onClick.AddListener(UpdateLeaderboard);
        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.DisplayLeaderboard(leaderboardText);
        }
        else
        {
            Debug.LogError("LeaderboardManager instance not found.");
            // Optionally update the leaderboardText to show an error message directly in the UI.
            leaderboardText.text = "Error: Leaderboard could not be loaded.";
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void RemoveUser()
    {
    // Retrieve the username from PlayerPrefs
    string playerName = PlayerPrefs.GetString("Username", "Anonymous");
    Debug.Log("Trying to remove player: " + playerName);
    LeaderboardManager.Instance.RemovePlayerEntries(playerName);
    }
    public void UpdateLeaderboard(){
    LeaderboardManager.Instance.RefreshDisplay(leaderboardText);

    }


}
