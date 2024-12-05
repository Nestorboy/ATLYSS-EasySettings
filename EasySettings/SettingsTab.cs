using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using Nessie.ATLYSS.EasySettings.UIElements;
using UnityEngine;
using UnityEngine.Events;

namespace Nessie.ATLYSS.EasySettings;

public class SettingsTab
{
    private const float DEFAULT_SLIDER_MIN = 0f;
    private const float DEFAULT_SLIDER_MAX = 1f;

    public AtlyssTabButton TabButton;
    public MenuElement Element;
    public RectTransform Content;
    public List<BaseAtlyssElement> ContentElements = new();

    internal AtlyssSpace BottomSpace;

    internal static Dictionary<int, AtlyssKeyButton> KeyButtonIndexToKeyButton = new();

    public AtlyssSpace AddSpace()
    {
        AtlyssSpace element = TemplateManager.CreateSpace(Content);
        element.Root.gameObject.SetActive(true);

        PushElement(element);
        return element;
    }

    /// <summary>
    /// An element that just has a label and darker background, useful for category separators.
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public AtlyssHeader AddHeader(string label)
    {
        AtlyssHeader element = TemplateManager.CreateHeader(Content);
        element.Label.text = label;
        element.Root.gameObject.SetActive(true);

        PushElement(element);
        return element;
    }

    public AtlyssButton AddButton(string buttonLabel, UnityAction onClick)
    {
        AtlyssButton element = AddButton(buttonLabel);
        element.Button.onClick.AddListener(onClick);

        return element;
    }

    public AtlyssButton AddButton(string buttonLabel)
    {
        AtlyssButton element = TemplateManager.CreateButton(Content);
        element.ButtonLabel.text = buttonLabel;
        element.Root.gameObject.SetActive(true);

        PushElement(element);
        return element;
    }

    public AtlyssToggle AddToggle(ConfigEntry<bool> config) => AddToggle(config.Definition.Key, config);

    public AtlyssToggle AddToggle(string label, ConfigEntry<bool> config)
    {
        AtlyssToggle element = AddToggle(label, config.Value);
        element.OnValueChanged.AddListener(newValue => { config.Value = newValue; });

        return element;
    }

    public AtlyssToggle AddToggle(string label, bool value = false)
    {
        AtlyssToggle element = TemplateManager.CreateToggle(Content);
        Settings.OnCloseSettings.AddListener(() => element.Revert());
        Settings.OnApplySettings.AddListener(() => element.Apply());

        element.Label.text = label;
        element.Toggle.SetIsOnWithoutNotify(value);
        element.Apply();
        element.Root.gameObject.SetActive(true);

        PushElement(element);
        return element;
    }

    public AtlyssSimpleSlider AddSlider(ConfigEntry<float> config, bool wholeNumbers = false) => AddSlider(config.Definition.Key, config, wholeNumbers);

    public AtlyssSimpleSlider AddSlider(string label, ConfigEntry<float> config, bool wholeNumbers = false)
    {
        float min = DEFAULT_SLIDER_MIN, max = DEFAULT_SLIDER_MAX;
        if (config.Description.AcceptableValues is AcceptableValueRange<float> range)
        {
            min = range.MinValue;
            max = range.MaxValue;
        }

        AtlyssSimpleSlider element = AddSlider(label, config.Value, min, max, wholeNumbers);
        element?.OnValueChanged.AddListener(newValue => { config.Value = newValue; });

        return element;
    }

    public AtlyssSimpleSlider AddSlider(string label, float value = 0f, float min = DEFAULT_SLIDER_MIN, float max = DEFAULT_SLIDER_MAX, bool wholeNumbers = false)
    {
        AtlyssSimpleSlider element = TemplateManager.CreateSimpleSlider(Content);
        Settings.OnCloseSettings.AddListener(() => element.Revert());
        Settings.OnApplySettings.AddListener(() => element.Apply());

        element.Label.text = label;
        element.Slider.wholeNumbers = wholeNumbers;
        element.Slider.minValue = min;
        element.Slider.maxValue = max;
        element.Slider.SetValueWithoutNotify(value);
        element.Apply();
        element.Root.gameObject.SetActive(true);

        PushElement(element);
        return element;
    }

