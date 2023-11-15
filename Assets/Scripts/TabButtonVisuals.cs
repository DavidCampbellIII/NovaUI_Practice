using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TabButtonVisuals : ItemVisuals
{
    public TextBlock label;
    public UIBlock2D background;
    public UIBlock2D selectedIndicator;

    public Color defaultColor;
    public Color selectedColor;

    public Color defaultGradientColor;
    public Color hoveredGradientColor;
    public Color pressedGradientColor;

    public bool IsSelected
    {
        get => selectedIndicator.gameObject.activeSelf;
        set
        {
            selectedIndicator.gameObject.SetActive(value);
            background.Color = value ? selectedColor : defaultColor;
        }
    }

    internal static void HandleHover(Gesture.OnHover evt, TabButtonVisuals target, int index)
    {
        target.background.Gradient.Color = target.hoveredGradientColor;
    }

    internal static void HandlePress(Gesture.OnPress evt, TabButtonVisuals target, int index)
    {
        target.background.Gradient.Color = target.pressedGradientColor;
    }

    internal static void HandleRelease(Gesture.OnRelease evt, TabButtonVisuals target, int index)
    {
        target.background.Gradient.Color = target.hoveredGradientColor;
    }

    internal static void HandleUnhover(Gesture.OnUnhover evt, TabButtonVisuals target, int index)
    {
        target.background.Gradient.Color = target.defaultGradientColor;
    }
}
