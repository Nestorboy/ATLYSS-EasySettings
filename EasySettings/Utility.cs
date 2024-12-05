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
}