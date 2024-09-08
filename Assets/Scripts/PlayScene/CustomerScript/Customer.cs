using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public RawImage customerImage; // Reference to the RawImage component for customer appearance

void Start()
{
    // Get the RawImage component attached to this GameObject
    customerImage = GetComponent<RawImage>();
    
    // Check if the customer image component is assigned
    if (customerImage == null)
    {
        Debug.LogError("Customer image component is not assigned.");
    }
    else
    {
        Debug.Log("Customer image component is assigned: " + customerImage.name);
    }
}

    // Other properties and methods of the Customer class...
}
