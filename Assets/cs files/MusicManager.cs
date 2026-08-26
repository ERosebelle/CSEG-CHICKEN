using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource =
            GetComponent<AudioSource>();

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}