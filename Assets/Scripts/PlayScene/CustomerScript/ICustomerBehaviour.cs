using UnityEngine;
using TMPro;

public interface ICustomerBehaviour
{
    // Define any common properties or methods here
    TMP_Text AcceptText { get; set; }
    TMP_Text DeclineText { get; set; }
    NotificationButton notificationButton { get; set; }
}
