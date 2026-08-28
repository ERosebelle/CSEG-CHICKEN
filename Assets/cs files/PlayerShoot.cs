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

    [Header("Ammo")]
    public AmmoCount ammoCount;

    [Header("Potion Counts")]
    public RedCount redCount;
    public BlueCount blueCount;
    public GreenCount greenCount;
    public YellowCount yellowCount;

    [Header("Aim")]
    public float aimDistance = 1000f;

    private int currentBulletColor = 0;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        DisableAllBullets();

        SelectFirstAvailableColor();
    }

    void Update()
    {
        if (Keyboard.current == null ||
            Mouse.current == null)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            TrySelectColor(1);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            TrySelectColor(2);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            TrySelectColor(3);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            TrySelectColor(4);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void TrySelectColor(int color)
    {
        if (color == currentBulletColor)
            return;

        if (!HasPotion(color))
            return;

        RemovePotion(color);

        currentBulletColor = color;
    }

    void SelectFirstAvailableColor()
    {
        if (HasPotion(1))
        {
            currentBulletColor = 1;
            return;
        }

        if (HasPotion(2))
        {
            currentBulletColor = 2;
            return;
        }

        if (HasPotion(3))
        {
            currentBulletColor = 3;
            return;
        }

        if (HasPotion(4))
        {
            currentBulletColor = 4;
            return;
        }

        currentBulletColor = 0;
    }

    void Shoot()
    {
        if (ammoCount == null)
            return;

        if (ammoCount.GetAmmoCount() <= 0)
            return;

        if (currentBulletColor == 0)
            return;

        if (playerCamera == null)
            return;

        bool isZoomed =
            cameraZoom != null &&
            cameraInitial != null &&
            !cameraInitial.IsActive;

        Transform currentSpawnPoint;

        if (isZoomed)
        {
            currentSpawnPoint = zoomBulletSpawnSpot;
        }
        else
        {
            currentSpawnPoint = bulletSpawnSpot;
        }

        if (currentSpawnPoint == null)
            return;

        GameObject selectedBullet =
            GetSelectedBullet();

        if (selectedBullet == null)
            return;

        Vector3 shootDirection;

        if (!isZoomed)
        {
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            Ray ray =
                playerCamera.ScreenPointToRay(
                    mousePosition
                );

            RaycastHit hit;

            Vector3 targetPoint;

            if (Physics.Raycast(
                ray,
                out hit,
                aimDistance
            ))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint =
                    ray.origin +
                    ray.direction *
                    aimDistance;
            }

            shootDirection =
                targetPoint -
                currentSpawnPoint.position;

            if (shootDirection.sqrMagnitude <= 0.001f)
                return;

            shootDirection.Normalize();

            float frontCheck =
                Vector3.Dot(
                    currentSpawnPoint.forward,
                    shootDirection
                );

            if (frontCheck <= 0f)
                return;
        }
        else
        {
            shootDirection =
                playerCamera.transform.forward;

            shootDirection.Normalize();
        }

        GameObject newBullet =
            Instantiate(
                selectedBullet,
                currentSpawnPoint.position,
                Quaternion.LookRotation(
                    shootDirection
                )
            );

        newBullet.transform.localScale =
            selectedBullet.transform.localScale;

        newBullet.SetActive(true);

        PlayerBullet playerBullet =
            newBullet.GetComponent<PlayerBullet>();

        if (playerBullet != null)
        {
            playerBullet.bulletColor =
                GetCurrentBulletColor();
        }

        ammoCount.RemoveAmmo();
    }

    bool HasPotion(int color)
    {
        switch (color)
        {
            case 1:
                return redCount != null &&
                       redCount.GetRedPotionCount() > 0;

            case 2:
                return blueCount != null &&
                       blueCount.GetBluePotionCount() > 0;

            case 3:
                return greenCount != null &&
                       greenCount.GetGreenPotionCount() > 0;

            case 4:
                return yellowCount != null &&
                       yellowCount.GetYellowPotionCount() > 0;

            default:
                return false;
        }
    }

    void RemovePotion(int color)
    {
        switch (color)
        {
            case 1:
                if (redCount != null)
                    redCount.RemoveRedPotion();
                break;

            case 2:
                if (blueCount != null)
                    blueCount.RemoveBluePotion();
                break;

            case 3:
                if (greenCount != null)
                    greenCount.RemoveGreenPotion();
                break;

            case 4:
                if (yellowCount != null)
                    yellowCount.RemoveYellowPotion();
                break;
        }

        if (currentBulletColor == color &&
            !HasPotion(color))
        {
            SelectFirstAvailableColor();
        }
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
                return Color.white;
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