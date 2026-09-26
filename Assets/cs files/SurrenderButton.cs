using UnityEngine;

public class SurrenderManager : MonoBehaviour
{
    [Header("Defeat Screen")]
    public DefeatScreen defeatScreen;

    [Header("Surrender Button")]
    public GameObject surrenderButton;

    [Header("Game References")]
    public AmmoCount ammoCount;
    public RedCount redCount;
    public BlueCount blueCount;
    public GreenCount greenCount;
    public YellowCount yellowCount;

    private bool hadAmmo = false;

    void Update()
    {
        if (ammoCount.GetAmmoCount() > 0)
        {
            hadAmmo = true;
        }

        bool ammoEmpty =
            hadAmmo && ammoCount.GetAmmoCount() == 0;

        bool allColorsEmpty =
            redCount.GetRedPotionCount() == 0 &&
            blueCount.GetBluePotionCount() == 0 &&
            greenCount.GetGreenPotionCount() == 0 &&
            yellowCount.GetYellowPotionCount() == 0;

        surrenderButton.SetActive(ammoEmpty || allColorsEmpty);
    }

    public void Surrender()
    {
        if (defeatScreen != null)
        {
            defeatScreen.ShowDefeatScreen();
        }
    }
}