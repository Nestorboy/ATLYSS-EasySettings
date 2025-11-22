using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.Prefabs;

internal static class TextInputPrefab
{
    private const string FONT_PATH = "_graphic/_font/terminal-grotesque";
    private const string BACKGROUND_SPRITE_PATH = "_graphic/_ui/bk_04";
    private const string INPUT_SPRITE_PATH = "_graphic/_ui/uiBox03";

    public static RectTransform Create()
    {
        GameObject obj = CreateRoot(out InputField inputField, out Text label);

        List<Component> compRefs = obj.AddComponent<ComponentReferences>().components;
        compRefs.Add(inputField);
        compRefs.Add(label);

        return obj.transform as RectTransform;
    }

    private static GameObject CreateRoot(out InputField inputField, out Text label)
    {
        GameObject obj = new("_easySettings_textField");

        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 545);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        CanvasRenderer canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        Image image = obj.AddComponent<Image>();
        image.color = new Color(0.4622f, 0.4622f, 0.4622f);
        image.raycastTarget = true;
        image.maskable = true;
        image.sprite = Resources.Load<Sprite>(BACKGROUND_SPRITE_PATH);
        image.type = Image.Type.Sliced;
        image.fillCenter = true;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillAmount = 1;
        image.fillClockwise = true;
        image.pixelsPerUnitMultiplier = 1;

        CreateInputField(obj, out inputField);
        CreateLabel(obj, out label);

        return obj;
    }

    private static GameObject CreateInputField(GameObject root, out InputField inputField)
    {
        GameObject obj = new("_easySettings_inputField");
        obj.transform.SetParent(root.transform);

        GameObject inputFieldText = CreateText(obj);
        GameObject inputFieldPlaceholder = CreatePlaceholder(obj);

        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 180);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        CanvasRenderer canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        Image image = obj.AddComponent<Image>();
        image.color = new Color(0.5943f, 0.5943f, 0.5943f, 0.8078f);
        image.raycastTarget = true;
        image.maskable = true;
        image.sprite = Resources.Load<Sprite>(INPUT_SPRITE_PATH);
        image.type = Image.Type.Sliced;
        image.fillCenter = true;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillAmount = 1;
        image.fillClockwise = true;
        image.pixelsPerUnitMultiplier = 1;

        inputField = obj.AddComponent<InputField>();
        inputField.transition = Selectable.Transition.ColorTint;
        inputField.colors = new ColorBlock
        {
            normalColor = new Color(1f, 0.3632f, 0.3632f),
            highlightedColor = new Color(0.9607f, 0.9607f, 0.9607f),
            pressedColor = new Color(0.78431f, 0.78431f, 0.78431f),
            selectedColor = new Color(0.9607f, 0.9607f, 0.9607f),
            disabledColor = new Color(0.78431f, 0.78431f, 0.78431f),
            colorMultiplier = 1f,
            fadeDuration = 0.1f,
        };
        inputField.animationTriggers.normalTrigger = "Normal";
        inputField.animationTriggers.highlightedTrigger = "Highlighted";
        inputField.animationTriggers.pressedTrigger = "Pressed";
        inputField.animationTriggers.selectedTrigger = "Selected";
        inputField.animationTriggers.disabledTrigger = "Disabled";
        inputField.interactable = true;
        inputField.targetGraphic = image;
        inputField.textComponent = inputFieldText.GetComponent<Text>();
        inputField.placeholder = inputFieldPlaceholder.GetComponent<Text>();
        inputField.contentType = InputField.ContentType.Standard;
        inputField.inputType = InputField.InputType.Standard;
        inputField.asteriskChar = '*';
        inputField.keyboardType = TouchScreenKeyboardType.Default;
        inputField.lineType = InputField.LineType.SingleLine;
        inputField.characterValidation = InputField.CharacterValidation.None;
        inputField.characterLimit = 12;
        inputField.caretColor = new Color(0.6039f, 0.8431f, 0.9607f);
        inputField.customCaretColor = true;
        inputField.selectionColor = new Color(0.6588f, 0.8078f, 1f);
        inputField.caretBlinkRate = 4;
        inputField.caretWidth = 5;
        inputField.readOnly = false;
        inputField.shouldActivateOnSelect = true;

        return obj;
    }

    private static GameObject CreateLabel(GameObject root, out Text label)
    {
        GameObject obj = new("_easySettings_inputFieldLabel");
        obj.transform.SetParent(root.transform);

        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0, 0.5f);
        rectTransform.anchorMin = new Vector2(0, 0.5f);
        rectTransform.anchorMax = new Vector2(0, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 6, 260);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        CanvasRenderer canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        Text text = label = obj.AddComponent<Text>();
        text.color = new Color(1f, 0.5141f, 0.5141f);
        text.maskable = true;
        text.font = Resources.Load<Font>(FONT_PATH);
        text.fontSize = 22;
        text.fontStyle = FontStyle.Normal;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = 40;
        text.alignment = TextAnchor.MiddleLeft;
        text.supportRichText = true;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.lineSpacing = 1;
        text.text = "Label";

        return obj;
    }

    private static GameObject CreateText(GameObject root)
    {
        GameObject obj = new("_easySettings_inputFieldText");
        obj.transform.SetParent(root.transform);

        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0, 0.5f);
        rectTransform.anchorMin = new Vector2(0, 0.5f);
        rectTransform.anchorMax = new Vector2(0, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 6, 260);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        CanvasRenderer canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        Text text = obj.AddComponent<Text>();
        text.color = new Color(1f, 0.5137f, 0.5137f);
        text.maskable = true;
        text.font = Resources.Load<Font>(FONT_PATH);
        text.fontSize = 22;
        text.fontStyle = FontStyle.Normal;
        text.resizeTextMinSize = 2;
        text.resizeTextMaxSize = 42;
        text.alignment = TextAnchor.MiddleLeft;
        text.supportRichText = false;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.lineSpacing = 1;
        text.text = "Text";

        return obj;
    }

    private static GameObject CreatePlaceholder(GameObject root)
    {
        GameObject obj = new("_easySettings_inputFieldPlaceholder");
        obj.transform.SetParent(root.transform);

        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 6, 180);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        CanvasRenderer canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        Text text = obj.AddComponent<Text>();
        text.color = new Color(0.8207f, 0.3677f, 0.3677f);
        text.maskable = true;
        text.font = Resources.Load<Font>(FONT_PATH);
        text.fontSize = 21;
        text.fontStyle = FontStyle.Italic;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = 40;
        text.alignment = TextAnchor.MiddleLeft;
        text.supportRichText = true;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.lineSpacing = 1;
        text.text = "Placeholder";

        return obj;
    }
}