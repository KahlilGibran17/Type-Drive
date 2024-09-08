using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Unity.VisualScripting;


public class DeclineRequest : MonoBehaviour
{
    public TMP_Text DeclineText;
    public int minWordCount = 1;
    public int maxWordCount = 1;
    private const string alphabets = "abcdefghijklmnopqrstuvwxyz"; // List of alphabet characters
    private const string numbers = "1234567890";
    private List<string> wordDeclineDatabase;
    private List<string> declineWord;
     private string declineSentence;
     private string typedDeclineSentence = "";
    private int currentIndex;

    
    // Start is called before the first frame update
    void Start()
    {
      GenerateDeclineSentence();
      LoadDeclineWordsFromDatabase();
      DeclineText = GameObject.Find("Decline").GetComponent<TMP_Text>();



    void LoadDeclineWordsFromDatabase()
    {
        
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
    void GenerateDeclineSentence()
    {
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

    }



    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
        CheckInputDecline();
        }
    }

    void CheckInputDecline()
{
    char nextChar = declineSentence[currentIndex];

    if (Input.GetKeyDown(nextChar.ToString().ToLower()))
    {



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
            RequestDecline();
        }
    }
    else if (Input.GetKeyDown(KeyCode.Backspace))
    {
        if (typedDeclineSentence.Length > 0)
         {   
            int lastRedIndex = DeclineText.text.LastIndexOf("<color=red>");
            int lastGreenIndex = DeclineText.text.LastIndexOf("<color=green>");

            if (lastGreenIndex >= 0)
            {   
                typedDeclineSentence = typedDeclineSentence.Substring(0, typedDeclineSentence.Length - 1);
                DeclineText.text = DeclineText.text.Remove(lastRedIndex, "<color=red>".Length).Insert(lastRedIndex, "<color=black>");
            }
            else if (lastRedIndex >= -1)
            {
                typedDeclineSentence = typedDeclineSentence.Substring(0, typedDeclineSentence.Length - 1);
                DeclineText.text = DeclineText.text.Remove(lastRedIndex, "<color=red>".Length).Insert(lastRedIndex, "<color=black>");
                currentIndex--;
            
            }
         }
    }
         
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
        Destroy(this.gameObject);
    }
}
