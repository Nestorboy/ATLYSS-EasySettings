using System;
using HarmonyLib;
using Nessie.ATLYSS.EasySettings.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings;

public static class SettingsPatches
{
    internal static UnityEvent OnSettingsMenuInitialized { get; } = new();

    private static SettingsTab ModTab => Settings.ModTab;

    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.Start))]
    private static class MenuInitialization
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPostfix]
        private static void InitializeModTab(SettingsManager __instance) // ReSharper restore InconsistentNaming
        {
            Transform tabsContainer = Utility.FindTabsContainer(__instance);
            if (!tabsContainer)
            {
                EasySettingsPlugin.Logger.LogError("Unable to find settings tabs container GameObject. EasySettings won't be available.");
                return;
            }

            HorizontalLayoutGroup horizontalGroup = tabsContainer.GetComponent<HorizontalLayoutGroup>();
            if (!horizontalGroup)
            {
                EasySettingsPlugin.Logger.LogError("Settings tabs container GameObject does not have a HorizontalLayoutGroup. EasySettings won't be available.");
                return;
            }
            horizontalGroup.spacing = 8;
            horizontalGroup.padding.right = horizontalGroup.padding.left;
            horizontalGroup.childControlWidth = true;

            Button cancelButton = Utility.FindCancelButton(__instance);
            if (cancelButton)
            {
                cancelButton.onClick.AddListener(() => Settings.OnCancelSettings.Invoke());
            }

            Button applyButton = Utility.FindApplyButton(__instance);
            if (applyButton)
            {
                applyButton.onClick.AddListener(() => Settings.OnApplySettings.Invoke());
            }
            else
            {
                EasySettingsPlugin.Logger.LogWarning("Unable to find Apply Button. Saving the settings will not be possible during this play session.");
            }

            TemplateManager.Initialize();

            OnSettingsMenuInitialized.Invoke();
        }
    }

    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.Set_SettingMenuSelectionIndex))]
    private static class MenuSelection
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPostfix]
        private static void ApplySelection(SettingsManager __instance) // ReSharper restore InconsistentNaming
        {
            if (ModTab.Element)
            {
                ModTab.Element.isEnabled = (int)__instance._currentSettingsMenuSelection == Settings.ModTabIndex;
            }

            if (ModTab.Content)
            {
                ModTab.Content.anchoredPosition = Vector2.zero;
            }
        }
    }

    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.Handle_InputParameters))]
    private static class ListenForKeyChanges
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPrefix]
        private static void RecordKeys(SettingsManager __instance, out string[] __state) // ReSharper restore InconsistentNaming
        {
            if (!__instance._waitingForKey)
            {
                __state = null;
                return;
            }

            EasySettingsPlugin.Logger.LogInfo("Handle_InputParameters RecordKeys");

            KeyBindButton[] keyButtons = __instance.keyBindButtons;
            string[] keys = new string[keyButtons.Length];
            for (int i = 0; i < keyButtons.Length; i++)
            {
                keys[i] = keyButtons[i]?._keyBind;
            }

            __state = keys;
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPostfix]
        private static void CompareKeys(SettingsManager __instance, string[] __state) // ReSharper restore InconsistentNaming
        {
            string[] keys = __state;
            if (keys == null) return;

            KeyBindButton[] keyButtons = __instance.keyBindButtons;
            for (int i = 0; i < keyButtons.Length; i++)
            {
                string oldKey = keys[i];
                string newKey = keyButtons[i]._keyBind;
                if (oldKey == newKey) continue;

                EasySettingsPlugin.Logger.LogInfo($"Key {i} changed from {oldKey} to {newKey}");

                bool isModButton = SettingsTab.KeyButtonIndexToKeyButton.ContainsKey(i);
                if (isModButton)
                {
                    SettingsTab.KeyButtonIndexToKeyButton[i].SetValue(Enum.Parse<KeyCode>(newKey));
                }
            }
        }
    }

    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.OnGUI))]
    private static class ListenForKeyChanges2
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPrefix]
        private static void RecordKeys(SettingsManager __instance, out string[] __state) // ReSharper restore InconsistentNaming
        {
            if (!__instance._waitingForKey)
            {
                __state = null;
                return;
            }

            EasySettingsPlugin.Logger.LogInfo("OnGUI RecordKeys");

            KeyBindButton[] keyButtons = __instance.keyBindButtons;
            string[] keys = new string[keyButtons.Length];
            for (int i = 0; i < keyButtons.Length; i++)
            {
                keys[i] = keyButtons[i]?._keyBind;
            }

            __state = keys;
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable InconsistentNaming
        [HarmonyPostfix]
        private static void CompareKeys(SettingsManager __instance, string[] __state) // ReSharper restore InconsistentNaming
        {
            string[] keys = __state;
            if (keys == null) return;

            KeyBindButton[] keyButtons = __instance.keyBindButtons;
            for (int i = 0; i < keyButtons.Length; i++)
            {
                string oldKey = keys[i];
                string newKey = keyButtons[i]._keyBind;
                if (oldKey == newKey) continue;

                EasySettingsPlugin.Logger.LogInfo($"Key {i} changed from {oldKey} to {newKey}");

                if (SettingsTab.KeyButtonIndexToKeyButton.TryGetValue(i, out AtlyssKeyButton modKeyButton))
                {
                    modKeyButton.SetValue(Enum.Parse<KeyCode>(newKey));
                }
            }
        }
    }
}