using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nessie.ATLYSS.EasySettings.UIElements;

namespace Nessie.ATLYSS.EasySettings;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
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
}