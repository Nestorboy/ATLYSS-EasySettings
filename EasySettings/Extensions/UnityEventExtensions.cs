using UnityEngine.Events;

namespace Nessie.ATLYSS.EasySettings.Extensions;

public static class UnityEventExtensions
{
    public static void SetAllPersistentListenerStates(this UnityEventBase unityEvent, UnityEventCallState state)
    {
        for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
        {
            unityEvent.SetPersistentListenerState(i, state);
        }
    }

    public static void DisablePersistentListeners(this UnityEventBase unityEvent) => unityEvent.SetAllPersistentListenerStates(UnityEventCallState.Off);

    /// <summary>
    /// Removes runtime (non-persistent) events and disables editor (persistent) events.
    /// </summary>
    /// <param name="unityEvent"></param>
    public static void RemoveAndDisableAllListeners(this UnityEventBase unityEvent)
    {
        unityEvent.RemoveAllListeners();
        unityEvent.DisablePersistentListeners();
    }
}