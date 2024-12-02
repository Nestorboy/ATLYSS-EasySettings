using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssKeyButton : BaseAtlyssLabelElement
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

    public void Initialize()
    {
        Label.text = "Key Button";
        ButtonLabel.text = KeyCode.None.ToString();

        Button.onClick.RemoveAndDisableAllListeners();
        Button.onClick.AddListener(Clicked);
    }

    public void SetValue(KeyCode key)
    {
        ButtonLabel.text = key.ToString();
        ValueChanged(key);
    }

    private void ValueChanged(KeyCode newValue) => OnValueChanged.Invoke(newValue);

    private void Clicked() => OnClicked.Invoke();
}