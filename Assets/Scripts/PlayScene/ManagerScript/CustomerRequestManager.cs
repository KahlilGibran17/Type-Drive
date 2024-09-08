using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerRequestManager : MonoBehaviour
{
    public GameObject[] sparePartOilPrefabs;
    public GameObject[] sparePartShockBreakerPrefabs;
    public GameObject[] sparePartBatteryPrefabs;
    public GameObject[] customerGirlPrefabs;
    public GameObject[] customerManPrefabs;
    public GameObject[] customerThiefPrefabs;
    public Transform[] spawnPoints;
    public DeliverSparePart deliverSparePart; // Reference to DeliverSparePart script

    private List<Transform> occupiedSpawnPoints = new List<Transform>();

    void Start()
    {
        int instanceCountMan = PlayerPrefs.GetInt("CurrentInstanceCountMan", 0);
        int instanceCountGirl = PlayerPrefs.GetInt("CurrentInstanceCountGirl", 0);
        int instanceCountThief = PlayerPrefs.GetInt("CurrentInstanceCountThief", 0);

        Debug.Log($"Man instances: {instanceCountMan}, Girl instances: {instanceCountGirl}, Thief instances: {instanceCountThief}");

        SpawnAllSavedRequests();
    }

    private void SpawnAllSavedRequests()
    {
        int instanceCountMan = PlayerPrefs.GetInt("CurrentInstanceCountMan", 0);
        int instanceCountGirl = PlayerPrefs.GetInt("CurrentInstanceCountGirl", 0);
        int instanceCountThief = PlayerPrefs.GetInt("CurrentInstanceCountThief", 0);

        for (int i = 1; i <= instanceCountMan; i++)
        {
            SpawnSavedRequest("Man", i);
        }
        for (int i = 1; i <= instanceCountGirl; i++)
        {
            SpawnSavedRequest("Girl", i);
        }
        for (int i = 1; i <= instanceCountThief; i++)
        {
            SpawnSavedRequest("Thief", i);
        }
    }

    private void SpawnSavedRequest(string customerType, int instanceIndex)
    {
        string customerKey = $"Customer{customerType}{instanceIndex}";
        string sparePartTypeKey = $"SparePartType{customerType}{instanceIndex}";
        string scoreKey = $"Score{customerType}{instanceIndex}";

        Debug.Log($"Attempting to retrieve data for {customerType} instance {instanceIndex}");
        int customer = PlayerPrefs.GetInt(customerKey, -1);
        int sparePartType = PlayerPrefs.GetInt(sparePartTypeKey, -1);
        int score = PlayerPrefs.GetInt(scoreKey, 0);

        if (customer == -1 || sparePartType == -1)
        {
            Debug.LogError($"PlayerPrefs data not found for {customerType} instance {instanceIndex}");
            return;
        }

        GameObject[] chosenSpareParts = GetSparePartArray(sparePartType);
        GameObject[] chosenCustomers = GetCustomerArray(customerType);

        if (chosenSpareParts != null && chosenCustomers != null && spawnPoints.Length > 0)
        {
            List<Transform> availableSpawnPoints = new List<Transform>();
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (!occupiedSpawnPoints.Contains(spawnPoint))
                {
                    availableSpawnPoints.Add(spawnPoint);
                }
            }

            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogError("No available spawn points.");
                return;
            }

            Transform chosenSpawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];

            GameObject sparePart = Instantiate(chosenSpareParts[Random.Range(0, chosenSpareParts.Length)], chosenSpawnPoint.position, Quaternion.identity);
            GameObject customerObject = Instantiate(chosenCustomers[Random.Range(0, chosenCustomers.Length)], chosenSpawnPoint.position, Quaternion.identity);

            sparePart.transform.parent = chosenSpawnPoint;
            customerObject.transform.parent = chosenSpawnPoint;

            occupiedSpawnPoints.Add(chosenSpawnPoint);

            Debug.Log($"Spawned {customerType} with SparePartType {sparePartType} with score {score} at position {chosenSpawnPoint.position}");

            deliverSparePart.SetCustomerScore(customerObject, score);
            // Pass score information to DeliverSparePart
            
        }
        else
        {
            Debug.LogError("No prefabs found for the given types or no spawn points available.");
        }
    }

    private GameObject[] GetSparePartArray(int sparePartType)
    {
        switch (sparePartType)
        {
            case 0:
                return sparePartOilPrefabs;
            case 1:
                return sparePartShockBreakerPrefabs;
            case 2:
                return sparePartBatteryPrefabs;
            default:
                return null;
        }
    }

    private GameObject[] GetCustomerArray(string customerType)
    {
        switch (customerType)
        {
            case "Girl":
                return customerGirlPrefabs;
            case "Man":
                return customerManPrefabs;
            case "Thief":
                return customerThiefPrefabs;
            default:
                return null;
        }
    }
}
