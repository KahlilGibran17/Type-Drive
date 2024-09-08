using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameTimer : MonoBehaviour
{
    public static float totalTime = 230f;  // Total time in seconds
    public static float currentTime;     // Static to maintain the time across scenes
    public TMP_Text timerText;  // Reference to a UI text component that displays the time
    public Slider timerSlider;  // This will be re-assigned on scene load
    

    public static GameTimer Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentTime = totalTime;  // Initialize current time only once
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Assuming slider needs to be set only if it's not null
        if (timerSlider != null)
            timerSlider.maxValue = totalTime;
       
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (timerSlider != null)
                timerSlider.value = currentTime;
            UpdateTimerDisplay();
        }
        else
        {
            currentTime = 0;
            DeliverSparePart.Instance.Endgame();  // Call when time is up
        }

    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
            timerText.text = currentTime.ToString("F2");
    }

 



    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attempt to find the timerText and timerSlider again in the new scene
        timerText = GameObject.FindGameObjectWithTag("TimerText")?.GetComponent<TMP_Text>();
        timerSlider = GameObject.FindGameObjectWithTag("TimerSlider")?.GetComponent<Slider>();
        if (timerSlider != null)
        {
            timerSlider.maxValue = totalTime;
            timerSlider.value = currentTime;  // Ensure slider reflects current time
        }
        UpdateTimerDisplay();
    }
}



