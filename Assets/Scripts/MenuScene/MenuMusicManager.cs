using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    public AudioClip menuMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
            return;
        }

        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