    /// <summary>
    /// Adds a snapped slider with a reset button and value display with the label being the config key.
    /// To control the range using the config, make sure to pass an AcceptableValueRange when binding the config.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public AtlyssAdvancedSlider AddAdvancedSlider(ConfigEntry<int> config) => AddAdvancedSlider(config.Definition.Key, config);

    /// <summary>
    /// Adds a snapped slider with a reset button and value display.
    /// To control the range using the config, make sure to pass an AcceptableValueRange when binding the config.
    /// </summary>
    /// <param name="label"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public AtlyssAdvancedSlider AddAdvancedSlider(string label, ConfigEntry<int> config)
    {
        float min = DEFAULT_SLIDER_MIN, max = DEFAULT_SLIDER_MAX;
        if (config.Description.AcceptableValues is AcceptableValueRange<float> range)
        {
            min = range.MinValue;
            max = range.MaxValue;
        }

        AtlyssAdvancedSlider element = AddAdvancedSlider(label, config.Value, min, max, true);
        element?.OnValueChanged.AddListener(newValue => { config.Value = Mathf.RoundToInt(newValue); });
        element?.OnResetClicked.AddListener(() => { element.SetValue((float)config.DefaultValue); });

        return element;
    }

    /// <summary>
    /// Adds a slider with a reset button and value display with the label being the config key.
    /// To control the range using the config, make sure to pass an AcceptableValueRange when binding the config.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="wholeNumbers"></param>
    /// <returns></returns>
    public AtlyssAdvancedSlider AddAdvancedSlider(ConfigEntry<float> config, bool wholeNumbers = false) => AddAdvancedSlider(config.Definition.Key, config, wholeNumbers);

    /// <summary>
    /// Adds a slider with a reset button and value display.
    /// To control the range using the config, make sure to pass an AcceptableValueRange when binding the config.
    /// </summary>
    /// <param name="label"></param>
    /// <param name="config"></param>
    /// <param name="wholeNumbers"></param>
    /// <returns></returns>
    public AtlyssAdvancedSlider AddAdvancedSlider(string label, ConfigEntry<float> config, bool wholeNumbers = false)
    {
        float min = DEFAULT_SLIDER_MIN, max = DEFAULT_SLIDER_MAX;
        if (config.Description.AcceptableValues is AcceptableValueRange<float> range)
        {
            min = range.MinValue;
            max = range.MaxValue;
        }

        AtlyssAdvancedSlider element = AddAdvancedSlider(label, config.Value, min, max, wholeNumbers);
        element?.OnValueChanged.AddListener(newValue => { config.Value = newValue; });
        element?.OnResetClicked.AddListener(() => { element.SetValue((float)config.DefaultValue); });

        return element;
    }

    /// <summary>
    /// Adds a slider with a reset button and value display.
    /// </summary>
    /// <param name="label"></param>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="wholeNumbers"></param>
    /// <returns></returns>
    public AtlyssAdvancedSlider AddAdvancedSlider(string label, float value = 0f, float min = DEFAULT_SLIDER_MIN, float max = DEFAULT_SLIDER_MAX, bool wholeNumbers = false)
    {
        AtlyssAdvancedSlider element = TemplateManager.CreateAdvancedSlider(Content);
        Settings.OnCloseSettings.AddListener(() => element.Revert());
        Settings.OnApplySettings.AddListener(() => element.Apply());

        element.Label.text = label;
        element.ValueText.text = value.ToString(CultureInfo.InvariantCulture);
        element.Slider.onValueChanged.AddListener(newValue => { element.ValueText.text = newValue.ToString(CultureInfo.InvariantCulture); });
        element.Slider.wholeNumbers = wholeNumbers;
        element.Slider.minValue = min;
        element.Slider.maxValue = max;
        element.Slider.SetValueWithoutNotify(value);
        element.Apply();
        element.Root.gameObject.SetActive(true);

        PushElement(element);
        return element;
    }

    // TODO: Figure out way to avoid having to make other mods import UnityEngine.UI
    //public AtlyssDropdown AddDropdown(string label, List<Dropdown.OptionData> options, int value = 0)
    //{
    //    AtlyssDropdown element = AddDropdown(label, value);
    //    element.Dropdown.AddOptions(options);
    //    return element;
    //}

