using Nessie.ATLYSS.EasySettings.Prefabs;
using Nessie.ATLYSS.EasySettings.UIElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings;

internal static class TemplateManager
{
    internal static AtlyssTabButton TabTemplate;

    internal static RectTransform SpaceTemplate;
    internal static RectTransform HeaderTemplate;
    internal static RectTransform ButtonTemplate;
    internal static RectTransform ToggleTemplate;
    internal static RectTransform SimpleSliderTemplate;
    internal static RectTransform AdvancedSliderTemplate;
    internal static RectTransform DropdownTemplate;
    internal static RectTransform KeyButtonTemplate;
    internal static RectTransform TextFieldTemplate;
    internal static RectTransform TabSelectorTemplate;

    #region Initialization

    internal static void Initialize()
    {
        FindTemplateElements();
    }

    private static void FindTemplateElements()
    {
        SettingsManager manager = Object.FindObjectOfType<SettingsManager>(true);

        Transform tabsContainer = Utility.FindTabsContainer(manager);
        if (!tabsContainer)
        {
            EasySettingsPlugin.Logger.LogError("Unable to find settings tabs container GameObject. EasySettings won't be available.");
            return;
        }

        RectTransform firstTab = (RectTransform)tabsContainer.GetChild(0);
        TabTemplate = CreateTabButton((RectTransform)tabsContainer, firstTab);
        TabTemplate.Root.gameObject.SetActive(false);

        TextFieldTemplate = TextFieldPrefab.Create();
        Object.DontDestroyOnLoad(TextFieldTemplate);
        TextFieldTemplate.gameObject.SetActive(false);

        TabSelectorTemplate = TabSelectorPrefab.Create();
        Object.DontDestroyOnLoad(TabSelectorTemplate);
        TabSelectorTemplate.gameObject.SetActive(false);

        SpaceTemplate = CreateHiddenClone(FindSpace(manager));
        HeaderTemplate = CreateHiddenClone(FindHeader(manager));
        ButtonTemplate = CreateHiddenClone(FindButton(manager));
        ToggleTemplate = CreateHiddenClone(FindToggle(manager));
        SimpleSliderTemplate = CreateHiddenClone(FindSimpleSlider(manager));
        AdvancedSliderTemplate = CreateHiddenClone(FindAdvancedSlider(manager));
        DropdownTemplate = CreateHiddenClone(FindDropdown(manager));
        KeyButtonTemplate = CreateHiddenClone(FindKeyButton(manager));
    }

    internal static void InitializeTabContent(SettingsTab settingsTab)
    {
        SettingsManager manager = Object.FindObjectOfType<SettingsManager>(true);

        Transform modTabElement = Object.Instantiate(manager._videoTabElement.transform, manager._videoTabElement.transform.parent);
        settingsTab.Element = modTabElement.GetComponent<MenuElement>();
        settingsTab.Content = (RectTransform)modTabElement.GetComponentInChildren<VerticalLayoutGroup>().transform;
        settingsTab.BottomSpace = settingsTab.AddSpace();

        foreach (RectTransform child in settingsTab.Content)
        {
            Object.Destroy(child.gameObject);
        }

        RectTransform tabSelector = Object.Instantiate(TabSelectorTemplate, settingsTab.Content);
        tabSelector.gameObject.SetActive(true);
        ComponentReferences components = tabSelector.GetComponent<ComponentReferences>();
        Button leftButton = (Button)components.components[0];
        Button rightButton = (Button)components.components[1];
        Text label = (Text)components.components[2];

        settingsTab.TabControlLabel = label;

        leftButton.onClick.AddListener(() =>
        {
            Settings.SettingsTabIndex--;
            if (Settings.SettingsTabIndex < 0)
            {
                Settings.SettingsTabIndex = Settings.SettingsTabs.Count - 1;
            }
            Settings.UpdateTabVisibility();
            SettingsManager._current._gamepadSelectAsrc.Play();
        });

        rightButton.onClick.AddListener(() =>
        {
            Settings.SettingsTabIndex++;
            if (Settings.SettingsTabIndex >= Settings.SettingsTabs.Count)
            {
                Settings.SettingsTabIndex = 0;
            }
            Settings.UpdateTabVisibility();
            SettingsManager._current._gamepadSelectAsrc.Play();
        });

        label.text = settingsTab.TabName;
    }

