using UnityEngine;

public class PlayerEMP : MonoBehaviour
{
    public enum CharacterType
    {
        ChickenA,
        ChickenB,
        ChickenC
    }

    [Header("Characters")]
    public GameObject chickenA;
    public GameObject chickenB;
    public GameObject chickenC;

    [Header("Character Switching")]
    public float switchCooldown = 30f;

    private CharacterType currentCharacter;
    private float switchCooldownTimer = 0f;

    private void Start()
    {
        SelectCharacter(CharacterType.ChickenA);

        // Player must wait 30 seconds before first switch
        switchCooldownTimer = switchCooldown;
    }

    private void Update()
    {
        if (switchCooldownTimer > 0f)
        {
            switchCooldownTimer -= Time.deltaTime;

            if (switchCooldownTimer < 0f)
            {
                switchCooldownTimer = 0f;
            }
        }
    }

    // ==========================================
    // SELECT CHARACTER
    // ==========================================

    public void SelectCharacter(CharacterType character)
    {
        currentCharacter = character;

        if (chickenA != null)
            chickenA.SetActive(character == CharacterType.ChickenA);

        if (chickenB != null)
            chickenB.SetActive(character == CharacterType.ChickenB);

        if (chickenC != null)
            chickenC.SetActive(character == CharacterType.ChickenC);
    }

    // ==========================================
    // SWITCH CHARACTER
    // ==========================================

    public bool TrySwitchCharacter(CharacterType character)
    {
        // Still on cooldown
        if (switchCooldownTimer > 0f)
        {
            Debug.Log(
                "Character switching unavailable. "
                + Mathf.Ceil(switchCooldownTimer)
                + " seconds remaining."
            );

            return false;
        }

        // Already using this character
        if (currentCharacter == character)
        {
            return false;
        }

        // Switch character
        SelectCharacter(character);

        // Start another 30-second cooldown
        switchCooldownTimer = switchCooldown;

        Debug.Log("Character switched! Cooldown started.");

        return true;
    }

    // ==========================================
    // GET CURRENT CHARACTER
    // ==========================================

    public CharacterType GetCurrentCharacter()
    {
        return currentCharacter;
    }

    public GameObject GetCurrentCharacterObject()
    {
        switch (currentCharacter)
        {
            case CharacterType.ChickenA:
                return chickenA;

            case CharacterType.ChickenB:
                return chickenB;

            case CharacterType.ChickenC:
                return chickenC;
        }

        return null;
    }

    // ==========================================
    // COOLDOWN
    // ==========================================

    public float GetSwitchCooldownTimer()
    {
        return switchCooldownTimer;
    }

    public bool CanSwitchCharacter()
    {
        return switchCooldownTimer <= 0f;
    }
}