using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;


public class OilStore: MonoBehaviour
{
    public TMP_Text CompanyText;
    public TMP_Text ShockBreakerText;
    public TMP_Text BatteryText;
    public TMP_Text CustomerHouse1Text;
    public TMP_Text CustomerHouse2Text;
    public TMP_Text CustomerHouse3Text;
    public TMP_Text CustomerHouse4Text;
    public TMP_Text OilText;
    public int minWordCount = 1;
    public int maxWordCount = 1;
    private const string alphabets ="abcdefghijklmnopqrstuvwxyz";
    private const string numbers = "123456789";
    private  List <string> wordOilDatabase;
    private List <string> oilWord;
    
    private string typedOilSentence = "";
    
    private string oilSentence;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        LoadOilWordsFromDatabase();
        GenerateOilSentence();
    }

     void LoadOilWordsFromDatabase() {
        wordOilDatabase = new List <string>();

        string filePath = Path.Combine(Application.dataPath,"WordBank/WordOilDatabase.txt" );
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            wordOilDatabase.AddRange(lines);
        }
        else
        {
            Debug.Log("File not found: " + filePath);
        }
     }

        void GenerateOilSentence(){
   
        oilWord = new List<string>();
        oilSentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordOilDatabase[Random.Range(0, wordOilDatabase.Count)];
            oilWord.Add(word);
            oilSentence += word + " ";
        }

        OilText.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in oilWord)
        {
            foreach (char c in word)
            {
                OilText.text += "<color=black>" + c + "</color>";
            }
            OilText.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedOilSentence = "";
    }
    
    // Update is called once per frame
    void Update()
    {
         if (Input.anyKeyDown){
            CheckInputOil();
    }
    void CheckInputOil(){
        char nextChar = oilSentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower())){

            if (currentIndex == 0 ){
                CompanyText.gameObject.SetActive(false);
                ShockBreakerText.gameObject.SetActive(false);
                BatteryText.gameObject.SetActive(false);
                CustomerHouse1Text.gameObject.SetActive(false);
                CustomerHouse2Text.gameObject.SetActive(false);
                CustomerHouse3Text.gameObject.SetActive(false);
                CustomerHouse4Text.gameObject.SetActive(false);
            }
            typedOilSentence += nextChar;
              OilText.text = OilText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < oilSentence.Length && oilSentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedOilSentence += "";
            OilText.text += "";
        }

        // Move to the next word
        while(currentIndex < oilSentence.Length && oilSentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= oilSentence.Length)
        {   
            CompanyText.gameObject.SetActive(true);
            ShockBreakerText.gameObject.SetActive(true);
            BatteryText.gameObject.SetActive(true);
            CustomerHouse1Text.gameObject.SetActive(true);
            CustomerHouse2Text.gameObject.SetActive(true);
            CustomerHouse3Text.gameObject.SetActive(true);
            CustomerHouse4Text.gameObject.SetActive(true);
            OilDestination();
        }
    }
   else if(Input.GetKeyDown(KeyCode.Backspace))
{
    // Reset to initial state when Backspace is pressed
    typedOilSentence = "";
    CompanyText.gameObject.SetActive(true);
    BatteryText.gameObject.SetActive(true);
    ShockBreakerText.gameObject.SetActive(true);
    CustomerHouse1Text.gameObject.SetActive(true);
    CustomerHouse2Text.gameObject.SetActive(true);
    CustomerHouse3Text.gameObject.SetActive(true);
    CustomerHouse4Text.gameObject.SetActive(true);
    currentIndex = 0;

    // Assuming the initial text should be in black color
    OilText.text = OilText.text.Replace("<color=red>", "<color=black>").Replace("<color=green>", "<color=black>");
}
         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        OilText.text = OilText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        OilText.text = OilText.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
}
}
    private void OilDestination(){
     GenerateOilSentence();
     TruckController.Instance.SetDestination(0);
     StartCoroutine(ProccesOil());
    }

    private IEnumerator ProccesOil(){
        while (!TruckController.Instance.HasArrived())
        {
            yield return new WaitForSeconds(1.0f);
        }
        PlaySceneController.Instance.SpawnSpareParts(0);
        SaveOil();
    }

    private void SaveOil(){
        TruckController.Instance.SetSparePart(0);   
        Debug.Log("Berhasil Tersimpan");
    }
}
