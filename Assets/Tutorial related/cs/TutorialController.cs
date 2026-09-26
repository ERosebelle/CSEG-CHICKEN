using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TutorialController : MonoBehaviour
{
    [Header("PLAYER")]
    public GameObject playerEMP;

    // =========================================================
    // TASK 1 - MOVEMENT
    // =========================================================

    [Header("TASK 1 - MOVEMENT")]
    public GameObject movementWall;
    public TextMeshProUGUI movementText;

    private bool pressedUp = false;
    private bool pressedDown = false;
    private bool pressedLeft = false;
    private bool pressedRight = false;

    private bool movementTutorialComplete = false;

    // =========================================================
    // TASK 2 - JUMP
    // =========================================================

    [Header("TASK 2 - JUMP")]
    public GameObject jumpWall;
    public TextMeshProUGUI jumpText;

    private bool normalJumpDetected = false;
    private bool highJumpDetected = false;

    private bool jumpTutorialComplete = false;

    private bool holdingSpace = false;
    private float spaceHoldTime = 0f;

    // =========================================================
    // TASK 3 - AMMO
    // =========================================================

    [Header("TASK 3 - AMMO")]
    public GameObject ammoWall;
    public TextMeshProUGUI ammoText;

    [Header("AMMO POTION")]
    public GameObject ammoPotion;

    [Header("TASK 3 - UI")]
    public GameObject border;
    public GameObject eggAmmoCountEMP;

    private bool ammoTutorialComplete = false;
    private bool ammoPotionActivated = false;

    // =========================================================
    // TASK 4 - SHOOTING ENEMY
    // =========================================================

    [Header("TASK 4 - SHOOTING ENEMY")]
    public GameObject enemy1;
    public GameObject enemy1Wall;
    public TextMeshProUGUI enemy1Text;

    private bool enemy1Activated = false;
    private bool enemy1Defeated = false;

    // =========================================================
    // TASK 5 - ZOOM
    // =========================================================

    [Header("TASK 5 - ZOOM")]
    public GameObject zoomWall;
    public TextMeshProUGUI zoomText;

    private bool zoomTutorialComplete = false;
    private bool zoomDetected = false;

    // =========================================================
    // TASK 6 - CHARACTER SWITCHING
    // =========================================================

    [Header("TASK 6 - CHARACTER SWITCHING")]
    public GameObject characterSwitchUI;
    public GameObject characterSwitchWall;
    public TextMeshProUGUI characterSwitchText;

    private bool characterSwitchTutorialComplete = false;

    private PlayerEMP playerEMPComponent;

    private PlayerEMP.CharacterType characterBeforeSwitch;

    private bool characterBeforeSwitchSaved = false;

    // =========================================================
    // TASK 7 - CHARACTER SKILL
    // =========================================================

    [Header("TASK 7 - CHARACTER SKILL")]
    public GameObject characterSkillWall;
    public TextMeshProUGUI characterSkillText;

    private bool characterSkillTutorialComplete = false;

    private CharacterSkill characterSkillComponent;

    // =========================================================
    // TASK 8 - CORRUPTED PLANT
    // =========================================================

    [Header("TASK 8 - CORRUPTED PLANT")]
    public GameObject corruptedPlant;
    public GameObject corruptedPlantWall;
    public TextMeshProUGUI corruptedPlantText;

    private bool corruptedPlantActivated = false;
    private bool corruptedPlantDefeated = false;

    // =========================================================
    // TASK 9 - HEALTH POTION
    // =========================================================

    [Header("TASK 9 - HEALTH POTION")]
    public TextMeshProUGUI healthPotionText;

    private bool healthPotionTutorialComplete = false;
    private bool loadingFarmRoom = false;

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // =====================================================
        // PLAYER
        // =====================================================

        if (playerEMP != null)
        {
            playerEMPComponent =
                playerEMP.GetComponent<PlayerEMP>();

            characterSkillComponent =
                playerEMP.GetComponent<CharacterSkill>();
        }

        // =====================================================
        // TASK 1
        // =====================================================

        if (movementWall != null)
        {
            movementWall.SetActive(true);
        }

        if (movementText != null)
        {
            movementText.gameObject.SetActive(true);
        }

        // =====================================================
        // TASK 2
        // =====================================================

        if (jumpWall != null)
        {
            jumpWall.SetActive(false);
        }

        if (jumpText != null)
        {
            jumpText.gameObject.SetActive(false);
        }

        // =====================================================
        // TASK 3
        // =====================================================

        if (ammoWall != null)
        {
            ammoWall.SetActive(false);
        }

        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(false);
        }

        if (ammoPotion != null)
        {
            ammoPotion.SetActive(false);
        }

        // =====================================================
        // TASK 3 UI
        // =====================================================

        if (border != null)
        {
            border.SetActive(false);
        }

        if (eggAmmoCountEMP != null)
        {
            eggAmmoCountEMP.SetActive(false);
        }

        // =====================================================
        // TASK 4
        // =====================================================

        if (enemy1Wall != null)
        {
            enemy1Wall.SetActive(false);
        }

        if (enemy1Text != null)
        {
            enemy1Text.gameObject.SetActive(false);
        }

        if (enemy1 != null)
        {
            enemy1.SetActive(false);
        }

        // =====================================================
        // TASK 5
        // =====================================================

        if (zoomWall != null)
        {
            zoomWall.SetActive(false);
        }

        if (zoomText != null)
        {
            zoomText.gameObject.SetActive(false);
        }

        // =====================================================
        // TASK 6
        // =====================================================

        if (characterSwitchUI != null)
        {
            characterSwitchUI.SetActive(false);
        }

        if (characterSwitchWall != null)
        {
            characterSwitchWall.SetActive(false);
        }

        if (characterSwitchText != null)
        {
            characterSwitchText.gameObject.SetActive(false);
        }

        // =====================================================
        // TASK 7
        // =====================================================

        if (characterSkillWall != null)
        {
            characterSkillWall.SetActive(false);
        }

        if (characterSkillText != null)
        {
            characterSkillText.gameObject.SetActive(false);
        }

        // =====================================================
        // TASK 8
        // =====================================================

        if (corruptedPlantWall != null)
        {
            corruptedPlantWall.SetActive(false);
        }

        if (corruptedPlantText != null)
        {
            corruptedPlantText.gameObject.SetActive(false);
        }

        if (corruptedPlant != null)
        {
            corruptedPlant.SetActive(false);
        }

        // =====================================================
        // TASK 9
        // =====================================================

        if (healthPotionText != null)
        {
            healthPotionText.gameObject.SetActive(false);
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        // =====================================================
        // TASK 1
        // =====================================================

        if (!movementTutorialComplete)
        {
            HandleMovementTutorial();
            return;
        }

        // =====================================================
        // TASK 2
        // =====================================================

        if (!jumpTutorialComplete)
        {
            HandleJumpTutorial();
            return;
        }

        // =====================================================
        // TASK 3
        // =====================================================

        if (!ammoTutorialComplete)
        {
            HandleAmmoTutorial();
            return;
        }

        // =====================================================
        // TASK 4
        // =====================================================

        if (!enemy1Defeated)
        {
            HandleEnemyTutorial();
            return;
        }

        // =====================================================
        // TASK 5
        // =====================================================

        if (!zoomTutorialComplete)
        {
            HandleZoomTutorial();
            return;
        }

        // =====================================================
        // TASK 6
        // =====================================================

        if (!characterSwitchTutorialComplete)
        {
            HandleCharacterSwitchTutorial();
            return;
        }

        // =====================================================
        // TASK 7
        // =====================================================

        if (!characterSkillTutorialComplete)
        {
            HandleCharacterSkillTutorial();
            return;
        }

        // =====================================================
        // TASK 8
        // =====================================================

        if (!corruptedPlantDefeated)
        {
            HandleCorruptedPlantTutorial();
            return;
        }

        // =====================================================
        // TASK 9
        // =====================================================

        if (!healthPotionTutorialComplete)
        {
            HandleHealthPotionTutorial();
            return;
        }
    }

    // =========================================================
    // TASK 1 - MOVEMENT
    // =========================================================

    private void HandleMovementTutorial()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            pressedUp = true;

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            pressedDown = true;

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            pressedLeft = true;

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            pressedRight = true;

        if (pressedUp &&
            pressedDown &&
            pressedLeft &&
            pressedRight)
        {
            CompleteMovementTutorial();
        }
    }

    private void CompleteMovementTutorial()
    {
        movementTutorialComplete = true;

        Debug.Log(
            "TUTORIAL PART 1 COMPLETE: MOVEMENT"
        );

        if (movementWall != null)
            movementWall.SetActive(false);

        if (movementText != null)
            movementText.gameObject.SetActive(false);

        if (jumpWall != null)
            jumpWall.SetActive(true);

        if (jumpText != null)
            jumpText.gameObject.SetActive(true);
    }

    // =========================================================
    // TASK 2 - JUMP
    // =========================================================

    private void HandleJumpTutorial()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            holdingSpace = true;
            spaceHoldTime = 0f;
        }

        if (holdingSpace &&
            Keyboard.current.spaceKey.isPressed)
        {
            spaceHoldTime += Time.deltaTime;
        }

        if (holdingSpace &&
            Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            holdingSpace = false;

            if (spaceHoldTime < 0.3f)
            {
                normalJumpDetected = true;

                Debug.Log(
                    "TUTORIAL: NORMAL JUMP DETECTED"
                );
            }
            else
            {
                highJumpDetected = true;

                Debug.Log(
                    "TUTORIAL: HIGH JUMP DETECTED"
                );
            }
        }

        if (normalJumpDetected &&
            highJumpDetected)
        {
            CompleteJumpTutorial();
        }
    }

    private void CompleteJumpTutorial()
    {
        jumpTutorialComplete = true;

        Debug.Log(
            "TUTORIAL PART 2 COMPLETE: JUMP"
        );

        if (jumpWall != null)
            jumpWall.SetActive(false);

        if (jumpText != null)
            jumpText.gameObject.SetActive(false);

        if (ammoWall != null)
            ammoWall.SetActive(true);

        if (ammoText != null)
            ammoText.gameObject.SetActive(true);

        if (ammoPotion != null)
        {
            ammoPotion.SetActive(true);
            ammoPotionActivated = true;
        }
    }

    // =========================================================
    // TASK 3 - AMMO
    // =========================================================

    private void HandleAmmoTutorial()
    {
        if (!ammoPotionActivated)
            return;

        if (ammoPotion == null)
            return;

        if (!ammoPotion.activeSelf)
            CompleteAmmoTutorial();
    }

    private void CompleteAmmoTutorial()
    {
        ammoTutorialComplete = true;

        Debug.Log(
            "TUTORIAL PART 3 COMPLETE: AMMO"
        );

        if (ammoWall != null)
            ammoWall.SetActive(false);

        if (ammoText != null)
            ammoText.gameObject.SetActive(false);

        if (border != null)
            border.SetActive(true);

        if (eggAmmoCountEMP != null)
            eggAmmoCountEMP.SetActive(true);

        StartEnemyTutorial();
    }

    // =========================================================
    // TASK 4
    // =========================================================

    private void StartEnemyTutorial()
    {
        Debug.Log(
            "TUTORIAL PART 4 STARTING: DEFEAT ENEMY 1"
        );

        if (enemy1Wall != null)
            enemy1Wall.SetActive(true);

        if (enemy1Text != null)
            enemy1Text.gameObject.SetActive(true);

        if (enemy1 != null)
        {
            enemy1.SetActive(true);
            enemy1Activated = true;
        }
    }

    private void HandleEnemyTutorial()
    {
        if (!enemy1Activated)
            return;

        if (enemy1 == null)
            CompleteEnemyTutorial();
    }

    public void Enemy1Defeated()
    {
        CompleteEnemyTutorial();
    }

    private void CompleteEnemyTutorial()
    {
        if (enemy1Defeated)
            return;

        enemy1Defeated = true;

        if (enemy1Wall != null)
            enemy1Wall.SetActive(false);

        if (enemy1Text != null)
            enemy1Text.gameObject.SetActive(false);

        StartZoomTutorial();
    }

    // =========================================================
    // TASK 5
    // =========================================================

    private void StartZoomTutorial()
    {
        Debug.Log(
            "TUTORIAL PART 5 STARTING: ZOOM"
        );

        if (zoomWall != null)
            zoomWall.SetActive(true);

        if (zoomText != null)
            zoomText.gameObject.SetActive(true);
    }

    private void HandleZoomTutorial()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            zoomDetected = true;
        }

        if (zoomDetected)
            CompleteZoomTutorial();
    }

    private void CompleteZoomTutorial()
    {
        if (zoomTutorialComplete)
            return;

        zoomTutorialComplete = true;

        if (zoomWall != null)
            zoomWall.SetActive(false);

        if (zoomText != null)
            zoomText.gameObject.SetActive(false);

        StartCharacterSwitchTutorial();
    }

    // =========================================================
    // TASK 6 - DO NOT CHANGE
    // =========================================================

    private void StartCharacterSwitchTutorial()
    {
        Debug.Log(
            "TUTORIAL PART 6 STARTING: CHARACTER SWITCHING"
        );

        if (playerEMPComponent != null)
        {
            characterBeforeSwitch =
                playerEMPComponent.GetCurrentCharacter();

            characterBeforeSwitchSaved = true;
        }

        if (characterSwitchWall != null)
            characterSwitchWall.SetActive(true);

        if (characterSwitchText != null)
            characterSwitchText.gameObject.SetActive(true);

        if (characterSwitchUI != null)
            characterSwitchUI.SetActive(true);
    }

    private void HandleCharacterSwitchTutorial()
    {
        if (!characterBeforeSwitchSaved)
            return;

        if (playerEMPComponent == null)
            return;

        PlayerEMP.CharacterType currentCharacter =
            playerEMPComponent.GetCurrentCharacter();

        if (currentCharacter != characterBeforeSwitch)
            CompleteCharacterSwitchTutorial();
    }

    public void CharacterSwitchButtonPressed()
    {
        if (characterSwitchTutorialComplete)
            return;

        CompleteCharacterSwitchTutorial();
    }

    private void CompleteCharacterSwitchTutorial()
    {
        if (characterSwitchTutorialComplete)
            return;

        characterSwitchTutorialComplete = true;

        if (characterSwitchWall != null)
            characterSwitchWall.SetActive(false);

        if (characterSwitchText != null)
            characterSwitchText.gameObject.SetActive(false);

        if (characterSwitchUI != null)
            characterSwitchUI.SetActive(false);

        StartCharacterSkillTutorial();
    }

    // =========================================================
    // TASK 7
    // =========================================================

    private void StartCharacterSkillTutorial()
    {
        Debug.Log(
            "TUTORIAL PART 7 STARTING: CHARACTER SKILL"
        );

        if (characterSkillWall != null)
            characterSkillWall.SetActive(true);

        if (characterSkillText != null)
            characterSkillText.gameObject.SetActive(true);

        if (characterSkillComponent == null &&
            playerEMP != null)
        {
            characterSkillComponent =
                playerEMP.GetComponent<CharacterSkill>();
        }

        if (characterSkillComponent != null &&
            characterSkillComponent.WasSkillActivatedInTutorial())
        {
            CompleteCharacterSkillTutorial();
            return;
        }
    }

    private void HandleCharacterSkillTutorial()
    {
        if (characterSkillTutorialComplete)
            return;

        if (characterSkillComponent == null &&
            playerEMP != null)
        {
            characterSkillComponent =
                playerEMP.GetComponent<CharacterSkill>();
        }

        if (characterSkillComponent == null)
            return;

        if (characterSkillComponent.WasSkillActivatedInTutorial())
            CompleteCharacterSkillTutorial();
    }

    public void CharacterSkillActivated()
    {
        if (characterSkillTutorialComplete)
            return;

        CompleteCharacterSkillTutorial();
    }

    private void CompleteCharacterSkillTutorial()
    {
        if (characterSkillTutorialComplete)
            return;

        characterSkillTutorialComplete = true;

        if (characterSkillWall != null)
            characterSkillWall.SetActive(false);

        if (characterSkillText != null)
            characterSkillText.gameObject.SetActive(false);

        StartCorruptedPlantTutorial();
    }

    // =========================================================
    // TASK 8
    // =========================================================

    private void StartCorruptedPlantTutorial()
    {
        Debug.Log(
            "TUTORIAL PART 8 STARTING: CORRUPTED PLANT"
        );

        if (corruptedPlantWall != null)
            corruptedPlantWall.SetActive(true);

        if (corruptedPlantText != null)
            corruptedPlantText.gameObject.SetActive(true);

        if (corruptedPlant != null)
        {
            corruptedPlant.SetActive(true);
            corruptedPlantActivated = true;
        }

        Debug.Log(
            "TUTORIAL PART 8 STARTED: CORRUPTED PLANT"
        );
    }

    private void HandleCorruptedPlantTutorial()
    {
        if (!corruptedPlantActivated)
            return;

        if (corruptedPlantDefeated)
            return;

        PlayerHealth playerHealth =
            GetPlayerHealth();

        if (playerHealth == null)
            return;

        float healthPercent =
            playerHealth.GetHealthPercent();

        if (healthPercent <= 0.5f)
        {
            Debug.Log(
                "TUTORIAL: PLAYER HEALTH REACHED 50%"
            );

            DefeatCorruptedPlant();
        }
    }

    // =========================================================
    // GET PLAYER HEALTH FROM PLAYEREMP
    // =========================================================

    private PlayerHealth GetPlayerHealth()
    {
        if (playerEMP == null)
            return null;

        PlayerHealth health =
            playerEMP.GetComponent<PlayerHealth>();

        if (health != null)
            return health;

        health =
            playerEMP.GetComponentInChildren<PlayerHealth>(true);

        if (health != null)
            return health;

        health =
            playerEMP.GetComponentInParent<PlayerHealth>();

        return health;
    }

    // =========================================================
    // DEFEAT CORRUPTED PLANT
    // =========================================================

    private void DefeatCorruptedPlant()
    {
        if (corruptedPlantDefeated)
            return;

        corruptedPlantDefeated = true;

        Debug.Log(
            "TUTORIAL PART 8: PLAYER HEALTH REACHED 50%"
        );

        if (corruptedPlant != null)
        {
            corruptedPlant.SetActive(false);
        }

        if (corruptedPlantWall != null)
        {
            corruptedPlantWall.SetActive(false);
        }

        if (corruptedPlantText != null)
        {
            corruptedPlantText.gameObject.SetActive(false);
        }

        Debug.Log(
            "TUTORIAL PART 8 COMPLETE: CORRUPTED PLANT DISABLED"
        );

        StartHealthPotionTutorial();
    }

    // =========================================================
    // TASK 9 - HEALTH POTION
    // =========================================================

    private void StartHealthPotionTutorial()
    {
        Debug.Log(
            "TUTORIAL PART 9 STARTING: DRINK HEALTH POTION"
        );

        if (healthPotionText != null)
        {
            healthPotionText.gameObject.SetActive(true);
        }
    }

    private void HandleHealthPotionTutorial()
    {
        if (loadingFarmRoom)
            return;

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            DrinkHealthPotion();
        }
    }

    private void DrinkHealthPotion()
    {
        if (healthPotionTutorialComplete)
            return;

        healthPotionTutorialComplete = true;

        Debug.Log(
            "TUTORIAL PART 9: HEALTH POTION DRINKING"
        );

        PlayerHealth playerHealth =
            GetPlayerHealth();

        if (playerHealth != null)
        {
            playerHealth.RestoreFullHealth();

            Debug.Log(
                "TUTORIAL: PLAYER HEALTH RESTORED"
            );
        }

        if (healthPotionText != null)
        {
            healthPotionText.gameObject.SetActive(false);
        }

        StartCoroutine(LoadFarmRoomAfterDelay());
    }

    private IEnumerator LoadFarmRoomAfterDelay()
    {
        loadingFarmRoom = true;

        Debug.Log(
            "TUTORIAL PART 9 COMPLETE: LOADING FARM ROOM IN 3 SECONDS"
        );

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("farmRoom");
    }
}