using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
public class CustomerHouse4 : MonoBehaviour
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
    
    public GameObject customerHouse4;
    public Transform moneyHouse4;


    public int minWordCount = 1;
    public int maxWordCount = 1;
    private const string alphabets ="abcdefghijklmnopqrstuvwxyz";
    private const string numbers = "123456789";
    private List <string> wordCustomerHouse4Database;
    private List <string> customerHouse4Word;

    private string typedCustomerHouse4Sentence = "";
    private string customerHouse4Sentence;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        LoadCustomerHouse4WordsFromDatabase();
        GenerateCustomerHouse4Sentence();
    }
     void LoadCustomerHouse4WordsFromDatabase(){
            wordCustomerHouse4Database = new List <string>();
            string filePath = Path.Combine(Application.dataPath, "WordBank/WordCustomerHome4Database.txt");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                wordCustomerHouse4Database.AddRange(lines);
            }
        }
     void GenerateCustomerHouse4Sentence(){
            customerHouse4Word = new List<string>();
            customerHouse4Sentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordCustomerHouse4Database[Random.Range(0, wordCustomerHouse4Database.Count)];
            customerHouse4Word.Add(word);
            customerHouse4Sentence += word + " ";
        }

        CustomerHouse4Text.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in customerHouse4Word)
        {
            foreach (char c in word)
            {
                CustomerHouse4Text.text += "<color=black>" + c + "</color>";
            }
            CustomerHouse4Text.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedCustomerHouse4Sentence = "";
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.anyKeyDown){
            CheckInputHouse4();
        
         }
    }
     void CheckInputHouse4(){
        char nextChar = customerHouse4Sentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower())){

            if (currentIndex == 0 ){
                CompanyText.gameObject.SetActive(false);
                OilText.gameObject.SetActive(false);
                BatteryText.gameObject.SetActive(false);
                ShockBreakerText.gameObject.SetActive(false);
                CustomerHouse1Text.gameObject.SetActive(false);
                CustomerHouse2Text.gameObject.SetActive(false);
                CustomerHouse3Text.gameObject.SetActive(false);
            }
            typedCustomerHouse4Sentence += nextChar;
              CustomerHouse4Text.text = CustomerHouse4Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < customerHouse4Sentence.Length && customerHouse4Sentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedCustomerHouse4Sentence += "";
            CustomerHouse4Text.text += "";
        }

        // Move to the next word
        while(currentIndex < customerHouse4Sentence.Length && customerHouse4Sentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= customerHouse4Sentence.Length)
        {   
            CompanyText.gameObject.SetActive(true);
            OilText.gameObject.SetActive(true);
            BatteryText.gameObject.SetActive(true);
            ShockBreakerText.gameObject.SetActive(true);
            CustomerHouse1Text.gameObject.SetActive(true);
            CustomerHouse2Text.gameObject.SetActive(true);
            CustomerHouse3Text.gameObject.SetActive(true);
            CustomerHouse4Destination();
        }
    }
  else if(Input.GetKeyDown(KeyCode.Backspace))
{
    // Reset to initial state when Backspace is pressed
    typedCustomerHouse4Sentence = "";
    CompanyText.gameObject.SetActive(true);
    OilText.gameObject.SetActive(true);
    BatteryText.gameObject.SetActive(true);
    ShockBreakerText.gameObject.SetActive(true);
    CustomerHouse2Text.gameObject.SetActive(true);
    CustomerHouse3Text.gameObject.SetActive(true);
    CustomerHouse1Text.gameObject.SetActive(true);
    currentIndex = 0;

    // Assuming the initial text should be in black color
    CustomerHouse4Text.text = CustomerHouse4Text.text.Replace("<color=red>", "<color=black>").Replace("<color=green>", "<color=black>");
}
         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        CustomerHouse4Text.text = CustomerHouse4Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        CustomerHouse4Text.text = CustomerHouse4Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
    }

    void CustomerHouse4Destination(){

        GenerateCustomerHouse4Sentence();
        TruckController.Instance.SetDestination(6);
          StartCoroutine(CustomerHouse4Delivery());
         DeliverSparePart.Instance.TakeMoney(customerHouse4);


    }   
        private IEnumerator CustomerHouse4Delivery()
        {
            while (!TruckController.Instance.HasArrived())
        {
            yield return new WaitForSeconds(0.5f);
        }    
        DeliverSparePart.Instance.CheckAndDeliverSparePart(truckSparePart, customerHouse4, moneyHouse4);
            Debug.Log("DELIVERED");
        }
}
