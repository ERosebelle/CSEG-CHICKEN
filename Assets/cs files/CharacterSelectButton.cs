using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : MonoBehaviour
{
    public PlayerEMP playerEMP;

    [Header("UI References")]
    public GameObject initialCharacterPopup;
    public GameObject permanentCharacterButtons;

    [Header("Cooldown Timer Texts")]
    public TextMeshProUGUI characterATimer;
    public TextMeshProUGUI characterBTimer;
    public TextMeshProUGUI characterCTimer;

    private TutorialController tutorialController;

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // =====================================================
        // INITIAL STATE
        // =====================================================

        if (initialCharacterPopup != null)
        {
            initialCharacterPopup.SetActive(true);
        }

        if (permanentCharacterButtons != null)
        {
            permanentCharacterButtons.SetActive(false);
        }

        // =====================================================
        // HIDE COOLDOWN TIMERS
        // =====================================================

        HideCooldownTimers();

        // =====================================================
        // FIND TUTORIAL CONTROLLER
        // =====================================================

        if (SceneManager.GetActiveScene().name == "Tutorial Scene")
        {
            tutorialController =
                FindFirstObjectByType<TutorialController>(
                    FindObjectsInactive.Include
                );
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (playerEMP == null)
            return;

        float remainingTime =
            playerEMP.GetSwitchCooldownTimer();

        if (remainingTime > 0f)
        {
            int seconds =
                Mathf.CeilToInt(remainingTime);

            ShowCooldownTimers(seconds);
        }
        else
        {
            HideCooldownTimers();
        }
    }

    // =========================================================
    // INITIAL CHARACTER SELECTION
    // =========================================================

    public void SelectInitialCharacterA()
    {
        SelectInitialCharacter(
            PlayerEMP.CharacterType.ChickenA
        );
    }

    public void SelectInitialCharacterB()
    {
        SelectInitialCharacter(
            PlayerEMP.CharacterType.ChickenB
        );
    }

    public void SelectInitialCharacterC()
    {
        SelectInitialCharacter(
            PlayerEMP.CharacterType.ChickenC
        );
    }

    private void SelectInitialCharacter(
        PlayerEMP.CharacterType character)
    {
        if (playerEMP != null)
        {
            playerEMP.SelectCharacter(character);
        }

        // =====================================================
        // CLOSE ONE-TIME CHARACTER POPUP
        // =====================================================

        if (initialCharacterPopup != null)
        {
            initialCharacterPopup.SetActive(false);
        }

        // =====================================================
        // SHOW PERMANENT CHARACTER BUTTONS
        // =====================================================

        if (permanentCharacterButtons != null)
        {
            permanentCharacterButtons.SetActive(true);
        }
    }

    // =========================================================
    // PERMANENT CHARACTER SWITCH BUTTONS
    // =========================================================

    public void SwitchToCharacterA()
    {
        // =====================================================
        // NORMAL GAMEPLAY LOGIC
        // =====================================================

        if (playerEMP != null)
        {
            playerEMP.TrySwitchCharacter(
                PlayerEMP.CharacterType.ChickenA
            );
        }

        // =====================================================
        // TUTORIAL ONLY
        // =====================================================

        NotifyTutorialCharacterSwitch();
    }

    public void SwitchToCharacterB()
    {
        // =====================================================
        // NORMAL GAMEPLAY LOGIC
        // =====================================================

        if (playerEMP != null)
        {
            playerEMP.TrySwitchCharacter(
                PlayerEMP.CharacterType.ChickenB
            );
        }

        // =====================================================
        // TUTORIAL ONLY
        // =====================================================

        NotifyTutorialCharacterSwitch();
    }

    public void SwitchToCharacterC()
    {
        // =====================================================
        // NORMAL GAMEPLAY LOGIC
        // =====================================================

        if (playerEMP != null)
        {
            playerEMP.TrySwitchCharacter(
                PlayerEMP.CharacterType.ChickenC
            );
        }

        // =====================================================
        // TUTORIAL ONLY
        // =====================================================

        NotifyTutorialCharacterSwitch();
    }

    // =========================================================
    // TUTORIAL CHARACTER SWITCH NOTIFICATION
    // =========================================================

    private void NotifyTutorialCharacterSwitch()
    {
        // Do NOTHING outside Tutorial Scene
        if (SceneManager.GetActiveScene().name != "Tutorial Scene")
            return;

        if (tutorialController == null)
        {
            tutorialController =
                FindFirstObjectByType<TutorialController>(
                    FindObjectsInactive.Include
                );
        }

        if (tutorialController != null)
        {
            tutorialController.CharacterSwitchButtonPressed();
        }
    }

    // =========================================================
    // COOLDOWN TIMER UI
    // =========================================================

    private void ShowCooldownTimers(int seconds)
    {
        string timerText =
            seconds.ToString();

        if (characterATimer != null)
        {
            characterATimer.text = timerText;
            characterATimer.gameObject.SetActive(true);
        }

        if (characterBTimer != null)
        {
            characterBTimer.text = timerText;
            characterBTimer.gameObject.SetActive(true);
        }

        if (characterCTimer != null)
        {
            characterCTimer.text = timerText;
            characterCTimer.gameObject.SetActive(true);
        }
    }

    private void HideCooldownTimers()
    {
        if (characterATimer != null)
        {
            characterATimer.gameObject.SetActive(false);
        }

        if (characterBTimer != null)
        {
            characterBTimer.gameObject.SetActive(false);
        }

        if (characterCTimer != null)
        {
            characterCTimer.gameObject.SetActive(false);
        }
    }
}