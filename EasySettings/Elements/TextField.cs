using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements
{
    public class AtlyssTextField : BaseAtlyssLabelElement, IValueElement
    {
        public InputField InputField
        {
            get;
            set
            {
                if (field != null) field.onEndEdit.RemoveListener(ValueChanged);

                field = value;

                if (field != null)
                {
                    field.onEndEdit.AddListener(ValueChanged);
                    field.text = AppliedValue;
                }
            }
        }

        public Text Placeholder;
        public UnityEvent<string> OnValueChanged { get; } = new();

        public string AppliedValue { get; private set; } = string.Empty;

        public void Initialize()
        {
            LabelText = "Text Field";

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