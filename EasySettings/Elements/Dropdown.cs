using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssDropdown : BaseAtlyssLabelElement
{
    private Dropdown _dropdown;

    public Dropdown Dropdown
    {
        get => _dropdown;
        set
        {
            if (_dropdown) _dropdown.onValueChanged.RemoveListener(ValueChanged);
            _dropdown = value;
            if (_dropdown) _dropdown.onValueChanged.AddListener(ValueChanged);
        }
    }

    public UnityEvent<int> OnValueChanged { get; } = new();

    public void Initialize()
    {
        Label.text = "Dropdown";

        Dropdown.onValueChanged.RemoveAndDisableAllListeners();
        Dropdown.onValueChanged.AddListener(ValueChanged);
        Dropdown.options.Clear();
        Dropdown.SetValueWithoutNotify(0);
    }

    private void ValueChanged(int newValue) => OnValueChanged.Invoke(newValue);
}