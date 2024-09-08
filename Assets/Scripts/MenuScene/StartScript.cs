using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
   public Button startButton;
   public Button leaderboardButton;
   public Button exitButton;
   
   void Start()
    {
    leaderboardButton.onClick.AddListener(Leaderboard);
    startButton.onClick.AddListener(StartGame);
    exitButton.onClick.AddListener(Exit);

    }
   
    // Start is called before the first frame update
   public void StartGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("InputUsernameScene");
    }
    public void Leaderboard(){
        SceneManager.LoadScene("LeaderboardScene");
    }
    public void Exit(){
        Application.Quit();
    }


}
