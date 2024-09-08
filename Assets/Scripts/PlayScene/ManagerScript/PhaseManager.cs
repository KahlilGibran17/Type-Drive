using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance;

    public GameObject handphoneUI;
    public GameObject inputStore;
    public GameObject inputHome;
    public GameObject customerRequestSpawn;
    public GameObject sparePartManager;
    public GameObject Layout1;
    public GameObject Layout2;
    public GameObject Layout3;
    public GameObject Layout4;
    public GameObject Layout5;
    public GameObject Layout6;

    private int maxTotalInstances = 4; // Maximum total instances before triggering DeliverPhase
    private int totalInstances = 0; // Total instances of customer requests

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to sceneLoaded event

        // Initialize totalInstances based on saved counts
        int instanceCountMan = PlayerPrefs.GetInt("CurrentInstanceCountMan", 0);
        int instanceCountGirl = PlayerPrefs.GetInt("CurrentInstanceCountGirl", 0);
        int instanceCountThief = PlayerPrefs.GetInt("CurrentInstanceCountThief", 0);

        totalInstances = instanceCountMan + instanceCountGirl + instanceCountThief;

        // Check if DeliverPhase should be triggered initially
        CheckAndDeliverPhase();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from sceneLoaded event
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Recalculate total instances
        int instanceCountMan = PlayerPrefs.GetInt("CurrentInstanceCountMan", 0);
        int instanceCountGirl = PlayerPrefs.GetInt("CurrentInstanceCountGirl", 0);
        int instanceCountThief = PlayerPrefs.GetInt("CurrentInstanceCountThief", 0);

        totalInstances = instanceCountMan + instanceCountGirl + instanceCountThief;

        // Check if DeliverPhase should be triggered
        CheckAndDeliverPhase();
    }

    void CheckAndDeliverPhase()
    {
        if (totalInstances >= maxTotalInstances)
        {
            DeliverPhase();
        }
    }

    public void RequestPhase()
    {
        handphoneUI.SetActive(true);
        inputStore.SetActive(false);
        inputHome.SetActive(false);
        sparePartManager.SetActive(false);
        customerRequestSpawn.SetActive(false);
        Debug.Log("handphone UI active");
    }

    public void DeliverPhase()
    {
        handphoneUI.SetActive(false);
        inputHome.SetActive(true);
        inputStore.SetActive(true);
        sparePartManager.SetActive(true);
        customerRequestSpawn.SetActive(true);
        Layout1.SetActive (false);
        Layout2.SetActive (true);

        Debug.Log("handphone UI inactive");
    }
    public void MoneyLow(){
        Layout2.SetActive (false);
        Layout3.SetActive (true);
    }

    public void MoneyMid(){
        Layout3.SetActive (false);
        Layout4.SetActive (true);
    }
    public void MoneyHigh(){
        Layout4.SetActive (false);
        Layout5.SetActive (true);
    }
    public void MoneyFull(){
        Layout5.SetActive (false);
        Layout6.SetActive (true);
    }
}
