using UnityEngine.Events;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public class AtlyssTabButton : BaseAtlyssElement
{
    public Text Label;

    public Button Button
    {
        get;
        set
        {
            if (field) field.onClick.RemoveListener(Clicked);
            field = value;
            if (field) field.onClick.AddListener(Clicked);
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