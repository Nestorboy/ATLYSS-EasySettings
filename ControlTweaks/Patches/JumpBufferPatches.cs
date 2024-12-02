using HarmonyLib;
using Mirror;
using UnityEngine;

namespace Nessie.ATLYSS.ControlTweaks.Patches;

public static class JumpBufferPatches
{
    [HarmonyPatch(typeof(PlayerMove), nameof(PlayerMove.Handle_JumpControl))]
    private static class JumpBufferPatch
    {
        private static bool _inputJumpDown;
        private static bool _canJump;
        private static BufferedInput<bool> _inputJump;

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPrefix]
        private static void RecordInputPressed(PlayerMove __instance) // ReSharper restore InconsistentNaming
        {
            _inputJumpDown = Input.GetKeyDown(InputControlManager.current._jump);
            _canJump = CanJumpInput(__instance);
            if (_inputJumpDown)
            {
                _inputJump.UpdateState(true);
            }
            else if (Input.GetKeyUp(InputControlManager.current._jump))
            {
                _inputJump.UpdateState(false);
            }
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPostfix]
        private static void HandleBufferedJump(PlayerMove __instance) // ReSharper restore InconsistentNaming
        {
            if (!_canJump)
                return;

            if (!NetworkClient.active || __instance._freezeMovementBuffer > 0f)
                return;

            // Regular jump was performed.
            if (_inputJumpDown)
            {
                _inputJump.Consume();
                return;
            }

            if (_inputJump.PressedWithinUnscaled(ControlTweaksPlugin.JumpBufferDuration))
            {
                InputJump(__instance);
                _inputJump.Consume();
            }
        }

        private static bool CanJumpInput(PlayerMove instance)
        {
            return instance._player._currentPlayerCondition == PlayerCondition.ACTIVE &&
                   !instance._player._inChat &&
                   instance._movementLockType == MovementLockType.NONE &&
                   instance._lockControlBuffer <= 0f &&
                   !instance.IsSlidingOnSlope &&
                   (instance._pWater._isInWater || instance._currentJumps < instance._maxJumps) &&
                   instance._attackMovementSpeedModifier > -80f &&
                   !instance._lockControlMidair &&
                   !instance._lockControl &&
                   !instance._jumpToLandBuffer &&
                   !instance.initJumpPress;
        }

        private static void InputJump(PlayerMove instance)
        {
            float force = instance._standardJumpForce;
            if (instance._pWater._isInWater && !instance._pWater._canSwimUp && instance._currentMovementAction == MovementAction.JUMP)
                return;

            if (instance._pWater._isInWater && instance._airTime > 1.5f && (instance._currentMovementAction == MovementAction.JUMP || instance._currentMovementAction == MovementAction.FALL))
            {
                force = instance._underwaterJumpForce;
            }

            instance.Init_Jump(force, instance._worldSpaceInput == Vector3.zero ? 0f : 1.65f, 0f);

            instance._isJumpInput = true;
            instance._pCombat._airAttackLock = 0f;
        }
    }

    [HarmonyPatch(typeof(PlayerClimbing), nameof(PlayerClimbing.Handle_LedgeClimbControl))]
    private static class LedgeJumpBufferPatch
    {
        private static BufferedInput<bool> _inputJump;

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPrefix]
        private static bool RecordInputPressed(PlayerClimbing __instance) // ReSharper restore InconsistentNaming
        {
            if (Input.GetKeyDown(InputControlManager.current._jump))
            {
                _inputJump.UpdateState(true);
            }
            else if (Input.GetKeyUp(InputControlManager.current._jump))
            {
                _inputJump.UpdateState(false);
            }

            if (!NetworkClient.active || __instance._player._currentPlayerAction != PlayerAction.IDLE || __instance._climbAttachBuffer > 0.0 || __instance._ropeParent || __instance._pCombat._isAirAttacked || __instance._pCombat._localAirAttackInput || __instance._pCombat._isChargingWeapon || __instance._pCombat._isBlocking || __instance._pCombat._localBlockingInput || __instance._pMove._lockControlMidair || __instance._pMove._lockControlBuffer > 0.0)
                return true;

            // Only replace local function.
            if (__instance._onLedge)
            {
                HandleOnLedge(__instance);
                return false;
            }

            return true;
        }

        private static void HandleOnLedge(PlayerClimbing instance)
        {
            if (!instance._parentGroundTypeObject)
            {
                instance._onLedge = false;
                instance._isClimbing = false;
                instance._climbAttachBuffer = instance._climbInitBuffer;
            }
            else
            {
                instance._pCombat._blockControlBuffer = 0.0f;
                instance._ledgeGrabBuffer = 0.12f;
                instance.transform.rotation = Quaternion.LookRotation(((instance._hit.point - instance.transform.position) with
                {
                    y = 0.0f
                }).normalized);

                if (!instance._parentGroundTypeObject._canLedgeClimb)
                    instance.Init_JumpOffLedgeGrab(20f, -25.5f, 45.5f);

                if (!_inputJump.PressedWithinUnscaled(ControlTweaksPlugin.JumpBufferDuration))
                    return;

                float forwardForce = instance._parentGroundTypeObject._noForwardForceOnLedgeJump ? instance._ledgeJumpUpForce : 0f;
                instance.Init_JumpOffLedgeGrab(instance._ledgeJumpUpForce, forwardForce, 65f);

                _inputJump.Consume();
            }
        }
    }
}