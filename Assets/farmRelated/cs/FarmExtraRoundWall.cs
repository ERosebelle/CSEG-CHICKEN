using UnityEngine;
using System.Collections;

public class FarmExtraRoundWall : MonoBehaviour
{
    [Header("Player Detection")]
    public GameObject playerObject;

    [Header("Extra Round Manager")]
    public FarmExtraRoundManager extraRoundManager;

    [Header("Closing Delay")]
    public float closeDelay = 3f;

    private bool playerDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(
            "FarmExtraRoundWall: Something entered trigger: "
            + other.gameObject.name
        );

        if (playerObject == null)
        {
            Debug.LogError(
                "FarmExtraRoundWall: Player Object is NOT assigned!"
            );

            return;
        }

        if (other.gameObject != playerObject &&
            !other.transform.IsChildOf(playerObject.transform))
        {
            Debug.Log(
                "FarmExtraRoundWall: Object is NOT the assigned Player."
            );

            return;
        }

        if (playerDetected)
            return;

        playerDetected = true;

        Debug.Log(
            "FarmExtraRoundWall: PLAYER DETECTED!"
        );

        if (extraRoundManager == null)
        {
            Debug.LogError(
                "FarmExtraRoundWall: FarmExtraRoundManager is NOT assigned!"
            );

            return;
        }

        Debug.Log(
            "FarmExtraRoundWall: Player detected. Closing wall in "
            + closeDelay + " seconds."
        );

        StartCoroutine(StartExtraRoundAfterDelay());
    }

    private IEnumerator StartExtraRoundAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);

        Debug.Log(
            "FarmExtraRoundWall: 3 seconds passed. Starting Extra Round!"
        );

        extraRoundManager.StartExtraRound();
    }
}