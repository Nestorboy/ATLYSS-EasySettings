using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nessie.ATLYSS.EasySettings.UIElements;
using UnityEngine;

namespace Nessie.ATLYSS.EasySettings;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("ATLYSS.exe")]
public class EasySettingsPlugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;

        new Harmony(MyPluginInfo.PLUGIN_GUID).PatchAll();

        SettingsPatches.OnSettingsMenuInitialized.AddListener(() =>
        {
            AddModTabButton();
            AddModTabBottomSpace();
            AddPreviewElements();
            Settings.OnInitialized.Invoke();
        });
    }

    private void AddModTabButton()
    {
        Settings.ModTabIndex = 4;
        AtlyssTabButton modTab = Settings.AddTabButton("Mods");
        modTab.OnClicked.AddListener(() => { SettingsManager._current.Set_SettingMenuSelectionIndex(Settings.ModTabIndex); });
        Settings.ModTab.TabButton = modTab;
    }

    private void AddModTabBottomSpace()
    {
        Settings.ModTab.BottomSpace = Settings.ModTab.AddSpace();
    }

    private void AddPreviewElements()
    {
        SettingsTab tab = Settings.ModTab;

        tab.AddHeader("Easy Settings");

        tab.AddToggle("Toggles!", true);

        tab.AddAdvancedSlider("Sliders!", 0.4f);

        tab.AddKeyButton("Key Binds!", KeyCode.LeftControl);

        tab.AddDropdown("Dropdowns!", new []{ "Circle", "Triangle", "Square" });
    }
}