using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SparePartsManager : MonoBehaviour
{
    public static SparePartsManager Instance;

    public List<GameObject> ExistingSpareParts = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start(){

    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.F1))
        SelectSparePart(0);
    if (Input.GetKeyDown(KeyCode.F2))
        SelectSparePart(1);
    if (Input.GetKeyDown(KeyCode.F3))
        SelectSparePart(2);
    if (Input.GetKeyDown(KeyCode.F4))
        SelectSparePart(3);
}

private void SelectSparePart(int index)
{
    if (index < ExistingSpareParts.Count)
    {
        GameObject selectedPart = ExistingSpareParts[index];
        if (selectedPart != null)
        {
            HighlightSparePart(selectedPart);
            Debug.Log($"Spare part selected: {selectedPart.name}");
        }
    }
    else
    {
        Debug.LogWarning($"No spare part available at slot {index + 1}");
    }
}

private void HighlightSparePart(GameObject part)
{
    // Assuming each part has a Renderer and we change the color to highlight
    Renderer renderer = part.GetComponent<Renderer>();
    if (renderer != null)
    {
        // Reset color of all parts first
        ExistingSpareParts.ForEach(p =>
        {
            Renderer r = p.GetComponent<Renderer>();
            if (r != null) r.material.color = Color.white; // Default color
        });

        // Set the selected part's color to yellow
        renderer.material.color = Color.yellow;
    }
}

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}. Tracking {ExistingSpareParts.Count} spare parts.");

       
        DuplicateSpareParts();
    }

    public void AddSparePart(GameObject part)
    {
        if (!ExistingSpareParts.Contains(part))
        {
            ExistingSpareParts.Add(part);
            DontDestroyOnLoad(part);
            Debug.Log($"Added and preserved: {part.name}");
        }
    }

    public bool CheckPartExists(GameObject prefab)
    {
        return ExistingSpareParts.Exists(part => part.name == prefab.name + "(Clone)");
    }

    private void DuplicateSpareParts()
    {
        GameObject sparePartDelivery = GameObject.Find("SparePartDelivery");
        if (sparePartDelivery != null)
        {
            foreach (GameObject part in ExistingSpareParts)
            {
                if (part != null)
                {
                    GameObject duplicate = Instantiate(part);
                    duplicate.transform.SetParent(sparePartDelivery.transform, false);
                    duplicate.name = part.name; // Ensure the duplicate has the same name
                    duplicate.transform.position = part.transform.position; // Set the correct position
                    Debug.Log($"Duplicated: {duplicate.name} in SparePartDelivery");
                }
            }
        }
        else
        {
            Debug.LogError("SparePartDelivery GameObject not found in the scene.");
        }
    }
}
