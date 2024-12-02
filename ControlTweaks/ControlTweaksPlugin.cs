using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Nessie.ATLYSS.EasySettings;
using UnityEngine;

namespace Nessie.ATLYSS.ControlTweaks;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ControlTweaksPlugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    internal static ConfigEntry<float> ConfigJumpBufferDuration;
    internal static ConfigEntry<KeyCode> ConfigWalkKey;
    internal static ConfigEntry<float> ConfigWalkSpeed;

    internal static float JumpBufferDuration => Mathf.Max(0f, ConfigJumpBufferDuration.Value);
    internal static KeyCode WalkKey => ConfigWalkKey.Value;
    internal static float WalkSpeed => Mathf.Clamp01(ConfigWalkSpeed.Value);

    private void Awake()
    {
        Logger = base.Logger;

        new Harmony(MyPluginInfo.PLUGIN_GUID).PatchAll();

        InitConfig();

        Settings.OnInitialized.AddListener(AddSettings);
        Settings.OnApplySettings.AddListener(() => { Config.Save(); });
    }

    private void InitConfig()
    {
        var bufferDefinition = new ConfigDefinition("Jumping", "JumpBufferDuration");
        var bufferDescription = new ConfigDescription("How long a regular jump and ledge jump is buffered for. Limited between 0 and 5.", new AcceptableValueRange<float>(0f, 5f));
        ConfigJumpBufferDuration = Config.Bind(bufferDefinition, 0.25f, bufferDescription);

        var walkKeyDefinition = new ConfigDefinition("Walking", "WalkKeyBind");
        var walkKeyDescription = new ConfigDescription("The key that needs to be held down in order to walk.");
        ConfigWalkKey = Config.Bind(walkKeyDefinition, KeyCode.LeftControl, walkKeyDescription);

        var walkSpeedDefinition = new ConfigDefinition("Walking", "WalkSpeedMultiplier");
        var walkSpeedDescription = new ConfigDescription("The speed multiplier that's applied when walking. Limited between 0 and 1.", new AcceptableValueRange<float>(0f, 1f));
        ConfigWalkSpeed = Config.Bind(walkSpeedDefinition, 0.4f, walkSpeedDescription);
    }

    private void AddSettings()
    {
        SettingsTab tab = Settings.ModTab;

        tab.AddHeader("Control Tweaks");

        tab.AddAdvancedSlider("Jump Buffer Duration", ConfigJumpBufferDuration);

        tab.AddKeyButton("Walk Key", ConfigWalkKey);

        tab.AddAdvancedSlider("Walk Speed", ConfigWalkSpeed);
    }
}