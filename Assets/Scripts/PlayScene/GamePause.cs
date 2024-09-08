using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GamePause : MonoBehaviour
{
    public GameObject PauseScreen;
    private bool isPauseScreenActive = false;

    void Update()
    {
        // Check if the Escape key is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroySpecificDontDestroyOnLoadObjects();
             SceneManager.LoadScene("MenuScene");
            // TogglePause();
        }
    }
              public void DestroySpecificDontDestroyOnLoadObjects()
    {
        // Find all objects marked as DontDestroyOnLoad
        var allObjects = FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.scene.buildIndex == -1) // Scene index -1 indicates it's a DontDestroyOnLoad object
            {
                // Check for specific object types or names and destroy
                if (obj.GetComponent<GameTimer>() != null || obj.name == "SparePartContainer" || obj.name.StartsWith("Battery(Clone)") || obj.name.StartsWith("ShockBreaker(Clone)") || obj.name.StartsWith("Oil(Clone)")
                || obj.name.StartsWith("SFX"))
                {
                    Destroy(obj);
                }
            }
        }
    }

    // private void TogglePause()
    // {
    //     isPauseScreenActive = !isPauseScreenActive;  // Toggle the state of isPauseScreenActive.
    //     PauseScreen.SetActive(isPauseScreenActive);  // Activate or deactivate the pause screen.
        
    //     // Set Time.timeScale to 0 to pause the game, 1 to resume.
    //     Time.timeScale = isPauseScreenActive ? 0 : 1;
    //     AudioListener.pause = isPauseScreenActive? true : false ;

    // }
}
