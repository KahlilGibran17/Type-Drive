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

public class CompanyStore : MonoBehaviour
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
    private List <string> wordCompanyDatabase;
    private List <string> companyWord;
     private string typedCompanySentence = "";
    private string companySentence;
    private int currentIndex;
  
    void Start()
    {
            LoadCompanyWordsFromDatabase();
            GenerateCompanySentence();
    }
    void LoadCompanyWordsFromDatabase()
    {
            wordCompanyDatabase = new List <string>();
            string filePath = Path.Combine(Application.dataPath, "WordBank/WordCompanyDatabase.txt");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                wordCompanyDatabase.AddRange(lines);
            }
    }
        void GenerateCompanySentence(){
        companyWord = new List<string>();
        companySentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordCompanyDatabase[Random.Range(0, wordCompanyDatabase.Count)];
            companyWord.Add(word);
            companySentence += word + " ";
        }

        CompanyText.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in companyWord)
        {
            foreach (char c in word)
            {
                CompanyText.text += "<color=black>" + c + "</color>";
            }
            CompanyText.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedCompanySentence = "";
    }   

    // Update is called once per frame
    void Update()
    {   
        if (Input.anyKeyDown){
            CheckInputCompany();
        }
    }

    void CheckInputCompany(){
        char nextChar = companySentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower())){

            if (currentIndex == 0 ){
                ShockBreakerText.gameObject.SetActive(false);
                BatteryText.gameObject.SetActive(false);
                OilText.gameObject.SetActive(false);
                CustomerHouse1Text.gameObject.SetActive(false);
                CustomerHouse2Text.gameObject.SetActive(false);
                CustomerHouse3Text.gameObject.SetActive(false);
                CustomerHouse4Text.gameObject.SetActive(false);
            }
            typedCompanySentence += nextChar;
              CompanyText.text = CompanyText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < companySentence.Length && companySentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedCompanySentence += "";
            CompanyText.text += "";
        }

        // Move to the next word
        while(currentIndex < companySentence.Length && companySentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= companySentence.Length)
        {   
            ShockBreakerText.gameObject.SetActive(true);
            OilText.gameObject.SetActive(true);
            BatteryText.gameObject.SetActive(true);
            CustomerHouse1Text.gameObject.SetActive(true);
            CustomerHouse2Text.gameObject.SetActive(true);
            CustomerHouse3Text.gameObject.SetActive(true);
            CustomerHouse4Text.gameObject.SetActive(true);
            CompanyDestination();
        }
    }
    else if(Input.GetKeyDown(KeyCode.Backspace))
{
    // Reset to initial state when Backspace is pressed
    typedCompanySentence = "";
    OilText.gameObject.SetActive(true);
    BatteryText.gameObject.SetActive(true);
    ShockBreakerText.gameObject.SetActive(true);
    CustomerHouse1Text.gameObject.SetActive(true);
    CustomerHouse2Text.gameObject.SetActive(true);
    CustomerHouse3Text.gameObject.SetActive(true);
    CustomerHouse4Text.gameObject.SetActive(true);
    currentIndex = 0;

    // Assuming the initial text should be in black color
    CompanyText.text = CompanyText.text.Replace("<color=red>", "<color=black>").Replace("<color=green>", "<color=black>");
}
         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        CompanyText.text = CompanyText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        CompanyText.text = CompanyText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
}

      private void CompanyDestination(){
        GenerateCompanySentence();
        TruckController.Instance.SetDestination(7);
        StartCoroutine(CompanyProccess());
    }
        private IEnumerator CompanyProccess(){

        while (!TruckController.Instance.HasArrived())
        {
            yield return new WaitForSeconds(1.0f);   
        }
         DeliverSparePart.Instance.Endgame();
  
        
        }

}
