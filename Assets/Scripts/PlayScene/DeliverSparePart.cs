using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliverSparePart : MonoBehaviour
{
    public static DeliverSparePart Instance;

    public GameObject oilPrefab;
    public GameObject shockbreakerPrefab;
    public GameObject batteryPrefab;

    public GameObject customerHouse1;
    public GameObject customerHouse2;
    public GameObject customerHouse3;
    public GameObject customerHouse4;

    public GameObject moneyPrefab; // The money prefab to be spawned
    public Transform moneyHouse1;
    public Transform moneyHouse2;
    public Transform moneyHouse3;
    public Transform moneyHouse4;
    
    public AudioClip moneySound;
    private AudioSource audioSource;

    public TMP_Text totalMoneyText; // UI Text component to display total money

    private GameObject instantiatedMoney1;
    private GameObject instantiatedMoney2;
    private GameObject instantiatedMoney3;
    private GameObject instantiatedMoney4;

    private Dictionary<GameObject, int> housePrices = new Dictionary<GameObject, int>();
     private bool taggingDone = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Store prices of spare parts in PlayerPrefs
        PlayerPrefs.SetInt("OilPrice", 50);
        PlayerPrefs.SetInt("ShockbreakerPrice", 100);
        PlayerPrefs.SetInt("BatteryPrice", 200);
        
            StartCoroutine(TagCustomersCoroutine());

    }


 private IEnumerator TagCustomersCoroutine()
    {
        TagCustomersInHouses();
        // Wait for tagging process to complete (if needed)
        yield return new WaitForSeconds(1f); // Adjust the wait time if necessary
        taggingDone = true;
    }

    public void SetCustomerScore(GameObject customerObject, int score)
    {
        if (!taggingDone)
        {
            Debug.LogWarning("Tagging not complete. Score setting is delayed.");
            StartCoroutine(WaitForTaggingAndSetScore(customerObject, score));
            return;
        }

        // Determine the appropriate key based on the customer's tag
        string scoreKey = $"{customerObject.tag}Score";
        PlayerPrefs.SetInt(scoreKey, score);
        Debug.Log($"Stored score {score} for {customerObject.tag}");
    }

    private IEnumerator WaitForTaggingAndSetScore(GameObject customerObject, int score)
    {
        while (!taggingDone)
        {
            yield return null; // Wait until the next frame
        }

        // Determine the appropriate key based on the customer's tag
        string scoreKey = $"{customerObject.tag}Score";
        PlayerPrefs.SetInt(scoreKey, score);
        Debug.Log($"Stored score {score} for {customerObject.tag}");
    }

    public void CheckAndDeliverSparePart(GameObject truckSparePart, GameObject targetHouse, Transform moneyHouse)
    {
        // Get the spare part GameObject from the truckSparePart
        GameObject sparePartObject = truckSparePart.transform.GetChild(0).gameObject;

        Debug.Log("Checking spare part: " + sparePartObject.name + " at house: " + targetHouse.name);

        // Check for spare part in the specific customer house
        if (CheckForSparePartInHouse(targetHouse, sparePartObject))
        {
            int price = GetPriceByTag(sparePartObject.tag);
            housePrices[targetHouse] = price; // Store the price for this house
            SpawnMoney(moneyHouse, targetHouse);
            Destroy(sparePartObject);
        }
        else
        {
            Debug.Log("Required spare part not found in " + targetHouse.name);
            Destroy(sparePartObject);
        }
    }

    private bool CheckForSparePartInHouse(GameObject customerHouse, GameObject truckSparePart)
    {
        string requiredTag = truckSparePart.tag;
        foreach (Transform child in customerHouse.transform)
        {
            if (child.CompareTag(requiredTag))
            {
                Debug.Log("Found spare part " + requiredTag + " in " + customerHouse.name);
                Destroy(child.gameObject); // Destroy the spare part in the customer house
                return true;
            }
        }
        return false;
    }

    private void SpawnMoney(Transform moneyHouse, GameObject customerHouse)
    {
        // Instantiate the money prefab at the money house's position
        GameObject instantiatedMoney = Instantiate(moneyPrefab, moneyHouse.position, Quaternion.identity);
        Debug.Log("Spawned money at " + moneyHouse.name);

        // Store the reference to the instantiated money object based on the customer house
        if (customerHouse == customerHouse1)
        {
            instantiatedMoney1 = instantiatedMoney;
        }
        else if (customerHouse == customerHouse2)
        {
            instantiatedMoney2 = instantiatedMoney;
        }
        else if (customerHouse == customerHouse3)
        {
            instantiatedMoney3 = instantiatedMoney;
        }
        else if (customerHouse == customerHouse4)
        {
            instantiatedMoney4 = instantiatedMoney;
        }
    }

    private int GetPriceByTag(string tag)
    {
        switch (tag)
        {
            case "Oil":
                return PlayerPrefs.GetInt("OilPrice", 50);
            case "Shockbreaker":
                return PlayerPrefs.GetInt("ShockbreakerPrice", 100);
            case "Battery":
                return PlayerPrefs.GetInt("BatteryPrice", 200);
            default:
                return 0;
        }
    }

