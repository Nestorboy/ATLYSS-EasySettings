using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssAdvancedSlider : BaseAtlyssLabelElement, IValueElement
{
    public Text ValueText;

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

    public Button ResetButton
    {
        get;
        set
        {
            if (field) field.onClick.RemoveListener(ResetClicked);
            field = value;
            if (field) field.onClick.AddListener(ResetClicked);
        }
    }

    public UnityEvent<float> OnValueChanged { get; } = new();
    public UnityEvent OnResetClicked { get; } = new();

    public float AppliedValue { get; private set; }

    public void Initialize()
    {
        LabelText = "Advanced Slider";
        ValueText.text = $"{AppliedValue}";

        Slider.onValueChanged.RemoveAndDisableAllListeners();
        Slider.onValueChanged.AddListener(ValueChanged);
        Slider.wholeNumbers = false;
        Slider.minValue = 0f;
        Slider.maxValue = 1f;
        Slider.SetValueWithoutNotify(AppliedValue);

        ResetButton.onClick.RemoveAndDisableAllListeners();
        ResetButton.onClick.AddListener(ResetClicked);
    }

    public void SetValue(float value) => _slider.value = value;

    public void Apply() => AppliedValue = _slider.value;

    public void Revert() => _slider.value = AppliedValue;

    private void ValueChanged(float newValue) => OnValueChanged.Invoke(newValue);
    private void ResetClicked() => OnResetClicked.Invoke();
}