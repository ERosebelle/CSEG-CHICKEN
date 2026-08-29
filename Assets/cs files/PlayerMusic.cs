
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


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        SetupAudioSources();
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        if (playMusicAtStart)
        {
            PlayRound1Music();
        }
    }


    // =========================================================
    // SETUP AUDIO SOURCES
    // =========================================================

    private void SetupAudioSources()
    {
        // Prevent duplicate setup
        if (musicInitialized)
            return;

        musicInitialized = true;


        // =====================================================
        // MUSIC AUDIO SOURCE
        // =====================================================

        GameObject musicObject =
            new GameObject("Theme Music Audio");

        musicObject.transform.SetParent(transform);

        musicObject.transform.localPosition =
            Vector3.zero;

        musicAudioSource =
            musicObject.AddComponent<AudioSource>();

        musicAudioSource.playOnAwake = false;
        musicAudioSource.loop = true;
        musicAudioSource.volume = musicVolume;
        musicAudioSource.spatialBlend = 0f;


        // =====================================================
        // HIT AUDIO SOURCE
        // =====================================================

        GameObject hitObject =
            new GameObject("Hit Sound Audio");

        hitObject.transform.SetParent(transform);

        hitObject.transform.localPosition =
            Vector3.zero;

        hitAudioSource =
            hitObject.AddComponent<AudioSource>();

        hitAudioSource.playOnAwake = false;
        hitAudioSource.loop = false;
        hitAudioSource.volume = hitSoundVolume;
        hitAudioSource.spatialBlend = 0f;


        Debug.Log(
            "PlayerMusic: Audio system initialized."
        );
    }


    // =========================================================
    // ROUND 1 MUSIC
    // =========================================================

    public void PlayRound1Music()
    {
        // =====================================================
        // DO NOT ALLOW ROUND 1 AFTER ROUND 2
        // =====================================================

        if (isRound2Music)
        {
            Debug.Log(
                "PlayerMusic: Round 2 is already active. " +
                "Round 1 will NOT start."
            );

            return;
        }


        // =====================================================
        // CHECK AUDIO SOURCE
        // =====================================================

        if (musicAudioSource == null)
        {
            Debug.LogError(
                "PlayerMusic: Music AudioSource is missing."
            );

            return;
        }


        // =====================================================
        // CHECK ROUND 1 CLIP
        // =====================================================

        if (round1Music == null)
        {
            Debug.LogError(
                "PlayerMusic: Round 1 music is NOT assigned."
            );

            return;
        }


        // =====================================================
        // ALREADY PLAYING ROUND 1
        // =====================================================

        if (
            musicAudioSource.isPlaying &&
            musicAudioSource.clip == round1Music
        )
        {
            return;
        }


        // =====================================================
        // STOP CURRENT MUSIC
        // =====================================================

        musicAudioSource.Stop();


        // =====================================================
        // ASSIGN ROUND 1
        // =====================================================

        musicAudioSource.clip =
            round1Music;

        musicAudioSource.loop =
            true;

        musicAudioSource.volume =
            musicVolume;


        // =====================================================
        // PLAY ROUND 1
        // =====================================================

        musicAudioSource.Play();


        Debug.Log(
            "========================================"
        );

        Debug.Log(
            "ROUND 1 MUSIC STARTED"
        );

        Debug.Log(
            "Music: " +
            round1Music.name
        );

        Debug.Log(
            "Volume: " +
            musicVolume
        );

        Debug.Log(
            "========================================"
        );
    }


    // =========================================================
    // ROUND 2 MUSIC
    // =========================================================

    public void ChangeToRound2Music()
    {
        // =====================================================
        // CHECK ROUND 2 CLIP
        // =====================================================

        if (round2Music == null)
        {
            Debug.LogError(
                "PlayerMusic: Round 2 music is NOT assigned."
            );

            return;
        }


        // =====================================================
        // SET ROUND 2 STATE FIRST
        // =====================================================

        isRound2Music = true;


        Debug.Log(
            "PlayerMusic: Switching to ROUND 2."
        );


        // =====================================================
        // STOP OUR MUSIC SOURCE
        // =====================================================

        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();

            musicAudioSource.clip = null;
        }


        // =====================================================
        // FORCE STOP ROUND 1 AUDIO SOURCES
        // =====================================================

        StopAllRound1Music();


        // =====================================================
        // START ROUND 2
        // =====================================================

        if (musicAudioSource == null)
        {
            Debug.LogError(
                "PlayerMusic: Music AudioSource is missing."
            );

            return;
        }


        musicAudioSource.clip =
            round2Music;

        musicAudioSource.loop =
            true;

        musicAudioSource.volume =
            musicVolume;


        musicAudioSource.Play();


        Debug.Log(
            "========================================"
        );

        Debug.Log(
            "ROUND 2 MUSIC STARTED"
        );

        Debug.Log(
            "ROUND 1 MUSIC FORCED OFF"
        );

        Debug.Log(
            "Music: " +
            round2Music.name
        );

        Debug.Log(
            "Volume: " +
            musicVolume
        );

        Debug.Log(
            "========================================"
        );
    }


    // =========================================================
    // FORCE STOP ALL ROUND 1 MUSIC
    // =========================================================

    private void StopAllRound1Music()
    {
        if (round1Music == null)
            return;


        AudioSource[] sources =
            GetComponentsInChildren<AudioSource>(
                true
            );


        foreach (AudioSource source in sources)
        {
            if (source == null)
                continue;


            // If this source is playing Round 1,
            // force it to stop.

            if (
                source.isPlaying &&
                source.clip == round1Music
            )
            {
                source.Stop();

                source.clip = null;


                Debug.Log(
                    "PlayerMusic: Forced Round 1 AudioSource OFF."
                );
            }
        }
    }


    // =========================================================
    // NORMAL HIT SOUND
    // =========================================================

    public void PlayNormalHitSound()
    {
        if (hitAudioSource == null)
        {
            Debug.LogError(
                "PlayerMusic: Hit Sound AudioSource is missing."
            );

            return;
        }


        if (normalHitSound == null)
        {
            Debug.LogError(
                "PlayerMusic: Normal hit sound is NOT assigned."
            );

            return;
        }


        hitAudioSource.volume =
            hitSoundVolume;


        hitAudioSource.PlayOneShot(
            normalHitSound,
            hitSoundVolume
        );


        Debug.Log(
            "PlayerMusic: NORMAL HIT SOUND PLAYED."
        );
    }


    // =========================================================
    // SPECIAL HIT SOUND
    // =========================================================

    public void PlaySpecialHitSound()
    {
        if (hitAudioSource == null)
        {
            Debug.LogError(
                "PlayerMusic: Hit Sound AudioSource is missing."
            );

            return;
        }


        if (specialHitSound == null)
        {
            Debug.LogError(
                "PlayerMusic: Special hit sound is NOT assigned."
            );

            return;
        }


        hitAudioSource.volume =
            hitSoundVolume;


        hitAudioSource.PlayOneShot(
            specialHitSound,
            hitSoundVolume
        );


        Debug.Log(
            "PlayerMusic: SPECIAL HIT SOUND PLAYED."
        );
    }


    // =========================================================
    // GENERIC HIT SOUND
    // =========================================================

    public void PlayHitSound(
        bool specialAttack
    )
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


    // =========================================================
    // SET MUSIC VOLUME
    // =========================================================

    public void SetMusicVolume(
        float volume
    )
    {
        musicVolume =
            Mathf.Clamp01(volume);


        if (musicAudioSource != null)
        {
            musicAudioSource.volume =
                musicVolume;
        }


        Debug.Log(
            "PlayerMusic: Music Volume = " +
            musicVolume
        );
    }


    // =========================================================
    // SET HIT SOUND VOLUME
    // =========================================================

    public void SetHitSoundVolume(
        float volume
    )
    {
        hitSoundVolume =
            Mathf.Clamp01(volume);


        if (hitAudioSource != null)
        {
            hitAudioSource.volume =
                hitSoundVolume;
        }


        Debug.Log(
            "PlayerMusic: Hit Sound Volume = " +
            hitSoundVolume
        );
    }


    // =========================================================
    // STOP MUSIC
    // =========================================================

    public void StopMusic()
    {
        if (musicAudioSource == null)
            return;


        musicAudioSource.Stop();

        musicAudioSource.clip = null;


        Debug.Log(
            "PlayerMusic: Music stopped."
        );
    }


    // =========================================================
    // RESET TO ROUND 1
    // =========================================================

    public void ResetToRound1()
    {
        // =====================================================
        // RESET STATE
        // =====================================================

        isRound2Music = false;


        // =====================================================
        // STOP EVERYTHING
        // =====================================================

        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();

            musicAudioSource.clip = null;
        }


        // =====================================================
        // START ROUND 1
        // =====================================================

        PlayRound1Music();


        Debug.Log(
            "PlayerMusic: Reset to Round 1."
        );
    }


    // =========================================================
    // CHECK ROUND 2
    // =========================================================

    public bool IsRound2MusicPlaying()
    {
        return isRound2Music;
    }


    // =========================================================
    // CHECK MUSIC
    // =========================================================

    public bool IsMusicPlaying()
    {
        if (musicAudioSource == null)
            return false;

        return musicAudioSource.isPlaying;
    }


    // =========================================================
    // GET MUSIC VOLUME
    // =========================================================

    public float GetMusicVolume()
    {
        return musicVolume;
    }


    // =========================================================
    // GET HIT SOUND VOLUME
    // =========================================================

    public float GetHitSoundVolume()
    {
        return hitSoundVolume;
    }
}
