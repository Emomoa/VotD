using UnityEngine;

public class Counter : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private string currentGroundTag;
    private int switchCount = 0;

    void Update()
    {
        currentGroundTag = playerMovement.GetGroundTag();
    }

    public int GetSwitchCount()
    {
        Debug.Log(switchCount);
        return switchCount;
    }
}