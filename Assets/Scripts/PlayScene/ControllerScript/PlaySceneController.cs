using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneController : MonoBehaviour
{
    // Singleton instance
    public static PlaySceneController Instance;

    public GameObject oilPrefab;
    public GameObject shockbreakerPrefab;
    public GameObject batteryPrefab;

    public int maxSpawns = 1; // Maximum spawns for each spare part
    public Transform[] spawnParents; // Array of parent transforms

    private int[] spawnCounts; // Array to keep track of spawn counts for each parent

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            InitializeSpawnCounts();
        }
        else
        {
            Debug.LogWarning("Multiple instances of PlaySceneController found!");
            Destroy(gameObject);
        }
    }

    private void InitializeSpawnCounts()
    {
        spawnCounts = new int[spawnParents.Length];
        for (int i = 0; i < spawnParents.Length; i++)
        {
            // Count existing children under each spawn parent
            spawnCounts[i] = spawnParents[i].childCount;
        }
    }

    public void SpawnSpareParts(int sparePartIdentifier)
    {
        GameObject prefabToSpawn = null;

        switch (sparePartIdentifier)
        {
            case 0: // Oil
                prefabToSpawn = oilPrefab;
                break;
            case 1: // Shockbreaker
                prefabToSpawn = shockbreakerPrefab;
                break;
            case 2: // Battery
                prefabToSpawn = batteryPrefab;
                break;
            default:
                Debug.LogError("Invalid spare part identifier: " + sparePartIdentifier);
                return;
        }

        bool spawned = false;
        for (int i = 0; i < spawnParents.Length; i++)
        {
            if (spawnCounts[i] < maxSpawns)
            {
                SpawnSingle(prefabToSpawn, spawnParents[i]);
                spawnCounts[i]++;
                spawned = true;
                break;
            }
        }

        if (!spawned)
        {
            Debug.LogWarning("All spawn slots are full!");
            HandleFullSpawnSlots();
        }
    }

    void SpawnSingle(GameObject prefab, Transform parent)
    {
        // Calculate random position within parent's boundaries
        Vector3 spawnPosition = parent.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-2f, 2f), 0f);

        // Instantiate the prefab as a child of parent
        GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity, parent);

        // Optionally, you can adjust the spawnedObject properties or add scripts/components here
    }

    private void HandleFullSpawnSlots()
    {
        // Log warning and notify the leaderboard or handle the full condition
        Debug.LogWarning("All spawn slots are full!");

        // Here you can add code to handle the leaderboard update or other actions.
        // For example:
      
        DeliverSparePart.Instance.Endgame();
    }
}
