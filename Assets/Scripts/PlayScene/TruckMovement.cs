using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
    public Transform destination; // The destination where the truck should move
    public float speed = 5f; // Speed of the truck movement

    private bool isMoving = false; // Flag to track if the truck is currently moving

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving) // Check for input to start movement
        {
            MoveToDestination();
        }
    }

    void MoveToDestination()
    {
        isMoving = true; // Set flag to indicate the truck is moving
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            // Calculate the direction towards the destination
            Vector3 direction = (destination.position - transform.position).normalized;

            // Move the truck towards the destination
            transform.position = Vector3.MoveTowards(transform.position, destination.position, speed * Time.fixedDeltaTime);

            // Check if the truck has reached the destination
            if (transform.position == destination.position)
            {
                isMoving = false; // Reset flag to indicate the truck has stopped moving
            }
        }
    }
}
