using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Camera State References")]
    public CameraInitial cameraInitial;
    public CameraZoom cameraZoom;

    [Header("Player Camera")]
    public Camera playerCamera;

    [Header("Bullet Spawn")]
    public Transform bulletSpawnSpot;

    [Header("Zoom Bullet Spawn")]
    public Transform zoomBulletSpawnSpot;

    [Header("Bullet Models")]
    public GameObject redBullet;
    public GameObject blueBullet;
    public GameObject greenBullet;
    public GameObject yellowBullet;

    [Header("Aim")]
    public float aimDistance = 1000f;

    private int currentBulletColor = 1;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (playerCamera == null)
        {
            Debug.LogError(
                "PlayerShoot: Player Camera is NOT assigned and Main Camera was not found!"
            );
        }

        if (cameraInitial == null)
        {
            Debug.LogError(
                "PlayerShoot: CameraInitial is NOT assigned!"
            );
        }

        if (cameraZoom == null)
        {
            Debug.LogError(
                "PlayerShoot: CameraZoom is NOT assigned!"
            );
        }

        if (bulletSpawnSpot == null)
        {
            Debug.LogError(
                "PlayerShoot: Bullet Spawn Spot is NOT assigned!"
            );
        }

        if (zoomBulletSpawnSpot == null)
        {
            Debug.LogError(
                "PlayerShoot: Zoom Bullet Spawn Spot is NOT assigned!"
            );
        }

        DisableAllBullets();
    }

    void Update()
    {
        if (Keyboard.current == null ||
            Mouse.current == null)
            return;

        // ==========================================
        // BULLET COLOR
        // ==========================================

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentBulletColor = 1;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentBulletColor = 2;
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentBulletColor = 3;
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentBulletColor = 4;
        }

        // ==========================================
        // RIGHT MOUSE BUTTON = SHOOT
        // ==========================================

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // ==========================================
        // CAMERA CHECK
        // ==========================================

        if (playerCamera == null)
        {
            Debug.LogError(
                "PlayerShoot: Player Camera is NOT assigned!"
            );

            return;
        }

        // ==========================================
        // DETERMINE CAMERA STATE
        // ==========================================

        bool isZoomed =
            cameraZoom != null &&
            cameraInitial != null &&
            !cameraInitial.IsActive;

        // ==========================================
        // DETERMINE SPAWN POINT
        // ==========================================

        Transform currentSpawnPoint;

        if (isZoomed)
        {
            currentSpawnPoint =
                zoomBulletSpawnSpot;

            if (currentSpawnPoint == null)
            {
                Debug.LogError(
                    "PlayerShoot: Zoom Bullet Spawn Spot is NOT assigned!"
                );

                return;
            }
        }
        else
        {
            currentSpawnPoint =
                bulletSpawnSpot;

            if (currentSpawnPoint == null)
            {
                Debug.LogError(
                    "PlayerShoot: Bullet Spawn Spot is NOT assigned!"
                );

                return;
            }
        }

        // ==========================================
        // GET SELECTED BULLET
        // ==========================================

        GameObject selectedBullet =
            GetSelectedBullet();

        if (selectedBullet == null)
        {
            Debug.LogError(
                "PlayerShoot: Selected Bullet is NOT assigned!"
            );

            return;
        }

        Vector3 shootDirection;

        // ==========================================
        // INITIAL CAMERA
        // ==========================================

        if (!isZoomed)
        {
            Vector3 targetPoint;

            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            Ray ray =
                playerCamera.ScreenPointToRay(
                    mousePosition
                );

            RaycastHit hit;

            if (Physics.Raycast(
                ray,
                out hit,
                aimDistance))
            {
                targetPoint = hit.point;

                Debug.Log(
                    "AIM HIT OBJECT: " +
                    hit.collider.gameObject.name
                );
            }
            else
            {
                targetPoint =
                    ray.origin +
                    ray.direction *
                    aimDistance;

                Debug.Log(
                    "AIM DID NOT HIT AN OBJECT"
                );
            }

            shootDirection =
                targetPoint -
                currentSpawnPoint.position;

            if (shootDirection.sqrMagnitude <= 0.001f)
                return;

            shootDirection.Normalize();

            // ==========================================
            // INITIAL CAMERA FRONT CHECK
            // ==========================================

            float frontCheck =
                Vector3.Dot(
                    currentSpawnPoint.forward,
                    shootDirection
                );

            if (frontCheck <= 0f)
            {
                return;
            }
        }

        // ==========================================
        // ZOOM CAMERA
        // ==========================================

        else
        {
            shootDirection =
                playerCamera.transform.forward;

            shootDirection.Normalize();

            Debug.Log(
                "ZOOM AIM ACTIVE - CAMERA FORWARD USED"
            );
        }

        // ==========================================
        // CREATE NEW BULLET
        // ==========================================

        GameObject newBullet =
            Instantiate(
                selectedBullet,
                currentSpawnPoint.position,
                Quaternion.LookRotation(
                    shootDirection
                )
            );

        // ==========================================
        // PRESERVE ORIGINAL BULLET SCALE
        // ==========================================

        newBullet.transform.localScale =
            selectedBullet.transform.localScale;

        // ==========================================
        // MAKE BULLET VISIBLE
        // ==========================================

        newBullet.SetActive(true);

        // ==========================================
        // SET BULLET COLOR
        // ==========================================

        PlayerBullet playerBullet =
            newBullet.GetComponent<PlayerBullet>();

        if (playerBullet != null)
        {
            playerBullet.bulletColor =
                GetCurrentBulletColor();
        }
        else
        {
            Debug.LogError(
                "PlayerShoot: Selected bullet has NO PlayerBullet component!"
            );
        }

        Debug.Log(
            "PLAYER BULLET FIRED: " +
            newBullet.name +
            " | SCALE: " +
            newBullet.transform.localScale
        );
    }

    GameObject GetSelectedBullet()
    {
        switch (currentBulletColor)
        {
            case 1:
                return redBullet;

            case 2:
                return blueBullet;

            case 3:
                return greenBullet;

            case 4:
                return yellowBullet;

            default:
                return null;
        }
    }

    // ==========================================
    // GET CURRENT BULLET COLOR
    // ==========================================

    public Color GetCurrentBulletColor()
    {
        switch (currentBulletColor)
        {
            case 1:
                return Color.red;

            case 2:
                return Color.blue;

            case 3:
                return Color.green;

            case 4:
                return Color.yellow;

            default:
                return Color.red;
        }
    }

    void DisableAllBullets()
    {
        if (redBullet != null)
            redBullet.SetActive(false);

        if (blueBullet != null)
            blueBullet.SetActive(false);

        if (greenBullet != null)
            greenBullet.SetActive(false);

        if (yellowBullet != null)
            yellowBullet.SetActive(false);
    }
}