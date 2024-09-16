using UnityEngine;

public class WeakPlank : MonoBehaviour
{
    private bool isBroken = false;

    public void BreakPlank()
    {
        if (!isBroken)
        {
            isBroken = true;
            // Spela ljud och animation för bruten planka
            Debug.Log("Plankan har gått sönder!");
            // Eventuellt inaktivera plankan eller ändra dess utseende
            gameObject.SetActive(false);
        }
    }
}
