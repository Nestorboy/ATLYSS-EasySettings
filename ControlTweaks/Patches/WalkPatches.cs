using HarmonyLib;
using UnityEngine;

namespace Nessie.ATLYSS.ControlTweaks.Patches;

public static class WalkPatches
{
    [HarmonyPatch(typeof(PlayerMove), "<Handle_MovementControl>g__Apply_MovementParams|87_2")]
    private static class WalkPatch
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPrefix]
        private static void ApplyWalkModifier(PlayerMove __instance, out Vector3 __state) // ReSharper restore InconsistentNaming
        {
            __state = __instance._worldSpaceInput;
            if (Input.GetKey(ControlTweaksPlugin.WalkKey))
                __instance._worldSpaceInput *= ControlTweaksPlugin.WalkSpeed;
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPostfix]
        private static void RevertWalkModifier(PlayerMove __instance, Vector3 __state) // ReSharper restore InconsistentNaming
        {
            __instance._worldSpaceInput = __state;
        }
    }
}