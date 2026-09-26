using UnityEngine;

public class FarmMusicManager : MonoBehaviour
{
    [Header("Music")]
    public AudioClip round1Music;
    public AudioClip round2Music;

    [Header("Audio Source")]
    public AudioSource audioSource;

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError(
                "FarmMusicManager: Audio Source is NOT assigned!"
            );

            return;
        }

        if (round1Music != null)
        {
            audioSource.clip = round1Music;
            audioSource.Play();
        }
        else
        {
          
        }
    }

    public void PlayRound2Music()
    {
        if (audioSource == null)
        {
           

            return;
        }

        if (round2Music == null)
        {
          

            return;
        }

        audioSource.clip = round2Music;
        audioSource.Play();

        
    }
}