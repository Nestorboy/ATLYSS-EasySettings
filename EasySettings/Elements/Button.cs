using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssButton : BaseAtlyssElement
{
    public Text ButtonLabel;

    private Button _button;

    public Button Button
    {
        get => _button;
        set
        {
            if (_button) _button.onClick.RemoveListener(Clicked);
            _button = value;
            if (_button) _button.onClick.RemoveListener(Clicked);
        }
    }

    public UnityEvent OnClicked { get; } = new();

    public void Initialize()
    {
        ButtonLabel.text = "Button";

        Button.onClick.RemoveAndDisableAllListeners();
        Button.onClick.AddListener(Clicked);
    }

    private void Clicked() => OnClicked.Invoke();
}