using UnityEngine;
using UnityEngine.UI;

public class NotificationButton : MonoBehaviour
{
    public Image notificationImage; // Reference to the Image component displaying the notification icon
    public Sprite[] notificationSprites; // Array of sprites representing different notification icons

    // Method to update the notification icon
    public void UpdateNotification(int count)
    {
        if (notificationImage != null && notificationSprites != null && notificationSprites.Length > 0)
        {
            // Ensure the count is within the range of available sprites
            int index = Mathf.Clamp(count, 0, notificationSprites.Length - 1);
            // Set the sprite based on the count
            Debug.Log("Count: "+ index);
            notificationImage.sprite = notificationSprites[index];
            // Ensure the image is visible if there are notifications
            notificationImage.enabled = count > 0;
        }
    }
}
