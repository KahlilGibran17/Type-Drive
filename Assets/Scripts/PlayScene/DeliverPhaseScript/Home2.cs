using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home2 : MonoBehaviour
{
    public static Home2 Instance;
    public GameObject home2;
    public GameObject truckSparePart;
    public GameObject customerHouse2;
    public Transform moneyHouse2;
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
