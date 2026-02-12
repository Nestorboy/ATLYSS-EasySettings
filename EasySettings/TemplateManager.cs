using Nessie.ATLYSS.EasySettings.Prefabs;
using Nessie.ATLYSS.EasySettings.UIElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings;

internal static class TemplateManager
{
    private static AtlyssTabButton _tabTemplate;

    private static RectTransform _spaceTemplate;
    private static RectTransform _headerTemplate;
    private static RectTransform _buttonTemplate;
    private static RectTransform _toggleTemplate;
    private static RectTransform _simpleSliderTemplate;
    private static RectTransform _advancedSliderTemplate;
    private static RectTransform _dropdownTemplate;
    private static RectTransform _keyButtonTemplate;
    private static RectTransform _textFieldTemplate;
    private static RectTransform _tabSelectorTemplate;

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
        _tabTemplate = CreateTabButton((RectTransform)tabsContainer, firstTab);
        _tabTemplate.Root.gameObject.SetActive(false);

        _textFieldTemplate = TextFieldPrefab.Create();
        Object.DontDestroyOnLoad(_textFieldTemplate);
        _textFieldTemplate.gameObject.SetActive(false);

        _tabSelectorTemplate = TabSelectorPrefab.Create();
        Object.DontDestroyOnLoad(_tabSelectorTemplate);
        _tabSelectorTemplate.gameObject.SetActive(false);

        _spaceTemplate = CreateHiddenClone(FindSpace(manager));
        _headerTemplate = CreateHiddenClone(FindHeader(manager));
        _buttonTemplate = CreateHiddenClone(FindButton(manager));
        _toggleTemplate = CreateHiddenClone(FindToggle(manager));
        _simpleSliderTemplate = CreateHiddenClone(FindSimpleSlider(manager));
        _advancedSliderTemplate = CreateHiddenClone(FindAdvancedSlider(manager));
        _dropdownTemplate = CreateHiddenClone(FindDropdown(manager));
        _keyButtonTemplate = CreateHiddenClone(FindKeyButton(manager));
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

        RectTransform tabSelector = Object.Instantiate(_tabSelectorTemplate, settingsTab.Content);
        tabSelector.gameObject.SetActive(true);
        ComponentReferences components = tabSelector.GetComponent<ComponentReferences>();
        Button leftButton = (Button)components.components[0];
        Button rightButton = (Button)components.components[1];
        Text label = (Text)components.components[2];

        settingsTab.TabControlLabel = label;

        leftButton.onClick.AddListener(Settings.SelectPreviousTab);

        rightButton.onClick.AddListener(Settings.SelectNextTab);

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

    private static RectTransform CreateHiddenClone(RectTransform transform)
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

    internal static AtlyssTabButton CreateTabButton(RectTransform container) => CreateTabButton(container, _tabTemplate.Root);

    private static AtlyssTabButton CreateTabButton(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssTabButton atlyssTabButton = new()
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            Button = root.GetComponentInChildren<Button>(true),
        };

        atlyssTabButton.Initialize();

        return atlyssTabButton;
    }

    internal static AtlyssHeader CreateHeader(RectTransform container) => CreateHeader(container, _headerTemplate);

    private static AtlyssHeader CreateHeader(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssHeader header = new()
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
        };

        header.Initialize();

        return header;
    }

    internal static AtlyssButton CreateButton(RectTransform container) => CreateButton(container, _buttonTemplate);

    private static AtlyssButton CreateButton(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssButton button = new()
        {
            Root = root,
            ButtonLabel = root.GetComponentInChildren<Text>(true),
            Button = root.GetComponentInChildren<Button>(true),
        };

        button.Initialize();

        return button;
    }

    internal static AtlyssToggle CreateToggle(RectTransform container) => CreateToggle(container, _toggleTemplate);

    private static AtlyssToggle CreateToggle(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssToggle toggle = new()
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            Toggle = (Toggle)root.GetComponentInChildren<ComponentReferences>(true).components[0],
        };

        toggle.Initialize();

        return toggle;
    }

    internal static AtlyssSimpleSlider CreateSimpleSlider(RectTransform container) => CreateSimpleSlider(container, _simpleSliderTemplate);

    private static AtlyssSimpleSlider CreateSimpleSlider(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;

        AtlyssSimpleSlider advSlider = new()
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            Slider = (Slider)components[0],
        };

        advSlider.Initialize();

        return advSlider;
    }

    internal static AtlyssAdvancedSlider CreateAdvancedSlider(RectTransform container) => CreateAdvancedSlider(container, _advancedSliderTemplate);

    private static AtlyssAdvancedSlider CreateAdvancedSlider(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;

        AtlyssAdvancedSlider advSlider = new()
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

    internal static AtlyssDropdown CreateDropdown(RectTransform container) => CreateDropdown(container, _dropdownTemplate);

    private static AtlyssDropdown CreateDropdown(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;

        AtlyssDropdown dropdown = new()
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            Dropdown = (Dropdown)components[0],
        };

        dropdown.Initialize();

        return dropdown;
    }

    internal static AtlyssKeyButton CreateKeyButton(RectTransform container) => CreateKeyButton(container, _keyButtonTemplate);

    private static AtlyssKeyButton CreateKeyButton(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;

        AtlyssKeyButton keyButton = new()
        {
            Root = root,
            Label = root.GetComponentInChildren<Text>(true),
            ButtonLabel = (Text)components[1],
            Button = (Button)components[0],
        };

        keyButton.Initialize();

        return keyButton;
    }

    internal static AtlyssSpace CreateSpace(RectTransform container) => CreateSpace(container, _spaceTemplate);

    private static AtlyssSpace CreateSpace(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssSpace space = new() { Root = root };

        return space;
    }

    internal static AtlyssTextField CreateTextField(RectTransform container) => CreateTextField(container, _textFieldTemplate);

    private static AtlyssTextField CreateTextField(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;
        InputField inputField = (InputField)components[0];
        Text label = (Text)components[1];

        AtlyssTextField textField = new()
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