using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home3 : MonoBehaviour
{
    public static Home3 Instance;
    public GameObject home3;
    public GameObject truckSparePart;
    public GameObject customerHouse3;
    public Transform moneyHouse3;
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
