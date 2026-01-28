using System;
using System.Collections.Generic;
using Nessie.ATLYSS.EasySettings.UIElements;
using UnityEngine;
using UnityEngine.Events;

namespace Nessie.ATLYSS.EasySettings;

public static class Settings
{
    /// <summary>
    /// Default tab that can be used for settings.
    /// </summary>
    public static SettingsTab ModTab { get; } = new();

    /// <summary>
    /// Invoked whenever the EasySettings system has been initialized. The system depends on the SettingsManager which is initialized in Start, so it has to wait on that for now.
    /// </summary>
    public static UnityEvent OnInitialized { get; } = new();

    /// <summary>
    /// A callback for whenever the user presses the "Cancel" button.
    /// </summary>
    public static UnityEvent OnCancelSettings { get; } = new();

    /// <summary>
    /// A callback which is invoked whenever the user presses the "Apply" button. Useful for finalizing and saving settings.
    /// </summary>
    public static UnityEvent OnApplySettings { get; } = new();

    /// <summary>
    /// A callback for when the settings menu is closed, this includes when the user presses "Cancel" or "Apply".
    /// </summary>
    public static UnityEvent OnCloseSettings { get; } = new();

    internal static int ModTabIndex { get; set; }
    internal static int SettingsTabIndex { get; set; } = 0;
    internal static List<SettingsTab> SettingsTabs { get; } = [ModTab];

    /// <summary>
    /// Creates a new settings tab with the given name.
    /// </summary>
    /// <param name="label">The name of the settings tab</param>
    /// <returns>A new tab you can use for organizing settings</returns>
    public static SettingsTab AddCustomTab(string label)
    {
        SettingsTab tab = new SettingsTab
        {
            TabName = label
        };
        
        SettingsTabs.Add(tab);
        SettingsTabs.Sort((first, second) =>
        {
            if (first == ModTab)
                return -1;
            
            if (second == ModTab)
                return 1;

            return string.Compare(first.TabName, second.TabName, StringComparison.Ordinal);
        });
        SettingsTabIndex = 0;
        TemplateManager.InitializeTabContent(tab);

        for (int i = 0; i < SettingsTabs.Count; i++)
            SettingsTabs[i].TabControlLabel.text = $"{SettingsTabs[i].TabName} ({i + 1} / {SettingsTabs.Count})";

        return tab;
    }
    
    internal static AtlyssTabButton AddTabButton(string label)
    {
        SettingsManager manager = SettingsManager._current;
        Transform tabsContainer = Utility.FindTabsContainer(manager);

        AtlyssTabButton tab = TemplateManager.CreateTabButton((RectTransform)tabsContainer);
        tab.Label.text = label;
        tab.Root.gameObject.SetActive(true);

        return tab;
    }

    internal static void UpdateTabVisibility()
    {
        bool currentlyOnModTab = (int)SettingsManager._current._currentSettingsMenuSelection == Settings.ModTabIndex;
        
        for (int i = 0; i < SettingsTabs.Count; i++)
        {
            var tab = SettingsTabs[i];
            
            if (tab.Element)
                tab.Element.isEnabled = currentlyOnModTab && i == Settings.SettingsTabIndex;
            
            if (tab.Content)
                tab.Content.anchoredPosition = Vector2.zero;
        }
    }
}