public void TakeMoney(GameObject customerHouse)
{
    GameObject customer = null;
    string customerTag = "";

    // Find the customer within the house's hierarchy and determine their tag
    foreach (Transform child in customerHouse.transform)
    {
        if (child.CompareTag("Customer1") || child.CompareTag("Customer2") ||
            child.CompareTag("Customer3") || child.CompareTag("Customer4"))
        {
            customer = child.gameObject;
            customerTag = child.tag;
            break;
        }
    }

    // Check if the customer object is found
    if (customer == null)
    {
        Debug.LogError($"No valid customer found in {customerHouse.name}");
        return;
    }

    Debug.Log($"Found customer with tag {customer.tag} in {customerHouse.name}");

    // Retrieve the score using the customer's tag
    string scoreKey = $"{customerTag}Score";
    int score = PlayerPrefs.GetInt(scoreKey, 0);
    Debug.Log($"Retrieved score for {customerTag}: {score}");

    // Retrieve the price for this house
    int price = housePrices.ContainsKey(customerHouse) ? housePrices[customerHouse] : 0;
    Debug.Log("Retrieved price: " + price);

    // Calculate total money for this house
    int totalMoneyForHouse = score * price;
    Debug.Log("Score: " + score + ", Price: " + price + ", Total Money for house: " + totalMoneyForHouse);

    // Update the UI Text component with the total money value for this house
    totalMoneyText.text = "Total Money: " + totalMoneyForHouse;

    // Destroy the instantiated money object
    DestroyInstantiatedMoney(customerHouse);

   
    

    // After taking money from each house, update the total money
    CalculateTotalMoney();
}


