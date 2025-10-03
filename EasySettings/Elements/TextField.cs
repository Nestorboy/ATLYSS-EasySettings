using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements
{
    public class AtlyssTextField : BaseAtlyssLabelElement, IValueElement
    {
        private InputField _inputField;
        public InputField InputField
        {
            get => _inputField;
            set
            {
                if (_inputField != null) _inputField.onEndEdit.RemoveListener(ValueChanged);

                _inputField = value;

                if (_inputField != null)
                {
                    _inputField.onEndEdit.AddListener(ValueChanged);
                    _inputField.text = AppliedValue;
                }
            }
        }

        public Text Placeholder;
        public UnityEvent<string> OnValueChanged { get; } = new();

        public string AppliedValue { get; private set; } = string.Empty;

        public void Initialize()
        {
            LabelText = "Text Field";
            if (Placeholder != null)
                Placeholder.text = "Text..";

            if (InputField != null)
            {
                InputField.onEndEdit.RemoveListener(ValueChanged);
                InputField.onEndEdit.AddListener(ValueChanged);
                InputField.text = AppliedValue;
            }
        }

        public void SetValue(string value)
        {
            if (InputField != null) InputField.text = value;
        }

        public void Apply() => AppliedValue = InputField.text;

        public void Revert() => InputField.text = AppliedValue;

        private void ValueChanged(string newValue) => OnValueChanged.Invoke(newValue);
    }
}