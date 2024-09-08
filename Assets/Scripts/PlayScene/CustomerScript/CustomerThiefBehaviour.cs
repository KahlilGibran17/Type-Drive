using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;



public class CustomerThiefBehaviour : MonoBehaviour {
    
    public TMP_Text AcceptText; 
    public TMP_Text DeclineText;
    public NotificationButton notificationButton;
    public int minWordCount = 1;
    public int maxWordCount = 1;

    private List<string> wordAcceptDatabase;
    private List<string> acceptWord;
    private List<string> wordDeclineDatabase;
    private List<string> declineWord;
    private const string alphabets = "abcdefghijklmnopqrstuvwxyz"; // List of alphabet characters
    private const string numbers = "1234567890";
    private string typedAcceptSentence = "";
    private string typedDeclineSentence = "";
    private string acceptSentence;
    private string declineSentence;
    private int currentIndex;
    private int customerCount;
    public CustomerManager customerManager;
    
    

    // Start is called before the first frame update

void Start()
{
   
    LoadAcceptWordsFromDatabase();
    GenerateAcceptSentence();
    LoadDeclineWordsFromDatabase();
    GenerateDeclineSentence();
    customerManager = FindObjectOfType<CustomerManager>();
    Debug.Log("AcceptText: " + AcceptText);
    Debug.Log("DeclineText: " + DeclineText);


   
}


                                        
    

 void LoadAcceptWordsFromDatabase()
    {
        wordAcceptDatabase = new List<string>();

        string filePath = Path.Combine(Application.dataPath, "WordBank/WordAcceptDatabase.txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            wordAcceptDatabase.AddRange(lines);
        }
        else
        {
            Debug.LogError("Word database file not found at: " + filePath);
        }
    }

