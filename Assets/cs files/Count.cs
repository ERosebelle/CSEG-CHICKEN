using UnityEngine;

public class Count : MonoBehaviour
{
    [Header("TENS DIGIT IMAGES")]
    public GameObject tens0;
    public GameObject tens1;
    public GameObject tens2;
    public GameObject tens3;
    public GameObject tens4;
    public GameObject tens5;
    public GameObject tens6;
    public GameObject tens7;
    public GameObject tens8;
    public GameObject tens9;

    [Header("ONES DIGIT IMAGES")]
    public GameObject ones0;
    public GameObject ones1;
    public GameObject ones2;
    public GameObject ones3;
    public GameObject ones4;
    public GameObject ones5;
    public GameObject ones6;
    public GameObject ones7;
    public GameObject ones8;
    public GameObject ones9;

    // ==========================================
    // AWAKE
    // ==========================================

    private void Awake()
    {
        HideAllNumbers();
    }

    // ==========================================
    // SET COUNT
    // ==========================================

    public void SetCount(int count)
    {
        // Always hide everything first
        HideAllNumbers();

        // ==========================================
        // ZERO
        // ==========================================

        if (count <= 0)
        {
            return;
        }

        // ==========================================
        // 1 - 9
        // ONLY ONES IS SHOWN
        // ==========================================

        if (count <= 9)
        {
            GameObject onesObject =
                GetOnesObject(count);

            if (onesObject == null)
            {
                return;
            }

            onesObject.SetActive(true);

            return;
        }

        // ==========================================
        // 10 - 99
        // BOTH TENS AND ONES ARE SHOWN
        // ==========================================

        if (count <= 99)
        {
            int tens = count / 10;
            int ones = count % 10;

            // ------------------------------------------
            // TENS
            // ------------------------------------------

            GameObject tensObject =
                GetTensObject(tens);

            if (tensObject != null)
            {
                tensObject.SetActive(true);
            }

            // ------------------------------------------
            // ONES
            // ------------------------------------------

            GameObject onesObject =
                GetOnesObject(ones);

            if (onesObject != null)
            {
                onesObject.SetActive(true);
            }

            return;
        }

        // ==========================================
        // ABOVE 99
        // ==========================================

        return;
    }

    // ==========================================
    // GET TENS OBJECT
    // ==========================================

    private GameObject GetTensObject(int digit)
    {
        switch (digit)
        {
            case 0:
                return tens0;

            case 1:
                return tens1;

            case 2:
                return tens2;

            case 3:
                return tens3;

            case 4:
                return tens4;

            case 5:
                return tens5;

            case 6:
                return tens6;

            case 7:
                return tens7;

            case 8:
                return tens8;

            case 9:
                return tens9;

            default:
                return null;
        }
    }

    // ==========================================
    // GET ONES OBJECT
    // ==========================================

    private GameObject GetOnesObject(int digit)
    {
        switch (digit)
        {
            case 0:
                return ones0;

            case 1:
                return ones1;

            case 2:
                return ones2;

            case 3:
                return ones3;

            case 4:
                return ones4;

            case 5:
                return ones5;

            case 6:
                return ones6;

            case 7:
                return ones7;

            case 8:
                return ones8;

            case 9:
                return ones9;

            default:
                return null;
        }
    }

    // ==========================================
    // HIDE EVERYTHING
    // ==========================================

    private void HideAllNumbers()
    {
        // ------------------------------------------
        // TENS
        // ------------------------------------------

        if (tens0 != null) tens0.SetActive(false);
        if (tens1 != null) tens1.SetActive(false);
        if (tens2 != null) tens2.SetActive(false);
        if (tens3 != null) tens3.SetActive(false);
        if (tens4 != null) tens4.SetActive(false);
        if (tens5 != null) tens5.SetActive(false);
        if (tens6 != null) tens6.SetActive(false);
        if (tens7 != null) tens7.SetActive(false);
        if (tens8 != null) tens8.SetActive(false);
        if (tens9 != null) tens9.SetActive(false);

        // ------------------------------------------
        // ONES
        // ------------------------------------------

        if (ones0 != null) ones0.SetActive(false);
        if (ones1 != null) ones1.SetActive(false);
        if (ones2 != null) ones2.SetActive(false);
        if (ones3 != null) ones3.SetActive(false);
        if (ones4 != null) ones4.SetActive(false);
        if (ones5 != null) ones5.SetActive(false);
        if (ones6 != null) ones6.SetActive(false);
        if (ones7 != null) ones7.SetActive(false);
        if (ones8 != null) ones8.SetActive(false);
        if (ones9 != null) ones9.SetActive(false);
    }
}