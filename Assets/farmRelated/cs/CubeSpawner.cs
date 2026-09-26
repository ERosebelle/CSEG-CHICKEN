using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeSpawner : MonoBehaviour
{
    [Header("Cube Prefab")]
    public GameObject cubePrefab;

    [Header("Round 1 - Farm Walls")]
    public Collider wall1;
    public Collider wall2;
    public Collider wall3;
    public Collider wall4;

    [Header("Round 2 - Extra Round Walls")]
    public Collider extraWall1;
    public Collider extraWall2;
    public Collider extraWall3;
    public Collider extraWall4;

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;

    [Tooltip("Height where the cube first appears.")]
    public float spawnHeight = 70f;

    [Tooltip("Distance kept inside the walls.")]
    public float wallPadding = 3f;

    [Header("Round 1 Goal")]
    public int requiredCubes = 10;

    [Header("Goal UI")]
    public TMP_Text goalText;

    [Header("Round Manager")]
    public FarmExtraRoundManager extraRoundManager;

    private int cubesDefeated = 0;
    private int cubesSpawned = 0;

    private List<GameObject> activeCubes =
        new List<GameObject>();

    private bool levelComplete = false;
    private bool round2Started = false;

    private void Start()
    {
        if (extraRoundManager == null)
        {
            extraRoundManager =
                FindFirstObjectByType<FarmExtraRoundManager>();
        }

        if (cubePrefab == null)
        {
            Debug.LogError(
                "CubeSpawner: Cube Prefab is NOT assigned!"
            );

            return;
        }

        if (!AreWallsAssigned(
            wall1,
            wall2,
            wall3,
            wall4))
        {
            Debug.LogError(
                "CubeSpawner: Round 1 walls are NOT fully assigned!"
            );

            return;
        }

        round2Started = false;

        ResetRound();

        UpdateGoalUI();

        SpawnCube();

        InvokeRepeating(
            nameof(SpawnCube),
            spawnInterval,
            spawnInterval
        );
    }

    private void ResetRound()
    {
        cubesDefeated = 0;
        cubesSpawned = 0;

        activeCubes.Clear();

        levelComplete = false;
    }

    private bool AreWallsAssigned(
        Collider a,
        Collider b,
        Collider c,
        Collider d
    )
    {
        return a != null &&
               b != null &&
               c != null &&
               d != null;
    }

    private bool IsRound2()
    {
        return round2Started;
    }

    private void UpdateGoalUI()
    {
        if (goalText != null)
        {
            goalText.text =
                cubesDefeated +
                "/" +
                requiredCubes;
        }
    }

    private void SpawnCube()
    {
        if (levelComplete)
            return;

        if (cubesSpawned >= requiredCubes)
        {
            CancelInvoke(
                nameof(SpawnCube)
            );

            return;
        }

        SpawnCubeAtRandomPosition();
    }

    public void SpawnExtraCube()
    {
        if (levelComplete)
            return;

        requiredCubes++;

        UpdateGoalUI();

        SpawnCubeAtRandomPosition();

        CancelInvoke(
            nameof(SpawnCube)
        );

        if (cubesSpawned < requiredCubes)
        {
            InvokeRepeating(
                nameof(SpawnCube),
                spawnInterval,
                spawnInterval
            );
        }
    }

    public void StartExtraRound(
        int extraRoundRequiredCubes,
        float extraRoundSpawnInterval
    )
    {
        if (extraRoundManager == null)
        {
            Debug.LogError(
                "CubeSpawner: Round Manager is missing!"
            );

            return;
        }

        if (!extraRoundManager.IsExtraRoundStarted())
        {
            return;
        }

        if (!AreWallsAssigned(
            extraWall1,
            extraWall2,
            extraWall3,
            extraWall4))
        {
            Debug.LogError(
                "CubeSpawner: Round 2 walls are NOT fully assigned!"
            );

            return;
        }

        CancelInvoke(
            nameof(SpawnCube)
        );

        round2Started = true;

        foreach (GameObject cube in activeCubes)
        {
            if (cube != null)
            {
                Destroy(cube);
            }
        }

        activeCubes.Clear();

        cubesDefeated = 0;
        cubesSpawned = 0;

        requiredCubes =
            extraRoundRequiredCubes;

        spawnInterval =
            extraRoundSpawnInterval;

        levelComplete = false;

        UpdateGoalUI();

        SpawnCube();

        if (!levelComplete &&
            cubesSpawned < requiredCubes)
        {
            InvokeRepeating(
                nameof(SpawnCube),
                spawnInterval,
                spawnInterval
            );
        }
    }

    private bool GetSpawnArea(
        out float minX,
        out float maxX,
        out float minZ,
        out float maxZ
    )
    {
        minX = float.MaxValue;
        maxX = float.MinValue;

        minZ = float.MaxValue;
        maxZ = float.MinValue;

        Collider[] walls;

        if (IsRound2())
        {
            walls = new Collider[]
            {
                extraWall1,
                extraWall2,
                extraWall3,
                extraWall4
            };
        }
        else
        {
            walls = new Collider[]
            {
                wall1,
                wall2,
                wall3,
                wall4
            };
        }

        foreach (Collider wall in walls)
        {
            if (wall == null)
                return false;

            Bounds bounds =
                wall.bounds;

            minX =
                Mathf.Min(
                    minX,
                    bounds.min.x
                );

            maxX =
                Mathf.Max(
                    maxX,
                    bounds.max.x
                );

            minZ =
                Mathf.Min(
                    minZ,
                    bounds.min.z
                );

            maxZ =
                Mathf.Max(
                    maxZ,
                    bounds.max.z
                );
        }

        minX += wallPadding;
        maxX -= wallPadding;

        minZ += wallPadding;
        maxZ -= wallPadding;

        if (minX >= maxX)
            return false;

        if (minZ >= maxZ)
            return false;

        return true;
    }

    private void SpawnCubeAtRandomPosition()
    {
        if (cubePrefab == null)
            return;

        float minX;
        float maxX;
        float minZ;
        float maxZ;

        if (!GetSpawnArea(
            out minX,
            out maxX,
            out minZ,
            out maxZ))
        {
            return;
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
                spawnHeight,
                randomZ
            );

        GameObject newCube =
            Instantiate(
                cubePrefab,
                spawnPosition,
                Quaternion.identity
            );

        newCube.SetActive(true);

        newCube.name =
            "Cube Con(Clone)";

        cubesSpawned++;

        activeCubes.Add(
            newCube
        );

        // IMPORTANT:
        // Give this cube its exact spawner.
        CubeHealth cubeHealth =
            newCube.GetComponentInChildren<CubeHealth>();

        if (cubeHealth != null)
        {
            cubeHealth.SetSpawner(this);
        }

        if (cubesSpawned >= requiredCubes)
        {
            CancelInvoke(
                nameof(SpawnCube)
            );
        }
    }

    public void CubeDestroyed(
        GameObject defeatedCube
    )
    {
        if (defeatedCube == null)
            return;

        if (levelComplete)
            return;

        GameObject matchedCube = null;

        for (int i = 0; i < activeCubes.Count; i++)
        {
            GameObject cube =
                activeCubes[i];

            if (cube == null)
                continue;

            if (cube == defeatedCube ||
                defeatedCube.transform.IsChildOf(
                    cube.transform
                ) ||
                cube.transform.IsChildOf(
                    defeatedCube.transform
                ))
            {
                matchedCube = cube;
                break;
            }
        }

        if (matchedCube == null)
        {
            Debug.LogWarning(
                "CubeSpawner: Defeated cube was NOT found."
            );

            return;
        }

        activeCubes.Remove(
            matchedCube
        );

        cubesDefeated++;

        Debug.Log(
            "CubeSpawner: Cube defeated. "
            + cubesDefeated
            + "/"
            + requiredCubes
        );

        UpdateGoalUI();

        Destroy(
            matchedCube
        );

        if (cubesDefeated >= requiredCubes)
        {
            if (IsRound2())
            {
                CompleteRound2();
            }
            else
            {
                CompleteRound1();
            }
        }
    }

    private void CompleteRound1()
    {
        if (levelComplete)
            return;

        levelComplete = true;

        CancelInvoke(
            nameof(SpawnCube)
        );

        if (extraRoundManager != null)
        {
            extraRoundManager.NotifyRoundCompleted(true);
        }
    }

    private void CompleteRound2()
    {
        if (levelComplete)
            return;

        levelComplete = true;

        CancelInvoke(
            nameof(SpawnCube)
        );

        if (extraRoundManager != null)
        {
            extraRoundManager.NotifyRoundCompleted(true);
        }
    }

    public int GetRequiredCubes()
    {
        return requiredCubes;
    }

    public int GetCubesDefeated()
    {
        return cubesDefeated;
    }

    public int GetActiveCubeCount()
    {
        return activeCubes.Count;
    }

    public int GetCubesSpawned()
    {
        return cubesSpawned;
    }

    private void OnDrawGizmos()
    {
        float minX;
        float maxX;
        float minZ;
        float maxZ;

        if (!GetSpawnArea(
            out minX,
            out maxX,
            out minZ,
            out maxZ))
        {
            return;
        }

        Vector3 center =
            new Vector3(
                (minX + maxX) / 2f,
                spawnHeight,
                (minZ + maxZ) / 2f
            );

        Vector3 size =
            new Vector3(
                maxX - minX,
                1f,
                maxZ - minZ
            );

        Gizmos.DrawWireCube(
            center,
            size
        );
    }
}