    private static RectTransform[] GetVanillaTabs(SettingsManager manager)
    {
        // Input dolly is higher priority since it's part of another game tab
        return [
            manager._keyboardInputDollyObject.GetComponent<RectTransform>(),
            manager._videoTabContent,
            manager._audioTabContent,
            manager._inputTabContent,
            manager._networkTabContent,
        ];
    }

    private static RectTransform FindSpace(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        foreach (RectTransform tab in tabContents)
        {
            foreach (RectTransform child in tab)
            {
                if (child.childCount != 0) continue;

                Component[] components = child.GetComponents<Component>();
                if (components.Length > 1) continue;

                return child;
            }
        }

        return null;
    }

    private static RectTransform FindHeader(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        foreach (RectTransform tab in tabContents)
        {
            foreach (RectTransform child in tab)
            {
                Selectable selectable = child.GetComponentInChildren<Selectable>(true);
                if (selectable) continue;

                Text text = child.GetComponentInChildren<Text>(true);
                if (!text) continue;

                return child;
            }
        }

        return null;
    }

    private static RectTransform FindButton(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        foreach (RectTransform tab in tabContents)
        {
            foreach (RectTransform child in tab)
            {
                Button butt = child.GetComponentInChildren<Button>(true);
                if (!butt) continue;

                Text[] texts = child.GetComponentsInChildren<Text>(true);
                if (texts.Length > 1 || !texts[0].transform.IsChildOf(butt.transform)) continue;

                Selectable[] selectables = child.GetComponentsInChildren<Selectable>(true);
                if (selectables.Length > 1) continue;

                return child;
            }
        }

        return null;
    }

    private static RectTransform FindToggle(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        Toggle toggle = manager._jiggleBonesToggle;

        if (!Utility.TryGetElementRoot(tabContents, toggle.transform, out Transform root)) return null;

        List<Component> compRefs = root.gameObject.AddComponent<ComponentReferences>().components;
        compRefs.Add(toggle);

        return (RectTransform)root;
    }

    private static RectTransform FindSimpleSlider(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        Slider slider = manager._masterVolumeSlider;

        if (!Utility.TryGetElementRoot(tabContents, slider.transform, out Transform root)) return null;

        List<Component> compRefs = root.gameObject.AddComponent<ComponentReferences>().components;
        compRefs.Add(slider);

        return (RectTransform)root;
    }

    private static RectTransform FindAdvancedSlider(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        Slider slider = manager._cameraFieldOfViewSlider;
        Text valueText = manager._cameraFieldOfViewText;

        if (!Utility.TryGetElementRoot(tabContents, slider.transform, out Transform root)) return null;

        List<Component> compRefs = root.gameObject.AddComponent<ComponentReferences>().components;
        compRefs.Add(slider);
        compRefs.Add(valueText);

        return (RectTransform)root;
    }

    private static RectTransform FindDropdown(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        Dropdown dropdown = manager._fullScreenResolutionDropdown;

        if (!Utility.TryGetElementRoot(tabContents, dropdown.transform, out Transform root)) return null;

        List<Component> compRefs = root.gameObject.AddComponent<ComponentReferences>().components;
        compRefs.Add(dropdown);

        return (RectTransform)root;
    }

    private static RectTransform FindKeyButton(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        KeyBindButton[] keyButtons = manager.keyBindButtons;
        if (keyButtons is not { Length: > 0 }) return null;

        foreach (KeyBindButton keyButton in keyButtons)
        {
            if (keyButton == null) continue;

            Button button = keyButton._button;
            Text valueText = keyButton._buttonLabel;
            if (!Utility.TryGetElementRoot(tabContents, button.transform, out Transform root)) continue;

            List<Component> compRefs = root.gameObject.AddComponent<ComponentReferences>().components;
            compRefs.Add(button);
            compRefs.Add(valueText);

            return (RectTransform)root;
        }

        return null;
    }

    internal static RectTransform CreateHiddenClone(RectTransform transform)
    {
        if (transform == null)
        {
            return null;
        }

        RectTransform root = Object.Instantiate(transform, null);
        root.gameObject.SetActive(false);
        Object.DontDestroyOnLoad(root);
        return root;
    }

