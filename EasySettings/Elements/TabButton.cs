using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssTabButton : BaseAtlyssElement
{
    public Text Label;

    private Button _resetButton;

    public Button Button
    {
        get => _resetButton;
        set
        {
            if (_resetButton) _resetButton.onClick.RemoveListener(Clicked);
            _resetButton = value;
            if (_resetButton) _resetButton.onClick.AddListener(Clicked);
        }
    }

    public UnityEvent OnClicked { get; } = new();

    public void Initialize()
    {
        Label.text = "Tab";

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(Clicked);
    }

    private void Clicked() => OnClicked.Invoke();
}