    public AtlyssDropdown AddDropdown<T>(ConfigEntry<T> config) where T : Enum => AddDropdown(config.Definition.Key, config);

    public AtlyssDropdown AddDropdown<T>(string label, ConfigEntry<T> config) where T : Enum
    {
        string[] enumNames = Enum.GetNames(config.Value.GetType());
        AtlyssDropdown element = AddDropdown(label, enumNames, (int)(object)config.Value);
        element.OnValueChanged.AddListener(newValue => { config.Value = (T)(object)newValue; });

        return element;
    }

    public AtlyssDropdown AddDropdown(string label, string[] options, int value = 0) => AddDropdown(label, options.ToList(), value);

    public AtlyssDropdown AddDropdown(string label, List<string> options, int value = 0)
    {
        AtlyssDropdown element = AddDropdown(label);
        element.Dropdown.AddOptions(options);
        element.Dropdown.SetValueWithoutNotify(value);
        element.Apply();

        PushElement(element);
        return element;
    }

    public AtlyssDropdown AddDropdown(string label, List<Sprite> options, int value = 0)
    {
        AtlyssDropdown element = AddDropdown(label);
        element.Dropdown.AddOptions(options);
        element.Dropdown.SetValueWithoutNotify(value);
        element.Apply();

        PushElement(element);
        return element;
    }

    public AtlyssDropdown AddDropdown(string label)
    {
        AtlyssDropdown element = TemplateManager.CreateDropdown(Content);
        Settings.OnCloseSettings.AddListener(() => element.Revert());
        Settings.OnApplySettings.AddListener(() => element.Apply());

        element.Label.text = label;
        element.Root.gameObject.SetActive(true);

        PushElement(element);
        return element;
    }

    public AtlyssKeyButton AddKeyButton(ConfigEntry<KeyCode> config) => AddKeyButton(config.Definition.Key, config);

    public AtlyssKeyButton AddKeyButton(string label, ConfigEntry<KeyCode> config)
    {
        AtlyssKeyButton element = AddKeyButton(label, config.Value);
        element.OnValueChanged.AddListener(newValue => { config.Value = newValue; });

        return element;
    }

    public AtlyssKeyButton AddKeyButton(string label, KeyCode value = KeyCode.None)
    {
        AtlyssKeyButton element = TemplateManager.CreateKeyButton(Content);
        Settings.OnCloseSettings.AddListener(() => element.Revert());
        Settings.OnApplySettings.AddListener(() => element.Apply());

        element.Label.text = label;
        element.SetValue(value);
        element.Apply();
        element.Root.gameObject.SetActive(true);

        RegisterKeyButton(element);

        PushElement(element);
        return element;
    }

    /// <summary>
    /// Prepares and pushes a KeyBindButton such that we're able to listen for changes in the SettingsManager.
    /// Pushing the KeyBindButton also makes it handle waiting for the user input.
    /// </summary>
    /// <param name="element"></param>
    private static void RegisterKeyButton(AtlyssKeyButton element)
    {
        SettingsManager manager = SettingsManager._current;

        var keyButton = new KeyBindButton
        {
            _inputReference = "", // Only used to associate native Atlyss key buttons with InputControlManager keys.
            _keyBind = element.ButtonLabel.text,
            _button = element.Button,
            _buttonLabel = element.ButtonLabel
        };

        int newButtonIndex = manager.keyBindButtons.Length;
        keyButton._button.onClick.AddListener(() => manager.OnClick_KeybindButton(newButtonIndex));
        KeyButtonIndexToKeyButton.Add(newButtonIndex, element);

        // Reverts the key bind in ATLYSS' keyBindButtons array, otherwise the displayed value will be overriden.
        Settings.OnCloseSettings.AddListener(() => keyButton._keyBind = element.ButtonLabel.text);

        manager.keyBindButtons = manager.keyBindButtons.AddToArray(keyButton);
    }

    private void PushElement(BaseAtlyssElement element)
    {
        ContentElements.Add(element);

        BottomSpace?.Root.SetSiblingIndex(Content.childCount);
    }
}