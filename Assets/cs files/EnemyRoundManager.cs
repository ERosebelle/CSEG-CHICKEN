using UnityEngine;

public class EnemyRoundManager : MonoBehaviour
{
    [Header("Round 1")]
    public EnemyCounter enemyCounter;

    [Header("Round 2")]
    public EnemyCounter2 enemyCounter2;

    private bool round2Activated;

    private void Start()
    {
        if (enemyCounter2 != null)
        {
            enemyCounter2.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (round2Activated)
            return;

        if (enemyCounter == null)
            return;

        if (enemyCounter.AreAllEnemiesDefeated())
        {
            ActivateRound2();
        }
    }

    private void ActivateRound2()
    {
        round2Activated = true;

        if (enemyCounter2 != null)
        {
            enemyCounter2.gameObject.SetActive(true);
        }
    }
}