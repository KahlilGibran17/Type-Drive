using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for input or any conditions that determine which animation to play
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput > 0)
        {
            // Play go right animation
            animator.Play("GoRight");
        }
        else if (horizontalInput < 0)
        {
            // Play go left animation
            animator.Play("GoLeft");
        }

        if (verticalInput > 0)
        {
            // Play go up animation
            animator.Play("GoUp");
        }
        else if (verticalInput < 0)
        {
            // Play go down animation
            animator.Play("GoDown");
        }
    }
}

