using UnityEngine;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.UIElements;

public abstract class BaseAtlyssElement
{
    public RectTransform Root;
}

public abstract class BaseAtlyssLabelElement : BaseAtlyssElement
{
    public Text Label { get; set; }

    public string LabelText { get => Label.text; set => Label.text = value; }
}