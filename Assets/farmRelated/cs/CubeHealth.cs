using UnityEngine;

public class CubeHealth : MonoBehaviour
{
    [Header("Cube Health")]
    public int maxHealth = 10;

    private int currentHealth;
    private bool isDefeated;

    private CubeSpawner cubeSpawner;

    [Header("Heart Models")]
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;

    [Header("Color Potion Containers")]
    public GameObject redPotionContainer;
    public GameObject bluePotionContainer;
    public GameObject greenPotionContainer;
    public GameObject yellowPotionContainer;

    [Header("Ammo Potion")]
    public GameObject ammoPotionContainer;

    [Header("Potion Spawn")]
    public float potionSpawnRadius = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        isDefeated = false;

        UpdateHearts();
    }

    public void SetSpawner(
        CubeSpawner spawner
    )
    {
        cubeSpawner = spawner;
    }

    public void TakeDamage()
    {
        if (isDefeated)
            return;

        currentHealth--;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0,
                maxHealth
            );

        UpdateHearts();

        SpawnRandomColorPotion();

        if (currentHealth <= 0)
        {
            Defeat();
        }
    }

    void UpdateHearts()
    {
        if (heart1 != null)
            heart1.SetActive(currentHealth >= 1);

        if (heart2 != null)
            heart2.SetActive(currentHealth >= 3);

        if (heart3 != null)
            heart3.SetActive(currentHealth >= 5);

        if (heart4 != null)
            heart4.SetActive(currentHealth >= 7);

        if (heart5 != null)
            heart5.SetActive(currentHealth >= 9);
    }

    Vector3 GetPotionSpawnPosition()
    {
        Vector2 randomCircle =
            Random.insideUnitCircle *
            potionSpawnRadius;

        return new Vector3(
            transform.position.x + randomCircle.x,
            transform.position.y,
            transform.position.z + randomCircle.y
        );
    }

    void SpawnRandomColorPotion()
    {
        GameObject selectedPotion = null;

        int randomColor =
            Random.Range(
                0,
                4
            );

        switch (randomColor)
        {
            case 0:
                selectedPotion =
                    redPotionContainer;
                break;

            case 1:
                selectedPotion =
                    bluePotionContainer;
                break;

            case 2:
                selectedPotion =
                    greenPotionContainer;
                break;

            case 3:
                selectedPotion =
                    yellowPotionContainer;
                break;
        }

        if (selectedPotion == null)
            return;

        GameObject spawnedPotion =
            Instantiate(
                selectedPotion,
                GetPotionSpawnPosition(),
                Quaternion.identity
            );

        spawnedPotion.SetActive(true);
    }

    void Defeat()
    {
        if (isDefeated)
            return;

        isDefeated = true;

        SpawnAmmoPotion();

        if (cubeSpawner != null)
        {
            cubeSpawner.CubeDestroyed(
                transform.root.gameObject
            );
        }
        else
        {
            Debug.LogError(
                "CubeHealth: CubeSpawner reference is missing!"
            );

            Destroy(
                transform.root.gameObject
            );
        }
    }

    void SpawnAmmoPotion()
    {
        if (ammoPotionContainer == null)
            return;

        GameObject spawnedAmmo =
            Instantiate(
                ammoPotionContainer,
                GetPotionSpawnPosition(),
                Quaternion.identity
            );

        spawnedAmmo.SetActive(true);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}