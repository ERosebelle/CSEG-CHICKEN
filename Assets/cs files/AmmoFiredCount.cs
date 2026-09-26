using UnityEngine;

public class AmmoFiredCount : MonoBehaviour
{
    private int ammoFired = 0;

    public void AddShot()
    {
        ammoFired++;

      
    }

    public int GetAmmoFired()
    {
        return ammoFired;
    }
}