    #endregion Initialization

    #region Create

    internal static AtlyssTabButton CreateTabButton(RectTransform container) => CreateTabButton(container, TabTemplate.Root);

    internal static AtlyssTabButton CreateTabButton(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssTabButton atlyssTabButton = new AtlyssTabButton
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            Button = root.GetComponentInChildren<Button>(true),
        };

        atlyssTabButton.Initialize();

        return atlyssTabButton;
    }

    internal static AtlyssHeader CreateHeader(RectTransform container) => CreateHeader(container, HeaderTemplate);

    internal static AtlyssHeader CreateHeader(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssHeader header = new AtlyssHeader
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
        };

        header.Initialize();

        return header;
    }

    internal static AtlyssButton CreateButton(RectTransform container) => CreateButton(container, ButtonTemplate);

    internal static AtlyssButton CreateButton(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssButton button = new AtlyssButton
        {
            Root = root,
            ButtonLabel = root.GetComponentInChildren<Text>(true),
            Button = root.GetComponentInChildren<Button>(true),
        };

        button.Initialize();

        return button;
    }

    internal static AtlyssToggle CreateToggle(RectTransform container) => CreateToggle(container, ToggleTemplate);

    internal static AtlyssToggle CreateToggle(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssToggle toggle = new AtlyssToggle
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            Toggle = (Toggle)root.GetComponentInChildren<ComponentReferences>(true).components[0],
        };

        toggle.Initialize();

        return toggle;
    }

    internal static AtlyssSimpleSlider CreateSimpleSlider(RectTransform container) => CreateSimpleSlider(container, SimpleSliderTemplate);

    internal static AtlyssSimpleSlider CreateSimpleSlider(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;

        AtlyssSimpleSlider advSlider = new AtlyssSimpleSlider
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            Slider = (Slider)components[0],
        };

        advSlider.Initialize();

        return advSlider;
    }

    internal static AtlyssAdvancedSlider CreateAdvancedSlider(RectTransform container) => CreateAdvancedSlider(container, AdvancedSliderTemplate);

    internal static AtlyssAdvancedSlider CreateAdvancedSlider(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;

        AtlyssAdvancedSlider advSlider = new AtlyssAdvancedSlider
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            ValueText = (Text)components[1],
            Slider = (Slider)components[0],
            ResetButton = root.GetComponentInChildren<Button>(true),
        };

        advSlider.Initialize();

        return advSlider;
    }

    internal static AtlyssDropdown CreateDropdown(RectTransform container) => CreateDropdown(container, DropdownTemplate);

    internal static AtlyssDropdown CreateDropdown(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;

        AtlyssDropdown dropdown = new AtlyssDropdown
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            Dropdown = (Dropdown)components[0],
        };

        dropdown.Initialize();

        return dropdown;
    }

    internal static AtlyssKeyButton CreateKeyButton(RectTransform container) => CreateKeyButton(container, KeyButtonTemplate);

    internal static AtlyssKeyButton CreateKeyButton(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;

        AtlyssKeyButton keyButton = new AtlyssKeyButton
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            ButtonLabel = (Text)components[1],
            Button = (Button)components[0],
        };

        keyButton.Initialize();

        return keyButton;
    }

    internal static AtlyssSpace CreateSpace(RectTransform container) => CreateSpace(container, SpaceTemplate);

    internal static AtlyssSpace CreateSpace(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssSpace space = new AtlyssSpace { Root = root };

        return space;
    }

    internal static AtlyssTextField CreateTextField(RectTransform container) => CreateTextField(container, TextFieldTemplate);

    internal static AtlyssTextField CreateTextField(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;
        InputField inputField = (InputField)components[0];
        Text label = (Text)components[1];

        AtlyssTextField textField = new AtlyssTextField
        {
            Root = root,
            Label = label,
            Placeholder = (Text)inputField.placeholder,
            InputField = inputField,
        };

        textField.Placeholder.color = Color.gray;
        textField.InputField.textComponent.color = Color.white;
        textField.Label.color = Color.white;

        ColorBlock currColors = textField.InputField.colors;
        currColors.normalColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
        textField.InputField.colors = currColors;

        textField.InputField.characterLimit = 10000;

        textField.Initialize();

        return textField;
    }

    #endregion
}