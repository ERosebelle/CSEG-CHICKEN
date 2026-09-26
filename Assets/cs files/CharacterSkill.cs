using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class CharacterSkill : MonoBehaviour
{
    public PlayerEMP playerEMP;

    [Header("Cooldown")]
    public float skillCooldown = 15f;

    [Header("Cooldown UI")]
    public TextMeshProUGUI cooldownText;

    [Header("Chicken A - Roll")]
    public float rollSpeed = 25f;
    public float rollDuration = 0.5f;

    [Header("Chicken C - Teleport")]
    public float teleportDistance = 80f;

    [Header("Teleport Camera")]
    public Camera teleportCamera;

    [Header("Teleport Boundary")]
    public Collider northWall;
    public Collider southWall;
    public Collider eastWall;
    public Collider westWall;

    private float skillCooldownTimer = 0f;

    // =========================================================
    // CHICKEN C
    // =========================================================

    private bool teleportReady = false;

    // =========================================================
    // CHICKEN A
    // =========================================================

    private bool isRolling = false;

    // =========================================================
    // TUTORIAL ONLY
    // =========================================================

    private bool tutorialSkillActivated = false;

    private bool IsTutorialScene()
    {
        return SceneManager.GetActiveScene().name == "Tutorial Scene";
    }

    public bool WasSkillActivatedInTutorial()
    {
        return tutorialSkillActivated;
    }

    private void MarkTutorialSkillActivated()
    {
        if (tutorialSkillActivated)
            return;

        TutorialController tutorialController =
            FindFirstObjectByType<TutorialController>(
                FindObjectsInactive.Include
            );

        if (tutorialController == null)
        {
            Debug.LogWarning(
                "CharacterSkill: TutorialController NOT FOUND!"
            );

            return;
        }

        tutorialSkillActivated = true;

        Debug.Log(
            "TUTORIAL: CHARACTER SKILL ACTIVATED"
        );

        Debug.Log(
            "TUTORIAL: SENDING SKILL COMPLETION TO TUTORIAL CONTROLLER"
        );

        tutorialController.CharacterSkillActivated();
    }

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        if (teleportCamera == null)
        {
            teleportCamera = Camera.main;
        }

        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(false);
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // =====================================================
        // COOLDOWN
        // =====================================================

        if (skillCooldownTimer > 0f)
        {
            skillCooldownTimer -= Time.deltaTime;

            if (skillCooldownTimer < 0f)
            {
                skillCooldownTimer = 0f;
            }
        }

        // =====================================================
        // COOLDOWN UI
        // =====================================================

        if (cooldownText != null)
        {
            if (skillCooldownTimer > 0f)
            {
                cooldownText.text =
                    Mathf.CeilToInt(
                        skillCooldownTimer
                    ).ToString();

                cooldownText.gameObject.SetActive(true);
            }
            else
            {
                cooldownText.gameObject.SetActive(false);
            }
        }

        // =====================================================
        // ENTER - ACTIVATE SKILL
        // =====================================================

        if (Keyboard.current != null &&
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            PrepareSkill();
        }

        // =====================================================
        // LEFT CLICK - TELEPORT
        // =====================================================

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (teleportReady)
            {
                TeleportToCursor();
            }
        }
    }

    // =========================================================
    // PREPARE SKILL
    // =========================================================

    private void PrepareSkill()
    {
        if (playerEMP == null)
            return;

        PlayerEMP.CharacterType currentCharacter =
            playerEMP.GetCurrentCharacter();

        // =====================================================
        // CHICKEN A
        // =====================================================

        if (currentCharacter ==
            PlayerEMP.CharacterType.ChickenA)
        {
            UseRoll();
        }

        // =====================================================
        // CHICKEN B
        // =====================================================

        else if (currentCharacter ==
                 PlayerEMP.CharacterType.ChickenB)
        {
            UseDoubleJump();
        }

        // =====================================================
        // CHICKEN C
        // =====================================================

        else if (currentCharacter ==
                 PlayerEMP.CharacterType.ChickenC)
        {
            PrepareTeleport();
        }
    }

    // =========================================================
    // CHICKEN A - ROLL
    // =========================================================

    private void UseRoll()
    {
        if (isRolling)
        {
            Debug.Log(
                "CHICKEN A: ALREADY ROLLING!"
            );

            return;
        }

        if (skillCooldownTimer > 0f)
        {
            Debug.Log(
                "Roll is on cooldown: "
                + Mathf.Ceil(skillCooldownTimer)
                + " seconds remaining."
            );

            return;
        }

        GameObject currentChicken =
            playerEMP.GetCurrentCharacterObject();

        if (currentChicken == null)
            return;

        Vector3 rollDirection =
            Vector3.zero;

        float rotationDirection =
            0f;

        if (Keyboard.current.upArrowKey.isPressed)
        {
            rollDirection =
                playerEMP.transform.forward;

            rotationDirection =
                1f;
        }
        else if (Keyboard.current.downArrowKey.isPressed)
        {
            rollDirection =
                -playerEMP.transform.forward;

            rotationDirection =
                -1f;
        }
        else if (Keyboard.current.leftArrowKey.isPressed)
        {
            rollDirection =
                -playerEMP.transform.right;

            rotationDirection =
                -1f;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            rollDirection =
                playerEMP.transform.right;

            rotationDirection =
                1f;
        }
        else
        {
            Debug.Log(
                "CHICKEN A: PRESS ENTER + AN ARROW KEY TO ROLL!"
            );

            return;
        }

        StartCoroutine(
            RollAnimation(
                currentChicken,
                rollDirection,
                rotationDirection
            )
        );

        Debug.Log(
            "CHICKEN A: ROLL ACTIVATED!"
        );

        MarkTutorialSkillActivated();

        skillCooldownTimer =
            skillCooldown;
    }

    // =========================================================
    // ROLL ANIMATION
    // =========================================================

    private IEnumerator RollAnimation(
        GameObject chicken,
        Vector3 direction,
        float rotationDirection)
    {
        isRolling = true;

        Quaternion originalRotation =
            chicken.transform.localRotation;

        float elapsedTime =
            0f;

        while (elapsedTime < rollDuration)
        {
            elapsedTime +=
                Time.deltaTime;

            float progress =
                Mathf.Clamp01(
                    elapsedTime /
                    rollDuration
                );

            playerEMP.transform.position +=
                direction *
                rollSpeed *
                Time.deltaTime;

            float rotationAmount =
                360f *
                progress *
                rotationDirection;

            if (Mathf.Abs(direction.z) >
                Mathf.Abs(direction.x))
            {
                chicken.transform.localRotation =
                    originalRotation *
                    Quaternion.Euler(
                        rotationAmount,
                        0f,
                        0f
                    );
            }
            else
            {
                chicken.transform.localRotation =
                    originalRotation *
                    Quaternion.Euler(
                        0f,
                        0f,
                        -rotationAmount
                    );
            }

            yield return null;
        }

        chicken.transform.localRotation =
            originalRotation;

        isRolling = false;
    }

    // =========================================================
    // CHICKEN B - DOUBLE JUMP
    // =========================================================

    private void UseDoubleJump()
    {
        PlayerController controller =
            playerEMP.GetComponent<PlayerController>();

        if (controller == null)
        {
            Debug.LogError(
                "PlayerEMP does not have PlayerController!"
            );

            return;
        }

        if (controller.CanDoubleJump())
        {
            controller.PerformDoubleJump();

            Debug.Log(
                "CHICKEN B: DOUBLE JUMP ACTIVATED!"
            );

            MarkTutorialSkillActivated();
        }
        else
        {
            Debug.Log(
                "DOUBLE JUMP NOT AVAILABLE!"
            );
        }
    }

    // =========================================================
    // CHICKEN C - PREPARE TELEPORT
    // =========================================================

    private void PrepareTeleport()
    {
        if (skillCooldownTimer > 0f)
        {
            Debug.Log(
                "Teleport is on cooldown: "
                + Mathf.Ceil(skillCooldownTimer)
                + " seconds remaining."
            );

            return;
        }

        teleportReady = true;

        Debug.Log(
            "CHICKEN C: TELEPORT READY! LEFT CLICK TO CHOOSE DIRECTION."
        );
    }

    // =========================================================
    // CHICKEN C - TELEPORT
    // =========================================================

    private void TeleportToCursor()
    {
        if (playerEMP == null)
            return;

        if (teleportCamera == null)
        {
            teleportCamera =
                Camera.main;

            if (teleportCamera == null)
            {
                Debug.LogError(
                    "Teleport Camera not found!"
                );

                return;
            }
        }

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            teleportCamera.ScreenPointToRay(
                mousePosition
            );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            1000f))
        {
            Vector3 direction =
                hit.point -
                playerEMP.transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.01f)
            {
                Debug.Log(
                    "TELEPORT FAILED: Cursor is too close to player."
                );

                teleportReady = false;

                return;
            }

            direction.Normalize();

            Vector3 teleportPosition =
                playerEMP.transform.position +
                direction *
                teleportDistance;

            // =================================================
            // CHECK INVISIBLE WALLS
            // =================================================

            if (TeleportHitsBoundary(
                playerEMP.transform.position,
                teleportPosition))
            {
                Debug.Log(
                    "TELEPORT BLOCKED: Invisible Wall boundary detected."
                );

                teleportReady = false;

                return;
            }

            playerEMP.transform.position =
                teleportPosition;

            Debug.Log(
                "CHICKEN C: TELEPORT ACTIVATED! Distance: "
                + teleportDistance
            );

            MarkTutorialSkillActivated();

            skillCooldownTimer =
                skillCooldown;

            teleportReady = false;
        }
        else
        {
            Debug.Log(
                "TELEPORT FAILED: Cursor is not pointing at a valid surface."
            );
        }
    }

    // =========================================================
    // TELEPORT BOUNDARY CHECK
    // =========================================================

    private bool TeleportHitsBoundary(
        Vector3 startPosition,
        Vector3 endPosition)
    {
        Vector3 direction =
            endPosition -
            startPosition;

        float distance =
            direction.magnitude;

        if (distance <= 0.01f)
            return false;

        direction.Normalize();

        RaycastHit hit;

        // -----------------------------------------------------
        // NORTH WALL
        // -----------------------------------------------------

        if (northWall != null &&
            northWall.Raycast(
                new Ray(startPosition, direction),
                out hit,
                distance))
        {
            return true;
        }

        // -----------------------------------------------------
        // SOUTH WALL
        // -----------------------------------------------------

        if (southWall != null &&
            southWall.Raycast(
                new Ray(startPosition, direction),
                out hit,
                distance))
        {
            return true;
        }

        // -----------------------------------------------------
        // EAST WALL
        // -----------------------------------------------------

        if (eastWall != null &&
            eastWall.Raycast(
                new Ray(startPosition, direction),
                out hit,
                distance))
        {
            return true;
        }

        // -----------------------------------------------------
        // WEST WALL
        // -----------------------------------------------------

        if (westWall != null &&
            westWall.Raycast(
                new Ray(startPosition, direction),
                out hit,
                distance))
        {
            return true;
        }

        return false;
    }

    // =========================================================
    // GETTERS
    // =========================================================

    public float GetSkillCooldownTimer()
    {
        return skillCooldownTimer;
    }

    public bool CanUseSkill()
    {
        return skillCooldownTimer <= 0f;
    }
}