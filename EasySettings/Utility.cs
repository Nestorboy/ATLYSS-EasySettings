using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings;

public static class Utility
{
    /// <summary>
    /// Returns the child of relativeParent that's a parent or grandparent of child.
    /// </summary>
    /// <param name="relativeParent"></param>
    /// <param name="child"></param>
    /// <param name="root"></param>
    /// <returns></returns>
    public static bool TryGetRelativeRoot(Transform relativeParent, Transform child, out Transform root)
    {
        root = child;
        while (root)
        {
            if (root.parent == relativeParent)
            {
                return true;
            }
            root = root.parent;
        }

        root = null;
        return false;
    }

    public static bool TryGetElementRoot(Transform[] contents, Transform elementChild, out Transform root)
    {
        foreach (Transform content in contents)
        {
            if (TryGetRelativeRoot(content, elementChild.transform, out root))
            {
                return true;
            }
        }

        root = null;
        return false;
    }

    public static bool TryGetElementRoot(RectTransform[] contents, Transform elementChild, out Transform root)
    {
        foreach (RectTransform content in contents)
        {
            if (TryGetRelativeRoot(content, elementChild.transform, out root))
            {
                return true;
            }
        }

        root = null;
        return false;
    }

    public static Transform FindTabsContainer(SettingsManager instance)
    {
        foreach (Button butt in instance.GetComponentsInChildren<Button>(true))
        {
            foreach (PersistentCall call in butt.onClick.m_PersistentCalls.m_Calls)
            {
                if (call == null) continue;
                if (call.target != instance) continue;
                if (call.methodName != nameof(SettingsManager.Set_SettingMenuSelectionIndex)) continue;

                return butt.transform.parent;
            }
        }

        return null;
    }

    public static Button FindCancelButton(SettingsManager instance)
    {
        foreach (Button butt in instance.GetComponentsInChildren<Button>(true))
        {
            foreach (PersistentCall call in butt.onClick.m_PersistentCalls.m_Calls)
            {
                if (call == null) continue;
                if (call.target != instance) continue;
                if (call.methodName != nameof(SettingsManager.Close_SettingsMenu)) continue;

                return butt;
            }
        }

        return null;
    }

    public static Button FindApplyButton(SettingsManager instance)
    {
        foreach (Button butt in instance.GetComponentsInChildren<Button>(true))
        {
            foreach (PersistentCall call in butt.onClick.m_PersistentCalls.m_Calls)
            {
                if (call == null) continue;
                if (call.target != instance) continue;
                if (call.methodName != nameof(SettingsManager.Save_SettingsData)) continue;

                return butt;
            }
        }

        return null;
    }

    public static bool IsKeyBound(SettingsManager instance, KeyCode key) => IsKeyBound(instance, key.ToString());

    public static bool IsKeyBound(SettingsManager instance, string key)
    {
        foreach (KeyBindButton keyButton in instance.keyBindButtons)
        {
            if (keyButton._keyBind == key) return true;
        }

        return false;
    }

    /// <summary>
    /// Used for finding the next available menu index to use when inserting additional menu tabs.
    /// </summary>
    /// <param name="settingsManager">The SettingsManager to search for an unused menu index in.</param>
    /// <param name="index">The next unused menu index as a byte, 0 if there was no unused index.</param>
    /// <returns>true if an unused menu index was found, false if none was found.</returns>
    public static bool TryGetNextUnusedMenuIndex(SettingsManager settingsManager, out byte index)
    {
        MenuElement menusContainer = settingsManager._settingsMenuElement;
        MenuElement[] menus = menusContainer.GetComponentsInChildren<MenuElement>(true);
        MenuElement[] childMenus = menus.Where(menu => menu != menusContainer).ToArray();

        SettingsManager.SettingsMenuSelection oldMenuSelection = settingsManager._currentSettingsMenuSelection;
        int nextIndex = 0;
        while (nextIndex <= byte.MaxValue)
        {
            settingsManager.Set_SettingMenuSelectionIndex(nextIndex);
            if (!childMenus.Any(menu => menu.isEnabled))
            {
                break;
            }

            nextIndex++;
        }

        settingsManager.Set_SettingMenuSelectionIndex((int)oldMenuSelection);
        index = (byte)Math.Min(nextIndex, byte.MaxValue);
        return nextIndex <= byte.MaxValue;
    }

    /// <summary>
    /// Used for finding the next available menu index to use when inserting additional menu tabs.
    /// </summary>
    /// <param name="index">The next unused menu index as a byte, 0 if there was no unused index.</param>
    /// <returns>true if an unused menu index was found, false if none was found.</returns>
    public static bool TryGetNextUnusedMenuIndex(out byte index) => TryGetNextUnusedMenuIndex(SettingsManager._current, out index);
}