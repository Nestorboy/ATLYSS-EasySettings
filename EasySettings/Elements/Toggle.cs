using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssToggle : BaseAtlyssLabelElement, IValueElement
{
    public Toggle Toggle
    {
        get;
        set
        {
            if (field) field.onValueChanged.RemoveListener(ValueChanged);
            field = value;
            if (field) field.onValueChanged.AddListener(ValueChanged);
        }
    }

    public UnityEvent<bool> OnValueChanged { get; } = new();

    public bool AppliedValue { get; private set; }

    public void Initialize()
    {
        LabelText = "Toggle";

        Toggle.onValueChanged.RemoveAndDisableAllListeners();
        Toggle.onValueChanged.AddListener(ValueChanged);
        Toggle.SetIsOnWithoutNotify(AppliedValue);
    }

    public void Apply() => AppliedValue = Toggle.isOn;

    public void Revert() => Toggle.isOn = AppliedValue;

    private void ValueChanged(bool newValue) => OnValueChanged.Invoke(newValue);
}