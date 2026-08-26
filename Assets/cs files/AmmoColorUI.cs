using UnityEngine;
using UnityEngine.UI;

public class AmmoColorUI : MonoBehaviour
{
    [Header("Player Shoot")]
    public PlayerShoot playerShoot;

    [Header("Color UI")]
    public Image redImage;
    public Image blueImage;
    public Image greenImage;
    public Image yellowImage;

    void Update()
    {
        if (playerShoot == null)
            return;

        redImage.gameObject.SetActive(false);
        blueImage.gameObject.SetActive(false);
        greenImage.gameObject.SetActive(false);
        yellowImage.gameObject.SetActive(false);

        Color bulletColor = playerShoot.GetCurrentBulletColor();

        if (ApproximatelyColor(bulletColor, Color.red))
        {
            redImage.gameObject.SetActive(true);
        }
        else if (ApproximatelyColor(bulletColor, Color.blue))
        {
            blueImage.gameObject.SetActive(true);
        }
        else if (ApproximatelyColor(bulletColor, Color.green))
        {
            greenImage.gameObject.SetActive(true);
        }
        else if (ApproximatelyColor(bulletColor, Color.yellow))
        {
            yellowImage.gameObject.SetActive(true);
        }
    }

    bool ApproximatelyColor(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) < 0.05f &&
               Mathf.Abs(a.g - b.g) < 0.05f &&
               Mathf.Abs(a.b - b.b) < 0.05f;
    }
}