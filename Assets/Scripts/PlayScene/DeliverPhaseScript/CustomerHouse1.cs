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
using Unity.VisualScripting.Generated.PropertyProviders;
public class CustomerHouse1 : MonoBehaviour
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
    public GameObject customerHouse1;
    public Transform moneyHouse1;

    public int minWordCount = 1;
    public int maxWordCount = 1;
    private const string alphabets ="abcdefghijklmnopqrstuvwxyz";
    private const string numbers = "123456789";
    private List <string> wordCustomerHouse1Database;
    private List <string> customerHouse1Word;

    private string typedCustomerHouse1Sentence = "";
    private string customerHouse1Sentence;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        LoadCustomerHouse1WordsFromDatabase();
        GenerateCustomerHouse1Sentence();
    }
     void LoadCustomerHouse1WordsFromDatabase(){
            wordCustomerHouse1Database = new List <string>();
            string filePath = Path.Combine(Application.dataPath, "WordBank/WordCustomerHome1Database.txt");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                wordCustomerHouse1Database.AddRange(lines);
            }
        }
     void GenerateCustomerHouse1Sentence(){
            customerHouse1Word = new List<string>();
            customerHouse1Sentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordCustomerHouse1Database[Random.Range(0, wordCustomerHouse1Database.Count)];
            customerHouse1Word.Add(word);
            customerHouse1Sentence += word + " ";
        }

        CustomerHouse1Text.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in customerHouse1Word)
        {
            foreach (char c in word)
            {
                CustomerHouse1Text.text += "<color=black>" + c + "</color>";
            }
            CustomerHouse1Text.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedCustomerHouse1Sentence = "";
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.anyKeyDown){
            CheckInputHouse1();
            
        
         }
    }
     void CheckInputHouse1(){
        char nextChar = customerHouse1Sentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower())){

            if (currentIndex == 0 ){
                CompanyText.gameObject.SetActive(false);
                OilText.gameObject.SetActive(false);
                BatteryText.gameObject.SetActive(false);
                ShockBreakerText.gameObject.SetActive(false);
                CustomerHouse2Text.gameObject.SetActive(false);
                CustomerHouse3Text.gameObject.SetActive(false);
                CustomerHouse4Text.gameObject.SetActive(false);
            }
            typedCustomerHouse1Sentence += nextChar;
              CustomerHouse1Text.text = CustomerHouse1Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < customerHouse1Sentence.Length && customerHouse1Sentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedCustomerHouse1Sentence += "";
            CustomerHouse1Text.text += "";
        }

        // Move to the next word
        while(currentIndex < customerHouse1Sentence.Length && customerHouse1Sentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= customerHouse1Sentence.Length)
        {   
            CompanyText.gameObject.SetActive(true);
            OilText.gameObject.SetActive(true);
            BatteryText.gameObject.SetActive(true);
            ShockBreakerText.gameObject.SetActive(true);
            CustomerHouse2Text.gameObject.SetActive(true);
            CustomerHouse3Text.gameObject.SetActive(true);
            CustomerHouse4Text.gameObject.SetActive(true);
            CustomerHouse1Destination();
        }
    }
    else if(Input.GetKeyDown(KeyCode.Backspace))
{
    // Reset to initial state when Backspace is pressed
    typedCustomerHouse1Sentence = "";
    CompanyText.gameObject.SetActive(true);
    OilText.gameObject.SetActive(true);
    BatteryText.gameObject.SetActive(true);
    ShockBreakerText.gameObject.SetActive(true);
    CustomerHouse2Text.gameObject.SetActive(true);
    CustomerHouse3Text.gameObject.SetActive(true);
    CustomerHouse4Text.gameObject.SetActive(true);
    currentIndex = 0;

    // Assuming the initial text should be in black color
    CustomerHouse1Text.text = CustomerHouse1Text.text.Replace("<color=red>", "<color=black>").Replace("<color=green>", "<color=black>");
}

         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        CustomerHouse1Text.text = CustomerHouse1Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        CustomerHouse1Text.text = CustomerHouse1Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
    }

    void CustomerHouse1Destination(){

        GenerateCustomerHouse1Sentence();
        
        TruckController.Instance.SetDestination(3);
        StartCoroutine(CustomerHouse1Delivery());
        DeliverSparePart.Instance.TakeMoney(customerHouse1);

    }   
        private IEnumerator CustomerHouse1Delivery()
        {
            while (!TruckController.Instance.HasArrived())
        {
            yield return new WaitForSeconds(0.5f);
        }    
        DeliverSparePart.Instance.CheckAndDeliverSparePart(truckSparePart, customerHouse1, moneyHouse1);
            Debug.Log("DELIVERED");
        }
    }



