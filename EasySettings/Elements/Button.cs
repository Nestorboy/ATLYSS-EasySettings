using Nessie.ATLYSS.EasySettings.Extensions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssButton : BaseAtlyssElement
{
    public Text ButtonLabel;

    public Button Button
    {
        get;
        set
        {
            if (field) field.onClick.RemoveListener(Clicked);
            field = value;
            if (field) field.onClick.RemoveListener(Clicked);
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