using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.Prefabs;

internal static class TextInputPrefab
{
    public static RectTransform Create()
    {
        var obj = CreateRoot(out var inputField, out var label);

        List<Component> compRefs = obj.AddComponent<ComponentReferences>().components;
        compRefs.Add(inputField);
        compRefs.Add(label);

        return obj.GetComponent<RectTransform>();
    }

    private static GameObject CreateRoot(out InputField inputField, out Text label)
    {
        var obj = new GameObject("_easySettings_textField");

        var rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 545);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        var image = obj.AddComponent<Image>();
        image.color = new Color(0.4622f, 0.4622f, 0.4622f);
        image.raycastTarget = true;
        image.maskable = true;
        image.sprite = Resources.Load<Sprite>("_graphic/_ui/bk_04");
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
        var obj = new GameObject("_easySettings_inputField");
        obj.transform.SetParent(root.transform);
        
        var inputFieldText = CreateText(obj);
        var inputFieldPlaceholder = CreatePlaceholder(obj);

        var rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 180);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        var image = obj.AddComponent<Image>();
        image.color = new Color(0.5943f, 0.5943f, 0.5943f, 0.8078f);
        image.raycastTarget = true;
        image.maskable = true;
        image.sprite = Resources.Load<Sprite>("_graphic/_ui/uiBox03");
        image.type = Image.Type.Sliced;
        image.fillCenter = true;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillAmount = 1;
        image.fillClockwise = true;
        image.pixelsPerUnitMultiplier = 1;

        inputField = obj.AddComponent<InputField>();
        inputField.transition = Selectable.Transition.ColorTint;
        inputField.colors = new ColorBlock()
        {
            normalColor = new Color(1f, 0.3632f, 0.3632f),
            highlightedColor = new Color(0.9607f, 0.9607f, 0.9607f),
            pressedColor = new Color(0.78431f, 0.78431f, 0.78431f),
            selectedColor = new Color(0.9607f, 0.9607f, 0.9607f),
            disabledColor = new Color(0.78431f, 0.78431f, 0.78431f),
            colorMultiplier = 1f,
            fadeDuration = 0.1f
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
        inputField.contentType = UnityEngine.UI.InputField.ContentType.Standard;
        inputField.inputType = UnityEngine.UI.InputField.InputType.Standard;
        inputField.asteriskChar = '*';
        inputField.keyboardType = TouchScreenKeyboardType.Default;
        inputField.lineType = UnityEngine.UI.InputField.LineType.SingleLine;
        inputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.None;
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
        var obj = new GameObject("_easySettings_inputFieldLabel");
        obj.transform.SetParent(root.transform);

        var rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0, 0.5f);
        rectTransform.anchorMin = new Vector2(0, 0.5f);
        rectTransform.anchorMax = new Vector2(0, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 6, 260);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        var text = label = obj.AddComponent<Text>();
        text.color = new Color(1f, 0.5141f, 0.5141f);
        text.maskable = true;
        text.font = Resources.Load<Font>("_graphic/_font/terminal-grotesque");
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
        var obj = new GameObject("_easySettings_inputFieldText");
        obj.transform.SetParent(root.transform);

        var rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0, 0.5f);
        rectTransform.anchorMin = new Vector2(0, 0.5f);
        rectTransform.anchorMax = new Vector2(0, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 6, 260);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        var text = obj.AddComponent<Text>();
        text.color = new Color(1f, 0.5137f, 0.5137f);
        text.maskable = true;
        text.font = Resources.Load<Font>("_graphic/_font/terminal-grotesque");
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
        var obj = new GameObject("_easySettings_inputFieldPlaceholder");
        obj.transform.SetParent(root.transform);

        var rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 6, 180);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        var text = obj.AddComponent<Text>();
        text.color = new Color(0.8207f, 0.3677f, 0.3677f);
        text.maskable = true;
        text.font = Resources.Load<Font>("_graphic/_font/terminal-grotesque");
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
