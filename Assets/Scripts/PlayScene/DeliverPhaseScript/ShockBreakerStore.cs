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

public class ShockBreakerStore : MonoBehaviour
{
    public TMP_Text OilText;
    public TMP_Text ShockBreakerText;
    public TMP_Text BatteryText;
    public TMP_Text CompanyText;
    public TMP_Text CustomerHouse1Text;
    public TMP_Text CustomerHouse2Text;
    public TMP_Text CustomerHouse3Text;
    public TMP_Text CustomerHouse4Text;
    public int minWordCount = 1;
    public int maxWordCount = 1;

    private const string alphabets ="abcdefghijklmnopqrstuvwxyz";
    private const string numbers = "123456789";
   
      private List <string> wordShockBreakerDatabase;
    private List <string> shockbreakerWord;
    private string typedShockBreakerSentence = "";

    private string shockbreakerSentence;
     private int currentIndex;


    // Start is called before the first frame update
    void Start()
    {

    LoadShockBreakerWordsFromDatabase();
    GenerateShockBreakerSentence();

  

    }



     void LoadShockBreakerWordsFromDatabase (){
        wordShockBreakerDatabase = new List <string>();

        string filePath = Path.Combine(Application.dataPath, "WordBank/WordShockBreakerDatabase.txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            wordShockBreakerDatabase.AddRange(lines);
        }
        else
        {
            Debug.Log("File not found" + filePath);
        }
     }
     

  
    void GenerateShockBreakerSentence(){
        shockbreakerWord = new List<string>();
        shockbreakerSentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordShockBreakerDatabase[Random.Range(0, wordShockBreakerDatabase.Count)];
            shockbreakerWord.Add(word);
            shockbreakerSentence += word + " ";
        }

        ShockBreakerText.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in shockbreakerWord)
        {
            foreach (char c in word)
            {
                ShockBreakerText.text += "<color=black>" + c + "</color>";
            }
            ShockBreakerText.text += " ";
        }

        // RtypedShockBreaker index and typed sentence
        currentIndex = 0;
        typedShockBreakerSentence = "";
    }


    // Update is called once per frame
 
       
    void Update()
    {
     if (Input.anyKeyDown){
        CheckInputShockBreaker();
     }   
    }

    
    void CheckInputShockBreaker(){
        char nextChar = shockbreakerSentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower())){

            if (currentIndex == 0 ){
                CompanyText.gameObject.SetActive(false);
                OilText.gameObject.SetActive(false);
                BatteryText.gameObject.SetActive(false);
                CustomerHouse1Text.gameObject.SetActive(false);
                CustomerHouse2Text.gameObject.SetActive(false);
                CustomerHouse3Text.gameObject.SetActive(false);
                CustomerHouse4Text.gameObject.SetActive(false);
            }
            typedShockBreakerSentence += nextChar;
              ShockBreakerText.text = ShockBreakerText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < shockbreakerSentence.Length && shockbreakerSentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedShockBreakerSentence += "";
            ShockBreakerText.text += "";
        }

        // Move to the next word
        while(currentIndex < shockbreakerSentence.Length && shockbreakerSentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= shockbreakerSentence.Length)
        {   
            CompanyText.gameObject.SetActive(true);
            OilText.gameObject.SetActive(true);
            BatteryText.gameObject.SetActive(true);
            CustomerHouse1Text.gameObject.SetActive(true);
            CustomerHouse2Text.gameObject.SetActive(true);
            CustomerHouse3Text.gameObject.SetActive(true);
            CustomerHouse4Text.gameObject.SetActive(true);
            ShockBreakerDestination();
        }
    }
   else if(Input.GetKeyDown(KeyCode.Backspace))
{
    // Reset to initial state when Backspace is pressed
    typedShockBreakerSentence = "";
    CompanyText.gameObject.SetActive(true);
    BatteryText.gameObject.SetActive(true);
    OilText.gameObject.SetActive(true);
    CustomerHouse1Text.gameObject.SetActive(true);
    CustomerHouse2Text.gameObject.SetActive(true);
    CustomerHouse3Text.gameObject.SetActive(true);
    CustomerHouse4Text.gameObject.SetActive(true);
    currentIndex = 0;

    // Assuming the initial text should be in black color
    ShockBreakerText.text = ShockBreakerText.text.Replace("<color=red>", "<color=black>").Replace("<color=green>", "<color=black>");
}
         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        ShockBreakerText.text = ShockBreakerText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        ShockBreakerText.text = ShockBreakerText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
    }
  
    private void ShockBreakerDestination(){
        
        GenerateShockBreakerSentence();
        TruckController.Instance.SetDestination(1);
        StartCoroutine(ShockBreakerProccess());
    }

    private IEnumerator ShockBreakerProccess(){
        while (!TruckController.Instance.HasArrived()){
            yield return new WaitForSeconds(2.0f);
        }
        PlaySceneController.Instance.SpawnSpareParts(1);
        SaveShockBreaker();
    }
    private void SaveShockBreaker(){
        TruckController.Instance.SetSparePart(1);
    }

}