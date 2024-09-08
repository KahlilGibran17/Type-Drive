using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;




public class SparePartTypingManagerMan : MonoBehaviour
{
    public GameObject[] SparePartOilPrefabs;
    public GameObject[] SparePartShockBreakerPrefabs;
    public GameObject[] SparePartBatteryPrefabs;
    public Transform[] spawnPoints;
    public GameObject Typo1;
    public GameObject Typo2;
    public GameObject Typo3;
    public GameObject Typo4;
    public GameObject Typo5;
    public TMP_Text inputText; // Reference to the TMP_Text component for displaying target word and input
    public TMP_Text scoreText; // Reference to the TMP_Text component for displaying score
    public TMP_Text timeText; // Reference to the TMP_Text component for displaying time
    public TMP_Text typoCount;
    public TMP_Text lifeText;
    public TMP_Text timeLabel;
    public int minWordCount = 1; // Minimum number of words in a sentence
    public int maxWordCount = 1; // Maximum number of words in a sentence
    public float typingDelay = 0.05f; // Delay between typing characters
    public float totalTime = 60f; // Total time for typing
    public int TypoLife = 5;
    public Slider timeSlider;
    
 

    private List<string> wordDatabase; // List of words loaded from the word database
    private List<string> sentenceWords; // List of words in the current sentence
    private string targetSentence; // The target sentence for the player to type
    private string typedSentence = ""; // The sentence typed by the player
    private int currentIndex; // Index of the character the player is currently typing
    private int greenIndex;
    private const string alphabets = "abcdefghijklmnopqrstuvwxyz"; // List of alphabet characters
    private const string numbers = "1234567890";
    private int score = 0; // Player's score
    private int typo = 0;
    private float timeLeft; // Time remaining for typing
    private int typoLeft;
    

    void Start()
    {


        LoadWordsFromDatabase();
        timeLeft = totalTime;
        typoLeft = TypoLife;
        timeSlider.maxValue = totalTime;
        timeSlider.value = totalTime;
        timeText.text = totalTime.ToString("F2");
        lifeText.text = TypoLife.ToString();
        timeLabel.text = "Time";
        GenerateSentence();
        UpdateTypoIndex();
        UpdateScoreText();
        SpawnSparePart();
    }

    void LoadWordsFromDatabase()
    {
        wordDatabase = new List<string>();

        string filePath = Path.Combine(Application.dataPath, "WordBank/WordDatabase.txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            wordDatabase.AddRange(lines);
        }
        else
        {
            Debug.LogError("Word database file not found at: " + filePath);
        }
    }

 void GenerateSentence()
{
    sentenceWords = new List<string>();
    targetSentence = "";

    int wordCount = Random.Range(minWordCount, maxWordCount + 1);
    for (int i = 0; i < wordCount; i++)
    {
        string word = wordDatabase[Random.Range(0, wordDatabase.Count)];
        sentenceWords.Add(word);
        targetSentence += word + " ";
        
    }

    inputText.text = "";

    // Display the target sentence with color tags for typing effect
    foreach (string word in sentenceWords)
    {
        foreach (char c in word)
        {
            inputText.text += "<color=black>" + c + "</color>";
        }
        inputText.text += " ";
    }

    // Reset current index and typed sentence
    currentIndex = 0;
    typedSentence = "";
}

   void SpawnSparePart()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Debug.Log("Spawn point selected: " + randomSpawnPoint.position);

        GameObject SparePartPrefab = null;
        int SparePartType = Random.Range(0, 3);

        switch (SparePartType)
        {
            case 0: // Oil
                SparePartPrefab = SparePartOilPrefabs[Random.Range(0, SparePartOilPrefabs.Length)];
                break;
            case 1: // Shockbreaker
                SparePartPrefab = SparePartShockBreakerPrefabs[Random.Range(0, SparePartShockBreakerPrefabs.Length)];
                break;
            case 2: // Battery
                SparePartPrefab = SparePartBatteryPrefabs[Random.Range(0, SparePartBatteryPrefabs.Length)];
                break;
        }

        if (SparePartPrefab != null)
        {
            GameObject sparepart = Instantiate(SparePartPrefab, randomSpawnPoint.position, Quaternion.identity);
            PlayerPrefs.SetInt("SparePartTypeMan",SparePartType);
            PlayerPrefs.Save();
            sparepart.transform.SetParent(transform); // Set the parent of the spare part
        }
    }
  void Update()
{
    if (timeLeft > 0)
    {
        timeLeft -= Time.deltaTime;
        timeSlider.value = timeLeft;
        timeText.text = timeLeft.ToString("F2");

        if (Input.anyKeyDown)
        {
            CheckInput();
        }

        // Check if the sentence is fully typed
    }
    else
    {
        GameOver();
    }
}
void CheckInput()
{
    if (currentIndex < targetSentence.Length)
    {
        char nextChar = targetSentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower()))
        {
            
            typedSentence += nextChar;
            inputText.text = inputText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");
            currentIndex++;

            Debug.Log("Typed correctly. currentIndex: " + currentIndex + ", targetSentence.Length: " + targetSentence.Length);
        }
                else
        {
            // Handle incorrect character
            inputText.text = inputText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
            typo++;
            SFXMan.Instance.PlayWrongSound();
            WrongLetterTyped();
            typoCount.text = "Typo: " + typo;
            currentIndex++;
            
        }
            // If all characters are typed correctly, generate a new sentence
            if (currentIndex == targetSentence.Length - 1 )
            {
                Debug.Log("All characters typed correctly. Generating new sentence.");
                SFXMan.Instance.PlayCongratsSound();
                CorrectSentenceTyped();
            }
        


    }
}








    void CorrectSentenceTyped()
    {
        score++;
        UpdateScoreText();
        GenerateSentence();
        SpawnSparePart();
    }

    void WrongLetterTyped()
    {
        TypoLife--;
        UpdateTypoIndex();

        if (TypoLife == 4)
        {
            Typo1.SetActive(true);
        }
        else if (TypoLife == 3)
        {
            Typo2.SetActive(true);
        }
        else if (TypoLife == 2)
        {
            Typo3.SetActive(true);
        }
        else if (TypoLife == 1)
        {
            Typo4.SetActive(true);
        }
        else if (TypoLife == 0)
        {
            Typo5.SetActive(true);
            GameOver();
        }
    }

    void UpdateTypoIndex()
    {
        lifeText.text = "Life: " + TypoLife;
        return;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

 void GameOver()
{
    int currentInstanceCount = PlayerPrefs.GetInt("CurrentInstanceCountMan", 0);
    int currentInstanceIndex = currentInstanceCount + 1;

    PlayerPrefs.SetInt("ScoreMan" + currentInstanceIndex, score + 5);
    PlayerPrefs.SetInt("CustomerMan" + currentInstanceIndex, 1);

    int sparePartType = PlayerPrefs.GetInt("SparePartTypeMan", -1);
    Debug.Log("Saving SparePartTypeMan: " + sparePartType + " for instance: " + currentInstanceIndex);
    PlayerPrefs.SetInt("SparePartTypeMan" + currentInstanceIndex, sparePartType);

    PlayerPrefs.Save();

    PlayerPrefs.SetInt("CurrentInstanceCountMan", currentInstanceCount + 1);

    // Other game over logic

    DestroySpecificDontDestroyOnLoadObjects();
    SceneManager.LoadScene("PlayScene");
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
                if (obj.name.StartsWith("SFX"))
                {
                    Destroy(obj);
                }
            }
        }
    }
    void OnEnable()
    {
        PlayerPrefs.SetInt("HasAccessedSparePartManScene", 1);
    }

    }







