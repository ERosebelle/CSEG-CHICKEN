using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    public float bulletSpeed = 100f;
    public float bulletLifetime = 5f;
    public float bulletSize = 0.15f;

    [Header("Fire Point")]
    public Transform playerCamera;

    private Color currentBulletColor = Color.red;

    void Start()
    {
        if (playerCamera == null)
        {
            Camera cam = GetComponentInChildren<Camera>();

            if (cam != null)
                playerCamera = cam.transform;
        }
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null)
            return;

        // 1 = Red
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentBulletColor = Color.red;
        }

        // 2 = Blue
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentBulletColor = Color.blue;
        }

        // 3 = Green
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentBulletColor = Color.green;
        }

        // 4 = Yellow
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentBulletColor = Color.yellow;
        }

        // Right Mouse Button = Shoot
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (playerCamera == null)
            return;

        // Create the bullet as a sphere
        GameObject bullet =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Put the bullet in front of the camera
        bullet.transform.position =
            playerCamera.position + playerCamera.forward * 1f;

        // Match the camera direction
        bullet.transform.rotation =
            playerCamera.rotation;

        // Set bullet size
        bullet.transform.localScale =
            Vector3.one * bulletSize;

        // Set bullet color
        Renderer bulletRenderer =
            bullet.GetComponent<Renderer>();

        if (bulletRenderer != null)
        {
            bulletRenderer.material.color =
                currentBulletColor;
        }

        // Add Rigidbody
        Rigidbody bulletRb =
            bullet.AddComponent<Rigidbody>();

        bulletRb.useGravity = false;

        bulletRb.collisionDetectionMode =
            CollisionDetectionMode.Continuous;

        // Shoot forward
        bulletRb.linearVelocity =
            playerCamera.forward * bulletSpeed;

        // Add collision behavior directly to this bullet
        BulletCollision bulletCollision =
            bullet.AddComponent<BulletCollision>();

        bulletCollision.bulletColor =
            currentBulletColor;

        // Destroy bullet after lifetime
        Destroy(bullet, bulletLifetime);
    }
}

public class BulletCollision : MonoBehaviour
{
    public Color bulletColor;

    void OnCollisionEnter(Collision collision)
    {
        Renderer targetRenderer =
            collision.gameObject.GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            targetRenderer.material.color =
                bulletColor;
        }

        Destroy(gameObject);
    }
}