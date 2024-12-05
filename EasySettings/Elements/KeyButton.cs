using System;
using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssKeyButton : BaseAtlyssLabelElement, IValueElement
{
    public Text ButtonLabel;

    private Button _button;

    public Button Button
    {
        get => _button;
        set
        {
            if (_button) _button.onClick.RemoveListener(Clicked);
            _button = value;
            if (_button) _button.onClick.RemoveListener(Clicked);
        }
    }

    public UnityEvent<KeyCode> OnValueChanged { get; } = new();

    public UnityEvent OnClicked { get; } = new();

    public KeyCode AppliedValue { get; private set; }

    public void Initialize()
    {
        Label.text = "Key Button";
        ButtonLabel.text = AppliedValue.ToString();

        Button.onClick.RemoveAndDisableAllListeners();
        Button.onClick.AddListener(Clicked);
    }

    public void SetValue(KeyCode key)
    {
        ButtonLabel.text = key.ToString();
        ValueChanged(key);
    }

    public void Apply()
    {
        if (!Enum.TryParse(ButtonLabel.text, true, out KeyCode key))
        {
            key = KeyCode.None;
        }

        AppliedValue = key;
    }

    public void Revert() => SetValue(AppliedValue);

    private void ValueChanged(KeyCode newValue) => OnValueChanged.Invoke(newValue);

    private void Clicked() => OnClicked.Invoke();
}