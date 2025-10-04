using Nessie.ATLYSS.EasySettings.UIElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings;

internal static class TemplateManager
{
    internal static AtlyssTabButton TabTemplate;

    internal static AtlyssSpace SpaceTemplate;
    internal static AtlyssHeader HeaderTemplate;
    internal static AtlyssButton ButtonTemplate;
    internal static AtlyssToggle ToggleTemplate;
    internal static AtlyssSimpleSlider SimpleSliderTemplate;
    internal static AtlyssAdvancedSlider AdvancedSliderTemplate;
    internal static AtlyssDropdown DropdownTemplate;
    internal static AtlyssKeyButton KeyButtonTemplate;
    internal static AtlyssTextField TextFieldTemplate;

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

        // TODO: Initialize template MenuElement and Content instead.
        SettingsTab modTab = Settings.ModTab;
        Transform modTabElement = Object.Instantiate(manager._videoTabElement.transform, manager._videoTabElement.transform.parent);
        modTab.Element = modTabElement.GetComponent<MenuElement>();
        modTab.Content = (RectTransform)modTabElement.GetComponentInChildren<VerticalLayoutGroup>().transform;

        InitializeTabContent(modTab.Content);

        RectTransform textFieldRoot = FindTextField(manager);
        if (textFieldRoot)
        {
            TextFieldTemplate = CreateTextField(modTab.Content, textFieldRoot);
            TextFieldTemplate.Root.gameObject.SetActive(false);
        }

        RectTransform spaceRoot = FindSpace(manager);
        if (spaceRoot)
        {
            SpaceTemplate = CreateSpace(modTab.Content, spaceRoot);
            SpaceTemplate.Root.gameObject.SetActive(false);
        }

        RectTransform headerRoot = FindHeader(manager);
        if (headerRoot)
        {
            HeaderTemplate = CreateHeader(modTab.Content, headerRoot);
            HeaderTemplate.Root.gameObject.SetActive(false);
        }

        RectTransform buttonRoot = FindButton(manager);
        if (buttonRoot)
        {
            ButtonTemplate = CreateButton(modTab.Content, buttonRoot);
            ButtonTemplate.Root.gameObject.SetActive(false);
        }

        RectTransform toggleRoot = FindToggle(manager);
        if (toggleRoot)
        {
            ToggleTemplate = CreateToggle(modTab.Content, toggleRoot);
            ToggleTemplate.Root.gameObject.SetActive(false);
        }

        RectTransform simpleSliderRoot = FindSimpleSlider(manager);
        if (simpleSliderRoot)
        {
            SimpleSliderTemplate = CreateSimpleSlider(modTab.Content, simpleSliderRoot);
            SimpleSliderTemplate.Root.gameObject.SetActive(false);
        }

        RectTransform advancedSliderRoot = FindAdvancedSlider(manager);
        if (advancedSliderRoot)
        {
            AdvancedSliderTemplate = CreateAdvancedSlider(modTab.Content, advancedSliderRoot);
            AdvancedSliderTemplate.Root.gameObject.SetActive(false);
        }

        RectTransform dropdownRoot = FindDropdown(manager);
        if (dropdownRoot)
        {
            DropdownTemplate = CreateDropdown(modTab.Content, dropdownRoot);
            DropdownTemplate.Root.gameObject.SetActive(false);
        }

        RectTransform keyButtonRoot = FindKeyButton(manager);
        if (keyButtonRoot)
        {
            KeyButtonTemplate = CreateKeyButton(modTab.Content, keyButtonRoot);
            KeyButtonTemplate.Root.gameObject.SetActive(false);
        }
    }

    private static void InitializeTabContent(RectTransform tabContent)
    {
        foreach (RectTransform child in tabContent)
        {
            Object.Destroy(child.gameObject);
        }
    }

    private static RectTransform[] GetVanillaTabs(SettingsManager manager)
    {
        return [manager._videoTabContent, manager._audioTabContent, manager._inputTabContent, manager._networkTabContent];
    }

    private static RectTransform FindTextField(SettingsManager manager)
    {
        RectTransform[] tabContents = GetVanillaTabs(manager);

        InputField inputField = manager._defaultChatRoomNameInput;

        if (!Utility.TryGetElementRoot(tabContents, inputField.transform, out Transform root))
            return null;

        List<Component> compRefs = root.gameObject.AddComponent<ComponentReferences>().components;
        compRefs.Add(inputField);

        return (RectTransform)root;
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

    #endregion Initialization

    internal static AtlyssTabButton CreateTabButton(RectTransform container) => CreateTabButton(container, TabTemplate);

    internal static AtlyssTabButton CreateTabButton(RectTransform container, AtlyssTabButton template) => CreateTabButton(container, template.Root);

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

    internal static AtlyssHeader CreateHeader(RectTransform container, AtlyssHeader template) => CreateHeader(container, template.Root);

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

    internal static AtlyssButton CreateButton(RectTransform container, AtlyssButton template) => CreateButton(container, template.Root);

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

    internal static AtlyssToggle CreateToggle(RectTransform container, AtlyssToggle template) => CreateToggle(container, template.Root);

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

    internal static AtlyssSimpleSlider CreateSimpleSlider(RectTransform container, AtlyssSimpleSlider template) => CreateSimpleSlider(container, template.Root);

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

    internal static AtlyssAdvancedSlider CreateAdvancedSlider(RectTransform container, AtlyssAdvancedSlider template) => CreateAdvancedSlider(container, template.Root);

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

    internal static AtlyssDropdown CreateDropdown(RectTransform container, AtlyssDropdown template) => CreateDropdown(container, template.Root);

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

    internal static AtlyssKeyButton CreateKeyButton(RectTransform container, AtlyssKeyButton template) => CreateKeyButton(container, template.Root);

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

    internal static AtlyssSpace CreateSpace(RectTransform container, AtlyssSpace template) => CreateSpace(container, template.Root);

    internal static AtlyssSpace CreateSpace(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        AtlyssSpace space = new AtlyssSpace { Root = root };

        return space;
    }

    internal static AtlyssTextField CreateTextField(RectTransform container) => CreateTextField(container, TextFieldTemplate);

    internal static AtlyssTextField CreateTextField(RectTransform container, AtlyssTextField template) => CreateTextField(container, template.Root);

    internal static AtlyssTextField CreateTextField(RectTransform container, RectTransform template)
    {
        RectTransform root = Object.Instantiate(template, container);

        List<Component> components = root.GetComponentInChildren<ComponentReferences>(true).components;
        Text[] textComponents = root.GetComponentsInChildren<Text>(true);
        InputField inputField = (InputField)components[0];

        AtlyssTextField textField = new AtlyssTextField
        {
            Root = root,
            Label = textComponents[2],
            Placeholder = (Text)inputField.placeholder,
            InputField = inputField,
        };

        textField.Placeholder.color = Color.gray;
        textField.InputField.textComponent.color = Color.white;
        textField.Label.color = Color.white;

        Object.Destroy(textComponents[3].gameObject); // hashtag symbol next to text field, destroy it last

        ColorBlock currColors = textField.InputField.colors;
        currColors.normalColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
        textField.InputField.colors = currColors;

        textField.InputField.characterLimit = 32;

        textField.Initialize();

        return textField;
    }
}