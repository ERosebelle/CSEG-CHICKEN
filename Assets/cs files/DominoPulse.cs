using UnityEngine;

public class DominoPulse : MonoBehaviour
{
    [Header("Models")]
    public Transform[] models = new Transform[7];

    [Header("Pulse Settings")]
    public float pulseScale = 1.2f;
    public float pulseDuration = 0.2f;
    public float delayBetweenModels = 0.1f;

    private Vector3[] originalScales;

    void Start()
    {
        originalScales = new Vector3[models.Length];

        for (int i = 0; i < models.Length; i++)
        {
            if (models[i] != null)
            {
                originalScales[i] =
                    models[i].localScale;
            }
        }

        StartCoroutine(PulseSequence());
    }

    System.Collections.IEnumerator PulseSequence()
    {
        while (true)
        {
            for (int i = 0; i < models.Length; i++)
            {
                if (models[i] != null)
                {
                    yield return StartCoroutine(
                        PulseModel(i)
                    );
                }

                yield return new WaitForSeconds(
                    delayBetweenModels
                );
            }
        }
    }

    System.Collections.IEnumerator PulseModel(int index)
    {
        Transform model = models[index];

        Vector3 originalScale =
            originalScales[index];

        Vector3 targetScale =
            originalScale * pulseScale;

        float halfDuration =
            pulseDuration / 2f;

        float timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;

            float t =
                timer / halfDuration;

            model.localScale =
                Vector3.Lerp(
                    originalScale,
                    targetScale,
                    t
                );

            yield return null;
        }

        timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;

            float t =
                timer / halfDuration;

            model.localScale =
                Vector3.Lerp(
                    targetScale,
                    originalScale,
                    t
                );

            yield return null;
        }

        model.localScale =
            originalScale;
    }
}