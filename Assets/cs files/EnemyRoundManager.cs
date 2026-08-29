using UnityEngine;

public class EnemyRoundManager : MonoBehaviour
{
    [Header("Round 1")]
    [Tooltip("Round 1 enemy counter. This detects when all 4 enemies are defeated.")]
    public EnemyCounter enemyCounter;

    [Header("Round 2")]
    [Tooltip("Round 2 enemy counter.")]
    public EnemyCounter2 enemyCounter2;

    [Header("Round 2 Map Entrance")]
    [Tooltip("Invisible wall blocking the entrance to the new map.")]
    public GameObject invisibleWall;

    [Tooltip("Mountain blocking the entrance to the new map.")]
    public GameObject mountain;

    [Header("Player")]
    [Tooltip("Player GameObject containing PlayerMusic.")]
    public PlayerMusic playerMusic;

    private bool round2Activated = false;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        round2Activated = false;

        // -----------------------------------------------------
        // ROUND 2 STARTS DISABLED
        // -----------------------------------------------------

        if (enemyCounter2 != null)
        {
            enemyCounter2.gameObject.SetActive(false);

            Debug.Log(
                "Round 2 EnemyCounter2 disabled at start."
            );
        }


        // -----------------------------------------------------
        // FIND PLAYER MUSIC AUTOMATICALLY
        // -----------------------------------------------------

        if (playerMusic == null)
        {
            playerMusic =
                FindFirstObjectByType<PlayerMusic>(
                    FindObjectsInactive.Include
                );
        }

        if (playerMusic != null)
        {
            Debug.Log(
                "EnemyRoundManager: PlayerMusic connected to " +
                playerMusic.name
            );
        }
        else
        {
            Debug.LogWarning(
                "EnemyRoundManager: PlayerMusic could NOT be found."
            );
        }
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // Round 2 already activated
        if (round2Activated)
            return;

        // No Round 1 counter
        if (enemyCounter == null)
            return;


        // -----------------------------------------------------
        // CHECK ROUND 1
        // -----------------------------------------------------

        if (enemyCounter.AreAllEnemiesDefeated())
        {
            ActivateRound2();
        }
    }


    // =========================================================
    // ACTIVATE ROUND 2
    // =========================================================

    private void ActivateRound2()
    {
        if (round2Activated)
            return;

        round2Activated = true;


        Debug.Log("================================");
        Debug.Log("ROUND 1 COMPLETE");
        Debug.Log("ACTIVATING ROUND 2");
        Debug.Log("OPENING NEW MAP ENTRANCE");
        Debug.Log("================================");


        // =====================================================
        // OPEN MAP ENTRANCE
        // =====================================================

        // -----------------------------------------------------
        // DISABLE INVISIBLE WALL
        // -----------------------------------------------------

        if (invisibleWall != null)
        {
            invisibleWall.SetActive(false);

            Debug.Log(
                "Round 2: Invisible Wall DISABLED."
            );
        }
        else
        {
            Debug.LogWarning(
                "Round 2: Invisible Wall is NOT assigned."
            );
        }


        // -----------------------------------------------------
        // DISABLE MOUNTAIN
        // -----------------------------------------------------

        if (mountain != null)
        {
            mountain.SetActive(false);

            Debug.Log(
                "Round 2: Mountain DISABLED."
            );
        }
        else
        {
            Debug.LogWarning(
                "Round 2: Mountain is NOT assigned."
            );
        }


        // =====================================================
        // ACTIVATE ROUND 2 ENEMIES
        // =====================================================

        if (enemyCounter2 != null)
        {
            enemyCounter2.gameObject.SetActive(true);

            Debug.Log(
                "Round 2 EnemyCounter2 ACTIVATED."
            );
        }
        else
        {
            Debug.LogWarning(
                "Round 2: EnemyCounter2 is NOT assigned."
            );
        }


        // =====================================================
        // CHANGE PLAYER MUSIC
        // =====================================================

        if (playerMusic != null)
        {
            Debug.Log(
                "Round 2: Telling PlayerMusic to change music."
            );

            playerMusic.ChangeToRound2Music();
        }
        else
        {
            Debug.LogWarning(
                "Round 2: PlayerMusic is NOT connected."
            );
        }


        // =====================================================
        // COMPLETE
        // =====================================================

        Debug.Log("================================");
        Debug.Log("ROUND 2 ACTIVATED");
        Debug.Log("MAP ENTRANCE OPEN");
        Debug.Log("ROUND 2 MUSIC ACTIVATED");
        Debug.Log("================================");
    }


    // =========================================================
    // CHECK ROUND 2
    // =========================================================

    public bool IsRound2Activated()
    {
        return round2Activated;
    }
}