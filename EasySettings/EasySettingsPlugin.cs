using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nessie.ATLYSS.EasySettings.UIElements;

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
            if (!TryAddModTabButton())
            {
                return;
            }

            Settings.OnInitialized.Invoke();
        });
    }

    private bool TryAddModTabButton()
    {
        if (!Utility.TryGetNextUnusedMenuIndex(out byte unusedIndex))
        {
            Logger.LogError("Unable to find unused menu index to use for 'Mods' tab. EasySettings won't be available.");
            return false;
        }

        Settings.ModTabIndex = unusedIndex;
        AtlyssTabButton modTab = Settings.AddTabButton("Mods");
        modTab.OnClicked.AddListener(() => { SettingsManager._current.Set_SettingMenuSelectionIndex(Settings.ModTabIndex); });
        Settings.ModTab.TabButton = modTab;
        return true;
    }
}