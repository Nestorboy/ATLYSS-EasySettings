using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssSimpleSlider : BaseAtlyssLabelElement, IValueElement
{
    public Slider Slider
    {
        get;
        set
        {
            if (field) field.onValueChanged.RemoveListener(ValueChanged);
            field = value;
            if (field) field.onValueChanged.AddListener(ValueChanged);
        }
    }

    public UnityEvent<float> OnValueChanged { get; } = new();

    public float AppliedValue { get; private set; }

    public void Initialize()
    {
        LabelText = "Slider";

        Slider.onValueChanged.RemoveAndDisableAllListeners();
        Slider.onValueChanged.AddListener(ValueChanged);
        Slider.wholeNumbers = false;
        Slider.minValue = 0f;
        Slider.maxValue = 1f;
        Slider.SetValueWithoutNotify(AppliedValue);
    }

    public void Apply() => AppliedValue = Slider.value;

    public void Revert() => Slider.value = AppliedValue;

    private void ValueChanged(float newValue) => OnValueChanged.Invoke(newValue);
}