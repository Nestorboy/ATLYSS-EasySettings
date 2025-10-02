using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements
{
    public class AtlyssTextField : BaseAtlyssLabelElement, IValueElement
    {
        public InputField InputField;
        public Text Placeholder;
        public UnityEvent<string> OnValueChanged { get; } = new();

        public string AppliedValue { get; private set; }

        public void Initialize()
        {
            Label.text = "Text Field";
            Placeholder.text = "Placeholder";

            InputField.onEndEdit.RemoveAllListeners();
            InputField.onEndEdit.AddListener(ValueChanged);
            InputField.text = AppliedValue;
        }

        public void SetValue(string value) => InputField.text = value;

        public void Apply() => AppliedValue = InputField.text;

        public void Revert() => InputField.text = AppliedValue;

        private void ValueChanged(string newValue) => OnValueChanged.Invoke(newValue);
    }
}