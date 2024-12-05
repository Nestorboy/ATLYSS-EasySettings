using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssToggle : BaseAtlyssLabelElement, IValueElement
{
    private Toggle _toggle;

    public Toggle Toggle
    {
        get => _toggle;
        set
        {
            if (_toggle) _toggle.onValueChanged.RemoveListener(ValueChanged);
            _toggle = value; 
            if (_toggle) _toggle.onValueChanged.AddListener(ValueChanged);
        }
    }

    public UnityEvent<bool> OnValueChanged { get; } = new();

    public bool AppliedValue { get; private set; }

    public void Initialize()
    {
        LabelText = "Toggle";

        Toggle.onValueChanged.RemoveAndDisableAllListeners();
        Toggle.SetIsOnWithoutNotify(AppliedValue);
    }
    
    public void Apply() => AppliedValue = _toggle.isOn;

    public void Revert() => _toggle.isOn = AppliedValue;

    private void ValueChanged(bool newValue) => OnValueChanged.Invoke(newValue);
}