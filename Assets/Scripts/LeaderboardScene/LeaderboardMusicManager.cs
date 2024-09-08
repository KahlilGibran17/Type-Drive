using UnityEngine;

public class LeaderboardMusicManager : MonoBehaviour
{
    public AudioClip leaderboardMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
            return;
        }

        audioSource.clip = leaderboardMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
