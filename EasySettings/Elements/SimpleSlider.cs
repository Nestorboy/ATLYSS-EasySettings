using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssSimpleSlider : BaseAtlyssLabelElement, IValueElement
{
    private Slider _slider;

    public Slider Slider
    {
        get => _slider;
        set
        {
            if (_slider) _slider.onValueChanged.RemoveListener(ValueChanged);
            _slider = value;
            if (_slider) _slider.onValueChanged.AddListener(ValueChanged);
        }
    }

    public UnityEvent<float> OnValueChanged { get; } = new();

    public float AppliedValue { get; private set; }

    public void Initialize()
    {
        LabelText = "Slider";

        Slider.onValueChanged.RemoveAndDisableAllListeners();
        Slider.wholeNumbers = false;
        Slider.minValue = 0f;
        Slider.maxValue = 1f;
        Slider.SetValueWithoutNotify(AppliedValue);
    }

    public void Apply() => AppliedValue = _slider.value;

    public void Revert() => _slider.value = AppliedValue;

    private void ValueChanged(float newValue) => OnValueChanged.Invoke(newValue);
}