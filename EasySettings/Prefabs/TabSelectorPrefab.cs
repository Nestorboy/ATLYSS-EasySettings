using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nessie.ATLYSS.EasySettings.Prefabs;

internal static class TabSelectorPrefab
{
    private const string FONT_PATH = "_graphic/_font/terminal-grotesque";
    private const string BACKGROUND_SPRITE_PATH = "_graphic/_ui/bk_04";
    private const string INDICATOR_SPRITE_PATH = "_graphic/_ui/uiSprite_playerIndicator";

    internal static RectTransform Create()
    {
        GameObject obj = CreateRoot(out Button leftButton, out Button rightButton, out Text label);

        List<Component> compRefs = obj.AddComponent<ComponentReferences>().components;
        compRefs.Add(leftButton);
        compRefs.Add(rightButton);
        compRefs.Add(label);

        return obj.transform as RectTransform;
    }
    
    private static GameObject CreateRoot(out Button leftButton, out Button rightButton, out Text label)
    {
        GameObject obj = new("EasySettings TabSelector");

        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 545);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        CanvasRenderer canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        Image image = obj.AddComponent<Image>();
        image.color = new Color(0.75f, 0.75f, 1f);
        image.raycastTarget = true;
        image.maskable = true;
        image.sprite = Resources.Load<Sprite>(BACKGROUND_SPRITE_PATH);
        image.type = Image.Type.Sliced;
        image.fillCenter = true;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillAmount = 1;
        image.fillClockwise = true;
        image.pixelsPerUnitMultiplier = 1;

        CreateButton(obj, out leftButton, true);
        CreateLabel(obj, out label);
        CreateButton(obj, out rightButton, false);

        return obj;
    }

    private static GameObject CreateButton(GameObject root, out Button button, bool isLeftOriented)
    {
        GameObject obj = new("Button");
        obj.transform.SetParent(root.transform);

        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.SetInsetAndSizeFromParentEdge(isLeftOriented ? RectTransform.Edge.Left : RectTransform.Edge.Right, 6, 30);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
        rectTransform.localRotation = Quaternion.Euler(0, 0, isLeftOriented ? -90 : 90); // Sprite normally points downward

        CanvasRenderer canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;
        
        Image image = obj.AddComponent<Image>();
        image.color = Color.white;
        image.raycastTarget = true;
        image.maskable = true;
        image.sprite = Resources.Load<Sprite>(INDICATOR_SPRITE_PATH);
        image.type = Image.Type.Filled;
        image.fillCenter = true;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillAmount = 1;
        image.fillClockwise = true;
        image.pixelsPerUnitMultiplier = 1;

        button = obj.AddComponent<Button>();
        button.transition = Selectable.Transition.ColorTint;
        button.colors = new ColorBlock
        {
            normalColor = Color.white,
            highlightedColor = new Color(0.9607f, 0.9607f, 0.9607f),
            pressedColor = new Color(0.78431f, 0.78431f, 0.78431f),
            selectedColor = new Color(0.9607f, 0.9607f, 0.9607f),
            disabledColor = new Color(0.78431f, 0.78431f, 0.78431f),
            colorMultiplier = 1f,
            fadeDuration = 0.1f,
        };
        button.animationTriggers.normalTrigger = "Normal";
        button.animationTriggers.highlightedTrigger = "Highlighted";
        button.animationTriggers.pressedTrigger = "Pressed";
        button.animationTriggers.selectedTrigger = "Selected";
        button.animationTriggers.disabledTrigger = "Disabled";
        button.targetGraphic = image;

        return obj;
    }
    
    private static GameObject CreateLabel(GameObject root, out Text label)
    {
        GameObject obj = new("Label");
        obj.transform.SetParent(root.transform);

        RectTransform rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 420);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        CanvasRenderer canvasRenderer = obj.AddComponent<CanvasRenderer>();
        canvasRenderer.cullTransparentMesh = true;

        Text text = label = obj.AddComponent<Text>();
        text.color = Color.white;
        text.maskable = true;
        text.font = Resources.Load<Font>(FONT_PATH);
        text.fontSize = 22;
        text.fontStyle = FontStyle.Normal;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = 40;
        text.alignment = TextAnchor.MiddleCenter;
        text.supportRichText = true;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.lineSpacing = 1;
        text.text = "Label";

        return obj;
    }
}