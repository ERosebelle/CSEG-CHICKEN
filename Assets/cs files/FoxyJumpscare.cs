using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FoxyJumpscare : MonoBehaviour
{
    [Header("Player Health")]
    [Tooltip("Drag PlayerHealth here. If empty, it will be found automatically.")]
    public PlayerHealth playerHealth;

    [Header("Foxy")]
    [Tooltip("Drag the Foxy GameObject here.")]
    public GameObject foxy;

    [Header("Foxy Camera")]
    [Tooltip("Drag the separate Foxy Camera here.")]
    public Camera foxyCamera;

    [Header("Player Camera")]
    [Tooltip("Drag the normal Player Camera here.")]
    public Camera playerCamera;

    [Header("Jumpscare Sound")]
    public bool enableJumpscareSound = true;

    public AudioClip jumpscareSound;

    [Range(0f, 1f)]
    public float jumpscareVolume = 1f;

    [Header("Jumpscare Settings")]
    public bool enableJumpscare = true;

    public bool hideFoxyAtStart = true;

    [Header("Restart")]
    public bool enableRestart = true;

    public bool restartOnlyAfterDeath = true;

    private bool jumpscareActive = false;

    private AudioSource audioSource;

    private AudioListener playerListener;
    private AudioListener foxyListener;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        jumpscareActive = false;

        SetupAudioSource();

        FindReferences();

        SetupFoxy();

        SetupCameras();

        Debug.Log("================================");
        Debug.Log("FOXY JUMPSCARE INITIALIZED");
        Debug.Log("================================");
    }


    // =========================================================
    // FIND REFERENCES
    // =========================================================

    private void FindReferences()
    {
        if (playerHealth == null)
        {
            playerHealth =
                FindFirstObjectByType<PlayerHealth>(
                    FindObjectsInactive.Include
                );
        }

        if (playerHealth != null)
        {
            Debug.Log(
                "FoxyJumpscare: PlayerHealth connected: " +
                playerHealth.name
            );
        }
        else
        {
            Debug.LogError(
                "FoxyJumpscare: PlayerHealth NOT FOUND!"
            );
        }
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (!enableRestart)
            return;

        if (restartOnlyAfterDeath &&
            !jumpscareActive)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            RestartGame();
        }
    }


    // =========================================================
    // SETUP FOXY
    // =========================================================

    private void SetupFoxy()
    {
        if (foxy == null)
        {
            Debug.LogError(
                "FoxyJumpscare: Foxy is NOT assigned!"
            );

            return;
        }

        if (hideFoxyAtStart)
        {
            foxy.SetActive(false);
        }

        Debug.Log(
            "Foxy hidden at start."
        );
    }


    // =========================================================
    // SETUP CAMERAS
    // =========================================================

    private void SetupCameras()
    {
        // -----------------------------------------------------
        // PLAYER CAMERA
        // -----------------------------------------------------

        if (playerCamera != null)
        {
            playerCamera.enabled = true;

            playerListener =
                playerCamera.GetComponent<AudioListener>();

            if (playerListener == null)
            {
                playerListener =
                    playerCamera.gameObject.AddComponent<AudioListener>();
            }

            playerListener.enabled = true;

            Debug.Log(
                "Player Camera ready: " +
                playerCamera.name
            );
        }
        else
        {
            Debug.LogWarning(
                "FoxyJumpscare: Player Camera is not assigned."
            );
        }


        // -----------------------------------------------------
        // FOXY CAMERA
        // -----------------------------------------------------

        if (foxyCamera == null)
        {
            Debug.LogError(
                "FoxyJumpscare: Foxy Camera is NOT assigned!"
            );

            return;
        }

        foxyCamera.targetDisplay = 0;

        foxyCamera.targetTexture = null;

        // IMPORTANT:
        // Only disable the CAMERA component.
        // Do NOT disable the GameObject.
        foxyCamera.enabled = false;

        foxyListener =
            foxyCamera.GetComponent<AudioListener>();

        if (foxyListener == null)
        {
            foxyListener =
                foxyCamera.gameObject.AddComponent<AudioListener>();
        }

        foxyListener.enabled = false;

        Debug.Log(
            "Foxy Camera ready and OFF: " +
            foxyCamera.name
        );
    }


    // =========================================================
    // SHOW FOXY
    // =========================================================

    public void ShowFoxy()
    {
        if (!enableJumpscare)
        {
            Debug.Log(
                "Foxy jumpscare is disabled."
            );

            return;
        }

        if (jumpscareActive)
            return;

        jumpscareActive = true;

        Debug.Log("================================");
        Debug.Log("FOXY JUMPSCARE STARTING");
        Debug.Log("================================");


        // -----------------------------------------------------
        // SHOW FOXY
        // -----------------------------------------------------

        if (foxy != null)
        {
            foxy.SetActive(true);

            Debug.Log(
                "FOXY GAMEOBJECT ACTIVATED"
            );
        }
        else
        {
            Debug.LogError(
                "FOXY ERROR: Foxy reference is NULL!"
            );
        }


        // -----------------------------------------------------
        // SWITCH CAMERA
        // -----------------------------------------------------

        SwitchToFoxyCamera();


        // -----------------------------------------------------
        // SOUND
        // -----------------------------------------------------

        PlayJumpscareSound();


        Debug.Log("================================");
        Debug.Log("FOXY JUMPSCARE ACTIVE");
        Debug.Log("PRESS ENTER TO RESTART");
        Debug.Log("================================");
    }


    // =========================================================
    // SWITCH TO FOXY CAMERA
    // =========================================================

    private void SwitchToFoxyCamera()
    {
        if (foxyCamera == null)
        {
            Debug.LogError(
                "Foxy Camera reference is NULL!"
            );

            return;
        }

        // -----------------------------------------------------
        // DISABLE PLAYER CAMERA
        // -----------------------------------------------------

        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }

        if (playerListener != null)
        {
            playerListener.enabled = false;
        }


        // -----------------------------------------------------
        // DISABLE ALL OTHER CAMERAS
        // -----------------------------------------------------

        Camera[] cameras =
            FindObjectsByType<Camera>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (Camera cam in cameras)
        {
            if (cam == null)
                continue;

            if (cam == foxyCamera)
                continue;

            cam.enabled = false;
        }


        // -----------------------------------------------------
        // FOXY CAMERA
        // -----------------------------------------------------

        foxyCamera.gameObject.SetActive(true);

        foxyCamera.targetDisplay = 0;

        foxyCamera.targetTexture = null;

        foxyCamera.enabled = true;


        // -----------------------------------------------------
        // AUDIO LISTENER
        // -----------------------------------------------------

        DisableAllAudioListeners();

        if (foxyListener != null)
        {
            foxyListener.enabled = true;
        }


        // -----------------------------------------------------
        // DEBUG
        // -----------------------------------------------------

        Debug.Log("================================");
        Debug.Log("FOXY CAMERA ACTIVATED");
        Debug.Log("================================");

        Debug.Log(
            "Camera Name: " +
            foxyCamera.name
        );

        Debug.Log(
            "GameObject Active: " +
            foxyCamera.gameObject.activeInHierarchy
        );

        Debug.Log(
            "Camera Enabled: " +
            foxyCamera.enabled
        );

        Debug.Log(
            "Camera Display: " +
            (foxyCamera.targetDisplay + 1)
        );

        Debug.Log(
            "Camera Position: " +
            foxyCamera.transform.position
        );

        Debug.Log(
            "Camera Rotation: " +
            foxyCamera.transform.rotation
        );
    }


    // =========================================================
    // DISABLE ALL AUDIO LISTENERS
    // =========================================================

    private void DisableAllAudioListeners()
    {
        AudioListener[] listeners =
            FindObjectsByType<AudioListener>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (AudioListener listener in listeners)
        {
            if (listener == null)
                continue;

            listener.enabled = false;
        }
    }


    // =========================================================
    // SETUP AUDIO
    // =========================================================

    private void SetupAudioSource()
    {
        audioSource =
            GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource =
                gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = jumpscareVolume;
    }


    // =========================================================
    // PLAY SOUND
    // =========================================================

    private void PlayJumpscareSound()
    {
        if (!enableJumpscareSound)
            return;

        if (jumpscareSound == null)
        {
            Debug.LogWarning(
                "Foxy sound enabled but no AudioClip assigned."
            );

            return;
        }

        if (audioSource == null)
        {
            SetupAudioSource();
        }

        audioSource.volume = jumpscareVolume;

        audioSource.PlayOneShot(
            jumpscareSound
        );

        Debug.Log(
            "FOXY SOUND PLAYING"
        );
    }


    // =========================================================
    // HIDE FOXY
    // =========================================================

    public void HideFoxy()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (foxy != null)
        {
            foxy.SetActive(false);
        }

        if (foxyCamera != null)
        {
            foxyCamera.enabled = false;
        }

        if (foxyListener != null)
        {
            foxyListener.enabled = false;
        }

        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            playerCamera.enabled = true;
        }

        if (playerListener != null)
        {
            playerListener.enabled = true;
        }

        jumpscareActive = false;

        Debug.Log(
            "Foxy hidden. Player camera restored."
        );
    }


    // =========================================================
    // RESTART
    // =========================================================

    public void RestartGame()
    {
        Debug.Log(
            "RESTARTING GAME..."
        );

        Time.timeScale = 1f;

        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }


    // =========================================================
    // CHECK STATE
    // =========================================================

    public bool IsJumpscareActive()
    {
        return jumpscareActive;
    }
}