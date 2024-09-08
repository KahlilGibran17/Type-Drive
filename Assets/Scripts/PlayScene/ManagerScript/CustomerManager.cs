using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class CustomerManager : MonoBehaviour
{
    public GameObject[] customerGirlPrefabs; // Array of customer girl prefabsF
    public GameObject[] customerManPrefabs; // Array of customer man prefabs
    public GameObject[] customerThiefPrefabs; // Array of customer Thief Prefabs
    public Transform[] spawnPoints;
    public NotificationButton notificationButton; // Reference to the notification button

    public int customerCount = 0;
    private bool isFirstSpawn = true; // Flag to track the first spawn
    public float firstSpawnInterval = 10f; // Interval for the first spawn
    public float regularSpawnInterval = 5f; // Regular spawn interval after the first spawn

    private static CustomerManager instance; // Singleton instance
    private List<GameObject> spawnedCustomers = new List<GameObject>(); // List to keep track of spawned customers

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned to CustomerManager.");
            return;
        }

        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        // Wait for the first spawn interval if it's the first spawn
        if (isFirstSpawn)
        {
            yield return new WaitForSeconds(firstSpawnInterval);
            isFirstSpawn = false; // Update flag after the first spawn
        }

        while (true)
        {
            GenerateCustomerRequest();
            yield return new WaitForSeconds(regularSpawnInterval);
        }
    }

private void GenerateCustomerRequest()
{
    Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
    Debug.Log("Spawn point selected: " + randomSpawnPoint.position);

    GameObject customerPrefab = null;
    int customerType = Random.Range(0, 3); // 0: girl, 1: man, 2: thief

    switch (customerType)
    {
        case 0: // Girl
            customerPrefab = customerGirlPrefabs[Random.Range(0, customerGirlPrefabs.Length)];
            break;
        case 1: // Man
            customerPrefab = customerManPrefabs[Random.Range(0, customerManPrefabs.Length)];
            break;
        case 2: // Thief
            customerPrefab = customerThiefPrefabs[Random.Range(0, customerThiefPrefabs.Length)];
            break;
    }

    if (customerPrefab != null)
    {
        GameObject spawnedCustomer = Instantiate(customerPrefab, randomSpawnPoint.position, Quaternion.identity);

        // Attach appropriate behavior script based on the type of the spawned customer
        MonoBehaviour customerBehaviour = null;
        if (customerType == 0) // Girl
        {
            CustomerGirlBehaviour customerGirlBehaviour = spawnedCustomer.AddComponent<CustomerGirlBehaviour>();
            customerBehaviour = customerGirlBehaviour;
        }
        else if (customerType == 1) // Man
        {
            CustomerManBehaviour customerManBehaviour = spawnedCustomer.AddComponent<CustomerManBehaviour>();
            customerBehaviour = customerManBehaviour;
        }
        else if (customerType == 2) // Thief
        {
            CustomerThiefBehaviour customerThiefBehaviour = spawnedCustomer.AddComponent<CustomerThiefBehaviour>();
            customerBehaviour = customerThiefBehaviour;
        }

        AssignTMPTextComponents(spawnedCustomer, customerBehaviour);

        spawnedCustomer.transform.SetParent(transform);
        SFXPlayScene.Instance.PlayWrongSound();

        customerCount++;
        Debug.Log("customerCount: " + customerCount);
        notificationButton.UpdateNotification(customerCount);

        spawnedCustomers.Add(spawnedCustomer);
    }
}

private void AssignTMPTextComponents(GameObject customer, MonoBehaviour customerBehaviour)
{
    TMP_Text[] tmpTexts = customer.GetComponentsInChildren<TMP_Text>();
    foreach (TMP_Text tmpText in tmpTexts)
    {
        if (tmpText.gameObject.name == "Accept")
        {
            if (customerBehaviour is CustomerGirlBehaviour girlBehaviour)
                girlBehaviour.AcceptText = tmpText;
            else if (customerBehaviour is CustomerManBehaviour manBehaviour)
                manBehaviour.AcceptText = tmpText;
            else if (customerBehaviour is CustomerThiefBehaviour thiefBehaviour)
                thiefBehaviour.AcceptText = tmpText;
        }
        else if (tmpText.gameObject.name == "Decline")
        {
            if (customerBehaviour is CustomerGirlBehaviour girlBehaviour)
                girlBehaviour.DeclineText = tmpText;
            else if (customerBehaviour is CustomerManBehaviour manBehaviour)
                manBehaviour.DeclineText = tmpText;
            else if (customerBehaviour is CustomerThiefBehaviour thiefBehaviour)
                thiefBehaviour.DeclineText = tmpText;
        }
    }

    // Assign other required references
    if (customerBehaviour is ICustomerBehaviour customerInterface)
        customerInterface.notificationButton = GameObject.FindWithTag("Notification").GetComponentInChildren<NotificationButton>();
}



    // Reparent spawned customers when the scene changes
    void OnLevelWasLoaded(int level)
    {
        foreach (var customer in spawnedCustomers)
        {
            customer.transform.SetParent(transform);
        }
    }
}
