using UnityEngine;

public class TruckManager : MonoBehaviour
{
    public static TruckManager Instance { get; private set; }

    private TruckController truckController;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        truckController = FindObjectOfType<TruckController>();
    }

    public void SetDestination(int destinationIndex)
    {
        if (truckController != null)
        {
            truckController.SetDestination(destinationIndex);
        }
    }
}
