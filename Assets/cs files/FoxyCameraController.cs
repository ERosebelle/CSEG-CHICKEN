using UnityEngine;

public class FoxyCameraController : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("The Camera component on this GameObject.")]
    public Camera foxyCamera;

    [Header("Starting State")]
    [Tooltip("Camera starts disabled until the player is defeated.")]
    public bool disableAtStart = true;

    private bool isActivated = false;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Find the Camera automatically if not assigned.
        if (foxyCamera == null)
        {
            foxyCamera =
                GetComponent<Camera>();
        }

        if (foxyCamera == null)
        {
            Debug.LogError(
                "FoxyCameraController: No Camera component found!"
            );

            return;
        }

        // Make sure this camera renders to Game View / Display 1.
        foxyCamera.targetDisplay = 0;

        // Do not render into a Render Texture.
        foxyCamera.targetTexture = null;

        // Start disabled.
        if (disableAtStart)
        {
            DisableCamera();
        }

        Debug.Log(
            "FoxyCameraController initialized: " +
            gameObject.name
        );
    }


    // =========================================================
    // ACTIVATE CAMERA
    // =========================================================

    public void ActivateCamera()
    {
        if (foxyCamera == null)
        {
            Debug.LogError(
                "FoxyCameraController: Cannot activate. Camera is missing."
            );

            return;
        }

        Debug.Log(
            "================================"
        );

        Debug.Log(
            "FOXY CAMERA RECEIVED ACTIVATION"
        );


        // Make sure the GameObject is active.
        gameObject.SetActive(true);


        // Make sure the camera renders to Display 1.
        foxyCamera.targetDisplay = 0;


        // Remove Render Texture.
        foxyCamera.targetTexture = null;


        // Enable the Camera component.
        foxyCamera.enabled = true;


        isActivated = true;


        Debug.Log(
            "Foxy Camera ACTIVATED!"
        );

        Debug.Log(
            "Camera: " +
            foxyCamera.name
        );

        Debug.Log(
            "GameObject Active: " +
            gameObject.activeInHierarchy
        );

        Debug.Log(
            "Camera Enabled: " +
            foxyCamera.enabled
        );

        Debug.Log(
            "Target Display: " +
            (foxyCamera.targetDisplay + 1)
        );

        Debug.Log(
            "Target Texture: " +
            foxyCamera.targetTexture
        );

        Debug.Log(
            "Position: " +
            transform.position
        );

        Debug.Log(
            "Rotation: " +
            transform.rotation
        );

        Debug.Log(
            "================================"
        );
    }


    // =========================================================
    // DISABLE CAMERA
    // =========================================================

    public void DisableCamera()
    {
        if (foxyCamera == null)
            return;

        foxyCamera.enabled = false;

        isActivated = false;

        Debug.Log(
            "Foxy Camera disabled."
        );
    }


    // =========================================================
    // CAMERA STATE
    // =========================================================

    public bool IsCameraActivated()
    {
        return isActivated;
    }
}