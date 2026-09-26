using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [Header("Ammo")]
    public GameObject ammoPrefab;

    [Header("Walls")]
    public Collider wall1;
    public Collider wall2;
    public Collider wall3;
    public Collider wall4;

    [Header("Spawn Settings")]
    [Tooltip("How many seconds before ammo spawns.")]
    public float spawnInterval = 30f;

    [Tooltip("Height where the ammo object will spawn.")]
    public float groundHeight = 5f;

    [Tooltip("Distance kept away from the walls.")]
    public float wallPadding = 1f;

    private GameObject currentAmmo;

    private GameObject extraAmmo1;
    private GameObject extraAmmo2;

    private bool isExtraRound = false;

    private void Start()
    {
        Invoke(
            nameof(SpawnAmmo),
            spawnInterval
        );
    }

    private void SpawnAmmo()
    {
        if (isExtraRound)
        {
            SpawnExtraAmmo();

            return;
        }

        // ROUND 1
        if (currentAmmo != null)
            return;

        GameObject spawnedAmmo =
            CreateAmmo();

        if (spawnedAmmo != null)
        {
            currentAmmo = spawnedAmmo;
        }
    }

    private GameObject CreateAmmo()
    {
        if (ammoPrefab == null)
        {
            Debug.LogError(
                "AmmoSpawner: Ammo Prefab is NOT assigned!"
            );

            return null;
        }

        if (wall1 == null ||
            wall2 == null ||
            wall3 == null ||
            wall4 == null)
        {
            Debug.LogError(
                "AmmoSpawner: One or more walls are NOT assigned!"
            );

            return null;
        }

        float minX = wall1.bounds.min.x;
        float maxX = wall1.bounds.max.x;

        float minZ = wall1.bounds.min.z;
        float maxZ = wall1.bounds.max.z;

        Collider[] selectedWalls =
        {
            wall1,
            wall2,
            wall3,
            wall4
        };

        foreach (Collider wall in selectedWalls)
        {
            minX = Mathf.Min(
                minX,
                wall.bounds.min.x
            );

            maxX = Mathf.Max(
                maxX,
                wall.bounds.max.x
            );

            minZ = Mathf.Min(
                minZ,
                wall.bounds.min.z
            );

            maxZ = Mathf.Max(
                maxZ,
                wall.bounds.max.z
            );
        }

        minX += wallPadding;
        maxX -= wallPadding;

        minZ += wallPadding;
        maxZ -= wallPadding;

        if (minX >= maxX ||
            minZ >= maxZ)
        {
            Debug.LogError(
                "AmmoSpawner: Invalid spawn area."
            );

            return null;
        }

        float randomX =
            Random.Range(
                minX,
                maxX
            );

        float randomZ =
            Random.Range(
                minZ,
                maxZ
            );

        Vector3 spawnPosition =
            new Vector3(
                randomX,
                groundHeight,
                randomZ
            );

        GameObject spawnedAmmo =
            Instantiate(
                ammoPrefab,
                spawnPosition,
                Quaternion.identity
            );

        spawnedAmmo.SetActive(true);

        Debug.Log(
            "AmmoSpawner: Ammo spawned at "
            + spawnPosition
        );

        return spawnedAmmo;
    }

    // ==========================================
    // EXTRA ROUND WALLS
    // ==========================================

    public void SetExtraRoundWalls(
        Collider extraWall1,
        Collider extraWall2,
        Collider extraWall3,
        Collider extraWall4
    )
    {
        wall1 = extraWall1;
        wall2 = extraWall2;
        wall3 = extraWall3;
        wall4 = extraWall4;

        Debug.Log(
            "AmmoSpawner: Extra Round walls assigned."
        );
    }

    // ==========================================
    // EXTRA ROUND SETUP
    // ==========================================

    public void StartExtraRound(
        float extraRoundSpawnInterval
    )
    {
        CancelInvoke(
            nameof(SpawnAmmo)
        );

        spawnInterval =
            extraRoundSpawnInterval;

        isExtraRound = true;

        Debug.Log(
            "AmmoSpawner: Extra Round settings applied."
        );

        Debug.Log(
            "AmmoSpawner: Spawn Interval = "
            + spawnInterval
        );

        // Spawn TWO ammo potions immediately
        SpawnExtraAmmo();

        InvokeRepeating(
            nameof(SpawnExtraAmmo),
            spawnInterval,
            spawnInterval
        );
    }

    // ==========================================
    // EXTRA ROUND AMMO
    // Maximum of 2 at the same time
    // ==========================================

    private void SpawnExtraAmmo()
    {
        // Both slots are occupied
        if (extraAmmo1 != null &&
            extraAmmo2 != null)
        {
            return;
        }

        // Fill first empty slot
        if (extraAmmo1 == null)
        {
            extraAmmo1 =
                CreateAmmo();

            return;
        }

        // Fill second empty slot
        if (extraAmmo2 == null)
        {
            extraAmmo2 =
                CreateAmmo();

            return;
        }
    }

    // ==========================================
    // AMMO PICKUP
    // ==========================================

    public void AmmoPickedUp()
    {
        if (isExtraRound)
        {
            return;
        }

        if (currentAmmo == null)
            return;

        Destroy(
            currentAmmo
        );

        currentAmmo = null;

        Invoke(
            nameof(SpawnAmmo),
            spawnInterval
        );
    }

    // ==========================================
    // EXTRA ROUND AMMO PICKUP
    // ==========================================

    public void ExtraAmmoPickedUp(
        GameObject pickedAmmo
    )
    {
        if (!isExtraRound)
            return;

        if (pickedAmmo == extraAmmo1)
        {
            Destroy(
                extraAmmo1
            );

            extraAmmo1 = null;
        }
        else if (pickedAmmo == extraAmmo2)
        {
            Destroy(
                extraAmmo2
            );

            extraAmmo2 = null;
        }

        // Immediately replace the picked-up ammo
        SpawnExtraAmmo();
    }
}