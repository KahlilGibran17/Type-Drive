using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
public class CustomerHouse3 : MonoBehaviour
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
    
    public GameObject customerHouse3;
    public Transform moneyHouse3;

    public int minWordCount = 1;
    public int maxWordCount = 1;
    private const string alphabets ="abcdefghijklmnopqrstuvwxyz";
    private const string numbers = "123456789";
    private List <string> wordCustomerHouse3Database;
    private List <string> customerHouse3Word;

    private string typedCustomerHouse3Sentence = "";
    private string customerHouse3Sentence;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        LoadCustomerHouse3WordsFromDatabase();
        GenerateCustomerHouse3Sentence();
    }
     void LoadCustomerHouse3WordsFromDatabase(){
            wordCustomerHouse3Database = new List <string>();
            string filePath = Path.Combine(Application.dataPath, "WordBank/WordCustomerHome3Database.txt");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                wordCustomerHouse3Database.AddRange(lines);
            }
        }
     void GenerateCustomerHouse3Sentence(){
            customerHouse3Word = new List<string>();
            customerHouse3Sentence = "";

        int wordCount = Random.Range(minWordCount, maxWordCount + 1);
        for (int i = 0; i < wordCount; i++)
        {
            string word = wordCustomerHouse3Database[Random.Range(0, wordCustomerHouse3Database.Count)];
            customerHouse3Word.Add(word);
            customerHouse3Sentence += word + " ";
        }

        CustomerHouse3Text.text = "";

        // Display the target sentence with color tags for typing effect
        foreach (string word in customerHouse3Word)
        {
            foreach (char c in word)
            {
                CustomerHouse3Text.text += "<color=black>" + c + "</color>";
            }
            CustomerHouse3Text.text += " ";
        }

        // Reset current index and typed sentence
        currentIndex = 0;
        typedCustomerHouse3Sentence = "";
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.anyKeyDown){
            CheckInputHouse3();
        
         }
    }
     void CheckInputHouse3(){
        char nextChar = customerHouse3Sentence[currentIndex];

        if (Input.GetKeyDown(nextChar.ToString().ToLower())){

            if (currentIndex == 0 ){
                CompanyText.gameObject.SetActive(false);
                OilText.gameObject.SetActive(false);
                BatteryText.gameObject.SetActive(false);
                ShockBreakerText.gameObject.SetActive(false);
                CustomerHouse1Text.gameObject.SetActive(false);
                CustomerHouse2Text.gameObject.SetActive(false);
                CustomerHouse4Text.gameObject.SetActive(false);
            }
            typedCustomerHouse3Sentence += nextChar;
              CustomerHouse3Text.text = CustomerHouse3Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=green>" + nextChar + "</color>");

        currentIndex++;
        // Check if the current word is complete
        if (currentIndex < customerHouse3Sentence.Length && customerHouse3Sentence[currentIndex] == ' ')
        {
            currentIndex++;
            typedCustomerHouse3Sentence += "";
            CustomerHouse3Text.text += "";
        }

        // Move to the next word
        while(currentIndex < customerHouse3Sentence.Length && customerHouse3Sentence[currentIndex] == ' ')
        { 
            currentIndex++;
        }

        // If all words are complete, generate a new sentence
        if (currentIndex >= customerHouse3Sentence.Length)
        {   
            CompanyText.gameObject.SetActive(true);
            OilText.gameObject.SetActive(true);
            BatteryText.gameObject.SetActive(true);
            ShockBreakerText.gameObject.SetActive(true);
            CustomerHouse1Text.gameObject.SetActive(true);
            CustomerHouse2Text.gameObject.SetActive(true);
            CustomerHouse4Text.gameObject.SetActive(true);
            CustomerHouse3Destination();
        }
    }
   else if(Input.GetKeyDown(KeyCode.Backspace))
{
    // Reset to initial state when Backspace is pressed
    typedCustomerHouse3Sentence = "";
    CompanyText.gameObject.SetActive(true);
    OilText.gameObject.SetActive(true);
    BatteryText.gameObject.SetActive(true);
    ShockBreakerText.gameObject.SetActive(true);
    CustomerHouse2Text.gameObject.SetActive(true);
    CustomerHouse1Text.gameObject.SetActive(true);
    CustomerHouse4Text.gameObject.SetActive(true);
    currentIndex = 0;

    // Assuming the initial text should be in black color
    CustomerHouse3Text.text = CustomerHouse3Text.text.Replace("<color=red>", "<color=black>").Replace("<color=green>", "<color=black>");
}
         
     else if (alphabets.Contains(nextChar.ToString().ToLower()))
    {
        CustomerHouse3Text.text = CustomerHouse3Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
     else if (numbers.Contains(nextChar.ToString().ToLower()))
    {
        CustomerHouse3Text.text = CustomerHouse3Text.text.ReplaceFirst("<color=black>" + nextChar + "</color>", "<color=red>" + nextChar + "</color>");
    }
    }

    void CustomerHouse3Destination(){

        GenerateCustomerHouse3Sentence();
        TruckController.Instance.SetDestination(5);
        StartCoroutine(CustomerHouse3Delivery());
         DeliverSparePart.Instance.TakeMoney(customerHouse3);


    }   
        private IEnumerator CustomerHouse3Delivery()
        {
            while (!TruckController.Instance.HasArrived())
        {
            yield return new WaitForSeconds(0.5f);
        }    
        DeliverSparePart.Instance.CheckAndDeliverSparePart(truckSparePart, customerHouse3, moneyHouse3);
            Debug.Log("DELIVERED");
        }

}
