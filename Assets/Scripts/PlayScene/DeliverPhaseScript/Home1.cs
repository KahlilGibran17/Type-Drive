using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home1 : MonoBehaviour
{
    public static Home1 Instance;
    public GameObject home1;
    public GameObject truckSparePart;
    public GameObject customerHouse1;
    public Transform moneyHouse1;
    // Start is called before the first frame update
        private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }
}
    
