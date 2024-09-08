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
public class BatteryStore : MonoBehaviour
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
    private List <string> wordBatteryDatabase;
    private List <string> batteryWord;
     private string typedBatterySentence = "";
    private string batterySentence;
    private int currentIndex;
  
    void Start()
    {
            LoadBatteryWordsFromDatabase();
            GenerateBatterySentence();
    }
    void LoadBatteryWordsFromDatabase()
    {
            wordBatteryDatabase = new List <string>();
            string filePath = Path.Combine(Application.dataPath, "WordBank/WordBatteryDatabase.txt");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                wordBatteryDatabase.AddRange(lines);
            }
    }
        void GenerateBatterySentence(){
        batteryWord = new List<string>();
        batterySentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordBatteryDatabase[Random.Range(0, wordBatteryDatabase.Count)];
            batteryWord.Add(word);
            batterySentence += word + " ";
        }

        BatteryText.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in batteryWord)
        {
            foreach (char c in word)
            {
                BatteryText.text += "<color=black>" + c + "</color>";
            }
            BatteryText.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedBatterySentence = "";
    }   

    // Update is called once per frame
    void Update()
    {   
        if (Input.anyKeyDown){
            CheckInputBattery();
        }
    }

    void CheckInputBattery(){
        char nextChar = batterySentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower())){

            if (currentIndex == 0 ){
                CompanyText.gameObject.SetActive(false);
                ShockBreakerText.gameObject.SetActive(false);
                OilText.gameObject.SetActive(false);
                CustomerHouse1Text.gameObject.SetActive(false);
                CustomerHouse2Text.gameObject.SetActive(false);
                CustomerHouse3Text.gameObject.SetActive(false);
                CustomerHouse4Text.gameObject.SetActive(false);
            }
            typedBatterySentence += nextChar;
              BatteryText.text = BatteryText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < batterySentence.Length && batterySentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedBatterySentence += "";
            BatteryText.text += "";
        }

        // Move to the next word
        while(currentIndex < batterySentence.Length && batterySentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= batterySentence.Length)
        {   
            CompanyText.gameObject.SetActive(true);
            ShockBreakerText.gameObject.SetActive(true);
            OilText.gameObject.SetActive(true);
            CustomerHouse1Text.gameObject.SetActive(true);
            CustomerHouse2Text.gameObject.SetActive(true);
            CustomerHouse3Text.gameObject.SetActive(true);
            CustomerHouse4Text.gameObject.SetActive(true);
            BatteryDestination();
        }
    }
    else if(Input.GetKeyDown(KeyCode.Backspace))
{
    // Reset to initial state when Backspace is pressed
    typedBatterySentence = "";
    CompanyText.gameObject.SetActive(true);
    OilText.gameObject.SetActive(true);
    ShockBreakerText.gameObject.SetActive(true);
    CustomerHouse1Text.gameObject.SetActive(true);
    CustomerHouse2Text.gameObject.SetActive(true);
    CustomerHouse3Text.gameObject.SetActive(true);
    CustomerHouse4Text.gameObject.SetActive(true);
    currentIndex = 0;

    // Assuming the initial text should be in black color
    BatteryText.text = BatteryText.text.Replace("<color=red>", "<color=black>").Replace("<color=green>", "<color=black>");
}
         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        BatteryText.text = BatteryText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        BatteryText.text = BatteryText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
}

    



      private void BatteryDestination(){
        GenerateBatterySentence();
        TruckController.Instance.SetDestination(2);
        StartCoroutine(BatteryProccess());
    }
        private IEnumerator BatteryProccess(){

        while (!TruckController.Instance.HasArrived())
        {
            yield return new WaitForSeconds(3.0f);   
        }
         PlaySceneController.Instance.SpawnSpareParts(2);
        SaveBattery();
        
        }
        private void SaveBattery()
        {
            TruckController.Instance.SetSparePart(2);
        }
    

}
