using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TruckController : MonoBehaviour
{
    public static TruckController Instance { get; private set; }
    public static GameObject Truck;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    [SerializeField] private float upDownThreshold = 0.1f; // Threshold for triggering up and down animations
    [SerializeField] private float arrivalThreshold = 1.0f; // Threshold distance for considering the truck as arrived

    // Predefined destinations
    [SerializeField] private Transform[] destinations = new Transform[8];
    [SerializeField] private GameObject[] spareparts = new GameObject[3];

    private GameObject currentSparePart;
    private Transform currentDestination;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    void Update()
    {
        UpdateAnimation();
    }

    public void SetDestination(int destinationIndex)
    {
        if (destinationIndex >= 0 && destinationIndex < destinations.Length)
        {
            currentDestination = destinations[destinationIndex];
            navMeshAgent.SetDestination(currentDestination.position);
            Debug.Log("Setting destination to: " + currentDestination.name);
        }
        else
        {
            Debug.LogWarning("Invalid destination index: " + destinationIndex);
        }
    }

    public bool HasArrived()
    {
        if (currentDestination == null) return false;
        float distance = Vector3.Distance(transform.position, currentDestination.position);
        return distance <= arrivalThreshold;
    }

    public void SetSparePart(int sparepartIndex)
    {
        if (sparepartIndex >= 0 && sparepartIndex < spareparts.Length)
        {
            // Destroy the current spare part if it exists
            if (currentSparePart != null)
            {
                Destroy(currentSparePart);
            }

            // Instantiate the new spare part at the truck's position
            currentSparePart = Instantiate(spareparts[sparepartIndex], transform.position, transform.rotation);

            // Make the spare part a child of the truck to follow it
            currentSparePart.transform.SetParent(transform);

            Debug.Log("Spare part spawned: " + spareparts[sparepartIndex].name);
        }
        else
        {
            Debug.LogWarning("Invalid spare part index: " + sparepartIndex);
        }
    }

    void UpdateAnimation()
    {
        // Get the velocity of the truck
        Vector3 movement = navMeshAgent.velocity.normalized;

        // Calculate the angle between the truck's forward direction and its movement direction
        float angle = Vector3.SignedAngle(transform.forward, movement, Vector3.up);

        // Determine if the truck is moving horizontally
        bool isMovingHorizontally = Mathf.Abs(angle) > 45f;

        // Trigger "GoRight" animation if the angle is positive and the truck is not moving vertically
        if (angle > 45f && !IsMovingVertically())
        {
            animator.Play("GoRight");
        }
        // Trigger "GoLeft" animation if the angle is negative and the truck is not moving vertically
        else if (angle < -45f && !IsMovingVertically())
        {
            animator.Play("GoLeft");
        }
        // Trigger "GoUp" animation if the truck is moving vertically upwards
        else if (IsMovingUp())
        {
            animator.Play("GoUp");
        }
        // Trigger "GoDown" animation if the truck is moving vertically downwards
        else if (IsMovingDown())
        {
            animator.Play("GoDown");
        }
    }

    // Helper method to check if the truck is moving vertically upwards
    private bool IsMovingUp()
    {
        return navMeshAgent.velocity.y > upDownThreshold;
    }

    // Helper method to check if the truck is moving vertically downwards
    private bool IsMovingDown()
    {
        return navMeshAgent.velocity.y < -upDownThreshold;
    }

    // Helper method to check if the truck is moving vertically
    private bool IsMovingVertically()
    {
        return IsMovingUp() || IsMovingDown();
    }
}
