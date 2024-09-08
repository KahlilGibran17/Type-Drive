using UnityEngine;

public class TypingManMusicManager : MonoBehaviour
{
    public AudioClip typingManagerMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
            return;
        }

        audioSource.clip = typingManagerMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
