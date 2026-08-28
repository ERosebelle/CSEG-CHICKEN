using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [Header("E Collect UI")]
    public RectTransform eCollectUI;

    [Header("Camera Settings")]
    public GameObject cameraSettings;

    [Header("UI Height")]
    public float heightOffset = 1.5f;

    private Transform currentTarget;
    private Camera activeCamera;
    private Canvas canvas;

    void Start()
    {
        canvas = eCollectUI.GetComponentInParent<Canvas>();

        if (eCollectUI != null)
        {
            eCollectUI.gameObject.SetActive(false);
        }

        FindActiveCamera();
    }

    void Update()
    {
        if (currentTarget == null)
            return;

        FindActiveCamera();

        if (activeCamera == null)
            return;

        PositionUI();
    }

    void PositionUI()
    {
        Vector3 worldPosition =
            currentTarget.position +
            Vector3.up * heightOffset;

        Vector3 screenPosition =
            activeCamera.WorldToScreenPoint(
                worldPosition
            );

        RectTransform canvasRect =
            canvas.GetComponent<RectTransform>();

        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : activeCamera,
            out localPosition
        );

        eCollectUI.localPosition =
            localPosition;
    }

    void FindActiveCamera()
    {
        if (cameraSettings == null)
            return;

        Camera[] cameras =
            cameraSettings.GetComponentsInChildren<Camera>(
                true
            );

        foreach (Camera cam in cameras)
        {
            if (cam.isActiveAndEnabled)
            {
                activeCamera = cam;
                return;
            }
        }
    }

    public void ShowCollectUI(
        Transform target
    )
    {
        currentTarget = target;

        FindActiveCamera();

        if (eCollectUI != null)
        {
            eCollectUI.gameObject.SetActive(true);
        }
    }

    public void HideCollectUI()
    {
        currentTarget = null;

        if (eCollectUI != null)
        {
            eCollectUI.gameObject.SetActive(false);
        }
    }
}