using UnityEngine;

public class SFXPlayScene : MonoBehaviour
{
    public static SFXPlayScene Instance;  // Singleton instance

    public AudioClip notificationSound;
    public AudioClip playsceneBackground;
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
         audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
            return;
        }

        audioSource.clip = playsceneBackground;
        audioSource.loop = true;
        audioSource.Play();
    
    }

    
       

    public void PlayWrongSound()
    {
        audioSource.PlayOneShot(notificationSound);
    }


}
