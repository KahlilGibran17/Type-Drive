using UnityEngine;

public class SFXGirl : MonoBehaviour
{
    public static SFXGirl Instance;  // Singleton instance

    public AudioClip wrongSound;
    public AudioClip congratsSound;
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure that there's only one SoundManager instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: Keep the SoundManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();  // Ensure there is an AudioSource component
        }
    }

    public void PlayWrongSound()
    {
        audioSource.PlayOneShot(wrongSound);
    }

    public void PlayCongratsSound()
    {
        audioSource.PlayOneShot(congratsSound);
    }
}
