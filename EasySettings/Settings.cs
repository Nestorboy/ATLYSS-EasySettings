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
    internal static int SettingsTabIndex { get; set; }
    internal static List<SettingsTab> SettingsTabs { get; } = [ModTab];

    /// <summary>
    /// Finds or creates a new settings tab with the given name.
    /// </summary>
    /// <param name="label">The name of the settings tab.</param>
    /// <returns>An existing or new tab you can add settings to.</returns>
    public static SettingsTab GetOrAddCustomTab(string label)
    {
        SettingsTab existingTab = SettingsTabs.Find(tab => tab.TabName == label);
        if (existingTab != null)
        {
            return existingTab;
        }

        SettingsTab tab = new()
        {
            TabName = label,
        };

        SettingsTabs.Add(tab);
        SettingsTabs.Sort((first, second) =>
        {
            if (first == ModTab)
            {
                return -1;
            }

            if (second == ModTab)
            {
                return 1;
            }

            return string.Compare(first.TabName, second.TabName, StringComparison.Ordinal);
        });

        SettingsTabIndex = 0;
        TemplateManager.InitializeTabContent(tab);

        for (int i = 0; i < SettingsTabs.Count; i++)
        {
            SettingsTabs[i].TabControlLabel.text = $"{SettingsTabs[i].TabName} ({i + 1} / {SettingsTabs.Count})";
        }

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
        bool currentlyOnModTab = (int)SettingsManager._current._currentSettingsMenuSelection == ModTabIndex;

        for (int i = 0; i < SettingsTabs.Count; i++)
        {
            SettingsTab tab = SettingsTabs[i];

            if (tab.Element)
            {
                tab.Element.isEnabled = currentlyOnModTab && i == SettingsTabIndex;
            }

            if (tab.Content)
            {
                tab.Content.anchoredPosition = Vector2.zero;
            }
        }
    }

    internal static void SelectNextTab()
    {
        SettingsTabIndex++;
        if (SettingsTabIndex >= SettingsTabs.Count)
        {
            SettingsTabIndex = 0;
        }

        UpdateTabVisibility();
        SettingsManager._current._gamepadSelectAsrc.Play();
    }

    internal static void SelectPreviousTab()
    {
        SettingsTabIndex--;
        if (SettingsTabIndex < 0)
        {
            SettingsTabIndex = SettingsTabs.Count - 1;
        }

        UpdateTabVisibility();
        SettingsManager._current._gamepadSelectAsrc.Play();
    }
}