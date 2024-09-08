using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home4 : MonoBehaviour
{
    public static Home4 Instance;
    public GameObject home4;
    public GameObject truckSparePart;
     public GameObject customerHouse4;
    public Transform moneyHouse4;
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

  
    }