private void DestroyInstantiatedMoney(GameObject customerHouse)
{
    if (customerHouse == customerHouse1 && instantiatedMoney1 != null)
    {
        Destroy(instantiatedMoney1);
        PlayMoneySound();
        instantiatedMoney1 = null;
    }
    else if (customerHouse == customerHouse2 && instantiatedMoney2 != null)
    {
        Destroy(instantiatedMoney2);
        PlayMoneySound();
        instantiatedMoney2 = null;
    }
    else if (customerHouse == customerHouse3 && instantiatedMoney3 != null)
    {
        Destroy(instantiatedMoney3);
        PlayMoneySound();
        instantiatedMoney3 = null;
    }
    else if (customerHouse == customerHouse4 && instantiatedMoney4 != null)
    {
        Destroy(instantiatedMoney4);
         PlayMoneySound();
        instantiatedMoney4 = null;
    }
}


    private int CalculateMoneyForHouse(GameObject customerHouse)
{
    // Find the customer within the house's hierarchy by tag
    GameObject customer = null;
    string customerTag = "";

    foreach (Transform child in customerHouse.transform)
    {
        if (child.CompareTag("Customer1") || child.CompareTag("Customer2") ||
            child.CompareTag("Customer3") || child.CompareTag("Customer4"))
        {
            customer = child.gameObject;
            customerTag = child.tag;
            break;
        }
    }

    // Check if the customer object is found
    if (customer == null)
    {
        Debug.LogError($"No valid customer found in {customerHouse.name}");
        return 0;
    }

    Debug.Log($"Found customer with tag {customer.tag} in {customerHouse.name}");

    // Retrieve the score using the customer's tag
    string scoreKey = $"{customerTag}Score";
    int score = PlayerPrefs.GetInt(scoreKey, 0);
    Debug.Log($"Retrieved score for {customerTag}: {score}");

    // Retrieve the price for this house
    int price = housePrices.ContainsKey(customerHouse) ? housePrices[customerHouse] : 0;
    Debug.Log("Retrieved price: " + price);

    // Calculate total money for this house
    int totalMoneyForHouse = score * price;
    Debug.Log("Score: " + score + ", Price: " + price + ", Total Money for house: " + totalMoneyForHouse);

    return totalMoneyForHouse;
}


    public int CalculateTotalMoney()
    {
        int totalMoney = 0;
        totalMoney += CalculateMoneyForHouse(customerHouse1);
        totalMoney += CalculateMoneyForHouse(customerHouse2);
        totalMoney += CalculateMoneyForHouse(customerHouse3);
        totalMoney += CalculateMoneyForHouse(customerHouse4);
        UpdateLayoutBasedOnTotalMoney(totalMoney);

        Debug.Log("Total Money from all houses: " + totalMoney);

        // Update the UI Text component with the total money value
        UpdateLayoutBasedOnTotalMoney(totalMoney);
        totalMoneyText.text = "Total Money: " + totalMoney;
        return totalMoney;
    }

   public void Endgame()
    {
     Debug.Log("tamat");
     int totalMoney = CalculateTotalMoney();  // Calculate the total money
    string playerName = PlayerPrefs.GetString("Username", "Anonymous");

    // Add the entry to the leaderboard with the totalMoney
    LeaderboardManager.Instance.AddEntry(playerName, totalMoney);

    // Clean up any specific objects that should not persist between scenes
    DestroySpecificDontDestroyOnLoadObjects();

    // Load the leaderboard scene
    SceneManager.LoadScene("LeaderboardScene");

    }

    private void Start(){
     // Tag existing customers
           CalculateTotalMoney();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();  // Ensure there is an AudioSource component
        }
    }
    private void Update()
    {
        // Monitor for newly spawned customers and tag them
          TagCustomersInHouses(); 
         
    }

    // Tag customers based on their house
    public void TagCustomersInHouses()
    {
        TagCustomerInHouse(customerHouse1, "Customer1");
        TagCustomerInHouse(customerHouse2, "Customer2");
        TagCustomerInHouse(customerHouse3, "Customer3");
        TagCustomerInHouse(customerHouse4, "Customer4");
    }

      private void TagCustomerInHouse(GameObject house, string customerTag)
    {
        foreach (Transform child in house.transform)
        {
            if (child.CompareTag("Customer"))
            {
                if (child.tag != customerTag) // Check if it's already tagged correctly
                {
                    Debug.Log($"Tagging {child.name} with {customerTag}");
                    child.tag = customerTag; // Assign the new tag
                }
            }
        }
    }
    private void UpdateLayoutBasedOnTotalMoney(int totalMoney)
{
    if (totalMoney > 5000)
    {
        PhaseManager.Instance.MoneyFull();
    }
    else if (totalMoney > 3000)
    {
        PhaseManager.Instance.MoneyHigh();
    }
    else if (totalMoney > 1500)
    {
        PhaseManager.Instance.MoneyMid();
    }
    else if (totalMoney > 500)
    {
        PhaseManager.Instance.MoneyLow();
    }
    else
    {
        // Optionally, you can hide all layouts or set to a default state if needed

}
}
    public void PlayMoneySound()
    {
        audioSource.PlayOneShot(moneySound);
    }
          public void DestroySpecificDontDestroyOnLoadObjects()
    {
        // Find all objects marked as DontDestroyOnLoad
        var allObjects = FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.scene.buildIndex == -1) // Scene index -1 indicates it's a DontDestroyOnLoad object
            {
                // Check for specific object types or names and destroy
                if (obj.GetComponent<GameTimer>() != null || obj.name == "SparePartContainer" || obj.name.StartsWith("Battery(Clone)") || obj.name.StartsWith("ShockBreaker(Clone)") || obj.name.StartsWith("Oil(Clone)")
                || obj.name.StartsWith("SFX"))
                {
                    Destroy(obj);
                }
            }
        }
    }
}
    // Method to tag newly spawned customers

