using System;
using UnityEngine;

public static class EventManager
{
    // OBS! DENNA KLASS BEHÖVS INTE LÄGGAS PÅ ETT SPELOBJEKT I SCENEN
    public static event Action OnCompassActivated;

    public static void TriggerCompassEvent()
    {
        OnCompassActivated?.Invoke();
    }
}
