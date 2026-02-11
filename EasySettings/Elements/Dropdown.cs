using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssDropdown : BaseAtlyssLabelElement, IValueElement
{
    public Dropdown Dropdown
    {
        get;
        set
        {
            if (field) field.onValueChanged.RemoveListener(ValueChanged);
            field = value;
            if (field) field.onValueChanged.AddListener(ValueChanged);
        }
    }

    public UnityEvent<int> OnValueChanged { get; } = new();

    public int AppliedValue { get; private set; }

    public void Initialize()
    {
        LabelText = "Dropdown";

        Dropdown.onValueChanged.RemoveAndDisableAllListeners();
        Dropdown.onValueChanged.AddListener(ValueChanged);
        Dropdown.options.Clear();
        Dropdown.SetValueWithoutNotify(AppliedValue);
    }

    public void Apply() => AppliedValue = Dropdown.value;

    public void Revert() => Dropdown.value = AppliedValue;

    private void ValueChanged(int newValue) => OnValueChanged.Invoke(newValue);
}