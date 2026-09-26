using UnityEngine;

public class PlayerMusic : MonoBehaviour
{
    [Header("Round Music")]
    public AudioClip round1Music;
    public AudioClip round2Music;

    [Header("Player Hit Sounds")]
    public AudioClip normalHitSound;
    public AudioClip specialHitSound;

    [Header("Music Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    public bool playMusicAtStart = true;

    [Header("Hit Sound Settings")]
    [Range(0f, 1f)]
    public float hitSoundVolume = 1f;

    private AudioSource musicAudioSource;
    private AudioSource hitAudioSource;

    private bool isRound2Music = false;
    private bool musicInitialized = false;

    private void Awake()
    {
        SetupAudioSources();
    }

    private void Start()
    {
        if (playMusicAtStart)
        {
            PlayRound1Music();
        }
    }

    private void SetupAudioSources()
    {
        if (musicInitialized)
            return;

        musicInitialized = true;

        GameObject musicObject =
            new GameObject("Theme Music Audio");

        musicObject.transform.SetParent(transform);
        musicObject.transform.localPosition = Vector3.zero;

        musicAudioSource =
            musicObject.AddComponent<AudioSource>();

        musicAudioSource.playOnAwake = false;
        musicAudioSource.loop = true;
        musicAudioSource.volume = musicVolume;
        musicAudioSource.spatialBlend = 0f;

        GameObject hitObject =
            new GameObject("Hit Sound Audio");

        hitObject.transform.SetParent(transform);
        hitObject.transform.localPosition = Vector3.zero;

        hitAudioSource =
            hitObject.AddComponent<AudioSource>();

        hitAudioSource.playOnAwake = false;
        hitAudioSource.loop = false;
        hitAudioSource.volume = hitSoundVolume;
        hitAudioSource.spatialBlend = 0f;
    }

    public void PlayRound1Music()
    {
        if (isRound2Music)
        {
            return;
        }

        if (musicAudioSource == null)
        {
            return;
        }

        if (round1Music == null)
        {
            return;
        }

        if (
            musicAudioSource.isPlaying &&
            musicAudioSource.clip == round1Music
        )
        {
            return;
        }

        musicAudioSource.Stop();

        musicAudioSource.clip = round1Music;
        musicAudioSource.loop = true;
        musicAudioSource.volume = musicVolume;

        musicAudioSource.Play();
    }

    public void ChangeToRound2Music()
    {
        if (round2Music == null)
        {
            return;
        }

        isRound2Music = true;

        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = null;
        }

        StopAllRound1Music();

        if (musicAudioSource == null)
        {
            return;
        }

        musicAudioSource.clip = round2Music;
        musicAudioSource.loop = true;
        musicAudioSource.volume = musicVolume;

        musicAudioSource.Play();
    }

    private void StopAllRound1Music()
    {
        if (round1Music == null)
            return;

        AudioSource[] sources =
            GetComponentsInChildren<AudioSource>(true);

        foreach (AudioSource source in sources)
        {
            if (source == null)
                continue;

            if (
                source.isPlaying &&
                source.clip == round1Music
            )
            {
                source.Stop();
                source.clip = null;
            }
        }
    }

    public void PlayNormalHitSound()
    {
        if (hitAudioSource == null)
        {
            return;
        }

        if (normalHitSound == null)
        {
            return;
        }

        hitAudioSource.volume = hitSoundVolume;

        hitAudioSource.PlayOneShot(
            normalHitSound,
            hitSoundVolume
        );
    }

    public void PlaySpecialHitSound()
    {
        if (hitAudioSource == null)
        {
            return;
        }

        if (specialHitSound == null)
        {
            return;
        }

        hitAudioSource.volume = hitSoundVolume;

        hitAudioSource.PlayOneShot(
            specialHitSound,
            hitSoundVolume
        );
    }

    public void PlayHitSound(bool specialAttack)
    {
        if (specialAttack)
        {
            PlaySpecialHitSound();
        }
        else
        {
            PlayNormalHitSound();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }
    }

    public void SetHitSoundVolume(float volume)
    {
        hitSoundVolume = Mathf.Clamp01(volume);

        if (hitAudioSource != null)
        {
            hitAudioSource.volume = hitSoundVolume;
        }
    }

    public void StopMusic()
    {
        if (musicAudioSource == null)
            return;

        musicAudioSource.Stop();
        musicAudioSource.clip = null;
    }

    public void ResetToRound1()
    {
        isRound2Music = false;

        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = null;
        }

        PlayRound1Music();
    }

    public bool IsRound2MusicPlaying()
    {
        return isRound2Music;
    }

    public bool IsMusicPlaying()
    {
        if (musicAudioSource == null)
            return false;

        return musicAudioSource.isPlaying;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetHitSoundVolume()
    {
        return hitSoundVolume;
    }
}