using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of camera movement
    public float zoomSpeed = 2f; // Speed of camera zoom

    void Update()
    {
        // Move camera based on arrow key inputs
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Zoom camera based on mouse scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            float newSize = Camera.main.orthographicSize - scrollInput * zoomSpeed;
            // Clamp the zoom to prevent it from going too small or too large
            newSize = Mathf.Clamp(newSize, 1f, 10f); // Adjust the min and max zoom values as needed
            Camera.main.orthographicSize = newSize;
        }
    }
}

