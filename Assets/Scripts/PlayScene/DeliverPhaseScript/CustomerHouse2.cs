using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
public class CustomerHouse2 : MonoBehaviour
{
    public GameObject truckSparePart;
    public TMP_Text CompanyText;
    public TMP_Text OilText;
    public TMP_Text BatteryText;
    public TMP_Text ShockBreakerText;
    public TMP_Text CustomerHouse1Text;
    public TMP_Text CustomerHouse2Text;
    public TMP_Text CustomerHouse3Text;
    public TMP_Text CustomerHouse4Text;
    public GameObject customerHouse2;
    public Transform moneyHouse2;

    public int minWordCount = 1;
    public int maxWordCount = 1;
    private const string alphabets ="abcdefghijklmnopqrstuvwxyz";
    private const string numbers = "123456789";
    private List <string> wordCustomerHouse2Database;
    private List <string> customerHouse2Word;

    private string typedCustomerHouse2Sentence = "";
    private string customerHouse2Sentence;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        LoadCustomerHouse2WordsFromDatabase();
        GenerateCustomerHouse2Sentence();
    }
     void LoadCustomerHouse2WordsFromDatabase(){
            wordCustomerHouse2Database = new List <string>();
            string filePath = Path.Combine(Application.dataPath, "WordBank/WordCustomerHome2Database.txt");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                wordCustomerHouse2Database.AddRange(lines);
            }
        }
     void GenerateCustomerHouse2Sentence(){
            customerHouse2Word = new List<string>();
            customerHouse2Sentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordCustomerHouse2Database[Random.Range(0, wordCustomerHouse2Database.Count)];
            customerHouse2Word.Add(word);
            customerHouse2Sentence += word + " ";
        }

        CustomerHouse2Text.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in customerHouse2Word)
        {
            foreach (char c in word)
            {
                CustomerHouse2Text.text += "<color=black>" + c + "</color>";
            }
            CustomerHouse2Text.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedCustomerHouse2Sentence = "";
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.anyKeyDown){
            CheckInputHouse2();
        
         }
    }
     void CheckInputHouse2(){
        char nextChar = customerHouse2Sentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower())){

            if (currentIndex == 0 ){
                CompanyText.gameObject.SetActive(false);
                OilText.gameObject.SetActive(false);
                BatteryText.gameObject.SetActive(false);
                ShockBreakerText.gameObject.SetActive(false);
                CustomerHouse1Text.gameObject.SetActive(false);
                CustomerHouse3Text.gameObject.SetActive(false);
                CustomerHouse4Text.gameObject.SetActive(false);
            }
            typedCustomerHouse2Sentence += nextChar;
              CustomerHouse2Text.text = CustomerHouse2Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < customerHouse2Sentence.Length && customerHouse2Sentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedCustomerHouse2Sentence += "";
            CustomerHouse2Text.text += "";
        }

        // Move to the next word
        while(currentIndex < customerHouse2Sentence.Length && customerHouse2Sentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= customerHouse2Sentence.Length)
        {   
            CompanyText.gameObject.SetActive(true);
            OilText.gameObject.SetActive(true);
            BatteryText.gameObject.SetActive(true);
            ShockBreakerText.gameObject.SetActive(true);
            CustomerHouse1Text.gameObject.SetActive(true);
            CustomerHouse3Text.gameObject.SetActive(true);
            CustomerHouse4Text.gameObject.SetActive(true);
            CustomerHouse2Destination();
        }
    }
    else if(Input.GetKeyDown(KeyCode.Backspace))
{
    // Reset to initial state when Backspace is pressed
    typedCustomerHouse2Sentence = "";
    CompanyText.gameObject.SetActive(true);
    OilText.gameObject.SetActive(true);
    BatteryText.gameObject.SetActive(true);
    ShockBreakerText.gameObject.SetActive(true);
    CustomerHouse1Text.gameObject.SetActive(true);
    CustomerHouse3Text.gameObject.SetActive(true);
    CustomerHouse4Text.gameObject.SetActive(true);
    currentIndex = 0;

    // Assuming the initial text should be in black color
    CustomerHouse2Text.text = CustomerHouse2Text.text.Replace("<color=red>", "<color=black>").Replace("<color=green>", "<color=black>");
}
         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        CustomerHouse2Text.text = CustomerHouse2Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        CustomerHouse2Text.text = CustomerHouse2Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
    }

    void CustomerHouse2Destination(){

        GenerateCustomerHouse2Sentence();
        TruckController.Instance.SetDestination(4);
        StartCoroutine(CustomerHouse2Delivery());
        DeliverSparePart.Instance.TakeMoney(customerHouse2);


    }   
        private IEnumerator CustomerHouse2Delivery()
        {
            while (!TruckController.Instance.HasArrived())
        {
            yield return new WaitForSeconds(0.5f);
        }    
        DeliverSparePart.Instance.CheckAndDeliverSparePart(truckSparePart, customerHouse2, moneyHouse2);
            Debug.Log("DELIVERED");
        }
}
