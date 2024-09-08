using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HandphoneController : MonoBehaviour
{
    public GameObject handphoneUI;
    public GameObject notificationButton;
    public GameObject customerRequestScreen;
   
    public GameObject customerManager; // Reference to the customer manager object
    private bool isHandphoneActive = false;
    private int pendingRequests = 0;
    private GameObject currentCustomerRequestScreen; // Track the current customer request screen

    void Update()
    {
        // Toggle handphone on and off
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleHandphone();
        }
    }

    void ToggleHandphone()
    {
        isHandphoneActive = !isHandphoneActive;

        // Activate/deactivate handphone UI
        handphoneUI.SetActive(isHandphoneActive);

        // Activate/deactivate notification button
        

        // Activate/deactivate customer manager children based on handphone state
        foreach (Transform child in customerManager.transform)
        {
            if (child.CompareTag("Customer"))
            {
                child.gameObject.SetActive(isHandphoneActive);
            }
        }

        // Switch screens based on handphone state
        if (isHandphoneActive)
        {
            // Check if there are any pending requests
            if (pendingRequests > 0)
            {
                // Check if there are any customer clones spawned
                GameObject[] customerClones = GameObject.FindGameObjectsWithTag("Customer");
                if (customerClones.Length > 0)
                {
                    currentCustomerRequestScreen = customerClones[0];
                    SwitchToCustomerRequestScreen();
                }
                else
                {
                    SwitchToTemplateScreen();
                }
            }
            else
            {
                SwitchToTemplateScreen();
            }
        }
        else
        {
            SwitchToTemplateScreen(); // Always switch to the template screen when turning off the handphone
        }
    }

    void SwitchToTemplateScreen()
    {;
        if (currentCustomerRequestScreen != null)
        {
            currentCustomerRequestScreen.SetActive(false);
        }
        customerRequestScreen.SetActive(false);
    }

    void SwitchToCustomerRequestScreen()
    {
        if (currentCustomerRequestScreen != null)
        {
            currentCustomerRequestScreen.SetActive(true);
        }
        customerRequestScreen.SetActive(true);
    }

    public void SwitchToSparePartScreen()
    {
        customerRequestScreen.SetActive(false);
    }


    public void DeclineRequest()
    {
        customerRequestScreen.SetActive(false);
    }
    



    public void UpdateNotificationButton(int count)
    {
        pendingRequests = count;
        // Update notification button logic here
    }

    // Method to set the current customer request screen
    public void SetCurrentCustomerRequestScreen(GameObject screen)
    {
        currentCustomerRequestScreen = screen;
        DontDestroyOnLoad(this.gameObject);
        

    }
}