        void LoadDeclineWordsFromDatabase(){
        
        wordDeclineDatabase = new List<string>();

            string filePath = Path.Combine(Application.dataPath, "WordBank/WordDeclineDatabase.txt");
            if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            wordDeclineDatabase.AddRange(lines);
        }
        else
        {
            Debug.LogError("Word database file not found at: " + filePath);
        }

    }

    
 void GenerateAcceptSentence()
    {
        acceptWord = new List<string>();
        acceptSentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordAcceptDatabase[Random.Range(0, wordAcceptDatabase.Count)];
            acceptWord.Add(word);
            acceptSentence += word + " ";
        }

        AcceptText.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in acceptWord)
        {
            foreach (char c in word)
            {
                AcceptText.text += "<color=black>" + c + "</color>";
            }
            AcceptText.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedAcceptSentence = "";
    }

    void GenerateDeclineSentence(){
        declineWord = new List<string>();
        declineSentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordDeclineDatabase[Random.Range(0, wordDeclineDatabase.Count)];
            declineWord.Add(word);
            declineSentence += word + " ";
        }

        DeclineText.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in declineWord)
        {
            foreach (char c in word)
            {
                DeclineText.text += "<color=black>" + c + "</color>";
            }
            DeclineText.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedDeclineSentence = "";
    }

    // Update is called once per frame
    void Update()
    {   if (Input.anyKeyDown)
    {
        CheckInputAccept();
        CheckInputDecline();
    }
    }

 
 void CheckInputAccept()
{
    char nextChar = acceptSentence[currentIndex];

    if (Input.GetKeyDown(nextChar.ToString().ToLower()))
    {

        if (currentIndex == 0)
        {
            DeclineText.gameObject.SetActive(false);
        }

        typedDeclineSentence += nextChar;

        AcceptText.text = AcceptText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < acceptSentence.Length && acceptSentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedAcceptSentence += "";
            AcceptText.text += "";
        }

        // Move to the next word
        while(currentIndex < acceptSentence.Length && acceptSentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= acceptSentence.Length)
        {   
            DeclineText.gameObject.SetActive(true);
            RequestAccept();
        }
    }
    else if (Input.GetKeyDown(KeyCode.Backspace))
    {
        if (typedAcceptSentence.Length > 0)
         {   
            int lastRedIndex = AcceptText.text.LastIndexOf("<color=red>");
            int lastGreenIndex = AcceptText.text.LastIndexOf("<color=green>");

            if (lastGreenIndex >= 0)
            {   
                typedAcceptSentence = typedAcceptSentence.Substring(0, typedAcceptSentence.Length - 1);
                AcceptText.text = AcceptText.text.Remove(lastRedIndex, "<color=red>".Length).Insert(lastRedIndex, "<color=black>");
            }
            else if (lastRedIndex >= -1)
            {
                typedAcceptSentence = typedAcceptSentence.Substring(0, typedAcceptSentence.Length - 1);
                AcceptText.text = AcceptText.text.Remove(lastRedIndex, "<color=red>".Length).Insert(lastRedIndex, "<color=black>");
                currentIndex--;
            
            }
         }
    }
         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        AcceptText.text = AcceptText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        AcceptText.text = AcceptText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
}
         
 void CheckInputDecline()
{
    char nextChar = declineSentence[currentIndex];

    // Check if the typed character matches the expected next character in the declineSentence
    if (Input.GetKeyDown(nextChar.ToString().ToLower()))
    {
        // Deactivate AcceptText only when the first character of the declineSentence is being typed
        if (currentIndex == 0)
        {
            AcceptText.gameObject.SetActive(false);
        }

        typedDeclineSentence += nextChar;

        DeclineText.text = DeclineText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        
        // Check if the current word is complete
        if (currentIndex < declineSentence.Length && declineSentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedDeclineSentence += "";
            DeclineText.text += "";
        }

        // Move to the next word
        while(currentIndex < declineSentence.Length && declineSentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= declineSentence.Length)
        {   
            AcceptText.gameObject.SetActive(true);
            RequestDecline();
        }
    }
    // Handle backspace input
    else if (Input.GetKeyDown(KeyCode.Backspace))
    {
        if (typedAcceptSentence.Length > 0)
         {   
            int lastRedIndex = AcceptText.text.LastIndexOf("<color=red>");
            int lastGreenIndex = AcceptText.text.LastIndexOf("<color=green>");

            if (lastGreenIndex >= 0)
            {   
                typedAcceptSentence = typedAcceptSentence.Substring(0, typedAcceptSentence.Length - 1);
                AcceptText.text = AcceptText.text.Remove(lastRedIndex, "<color=red>".Length).Insert(lastRedIndex, "<color=black>");
            }
            else if (lastRedIndex >= -1)
            {
                typedAcceptSentence = typedAcceptSentence.Substring(0, typedAcceptSentence.Length - 1);
                AcceptText.text = AcceptText.text.Remove(lastRedIndex, "<color=red>".Length).Insert(lastRedIndex, "<color=black>");
                currentIndex--;
            
            }
         }
    }
    // Handle incorrect input (non-alphabetic characters)
    else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        DeclineText.text = DeclineText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
    else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        DeclineText.text = DeclineText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
}


    void RequestDecline(){
       // customerManager.customerCount--;
        // notificationButton.UpdateNotification(notificationButton.notificationImage - 1);

        Destroy(this.gameObject);
      

    }
    

    void RequestAccept(){
        Destroy(this.gameObject);
        // PlayerPrefs.Save      
        DestroySpecificDontDestroyOnLoadObjects();
        SceneManager.LoadScene("SparePartThiefScene");

        
    }
        void DestroySpecificDontDestroyOnLoadObjects()
    {
    // Find all objects marked as DontDestroyOnLoad
    var allObjects = FindObjectsOfType<GameObject>();
    foreach (var obj in allObjects)
    {
        if (obj.scene.buildIndex == -1) // Scene index -1 indicates it's a DontDestroyOnLoad object
        {
            // Check for specific object types or names and destroy
            if (obj.GetComponent<SFXPlayScene>())
            {
                Destroy(obj);

            }
        }
    }
}
}
