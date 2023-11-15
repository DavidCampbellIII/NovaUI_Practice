using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToggleVisuals : ItemVisuals
{
    public TextBlock label = null;
    public UIBlock2D checkBox = null;
    public UIBlock2D checkMark = null;

    public Color defaultColor;
    public Color hoveredColor;
    public Color pressedColor;

    public bool IsChecked
    {
        get => checkMark.gameObject.activeSelf;
        set => checkMark.gameObject.SetActive(value);
    }

    internal static void HandleHover(Gesture.OnHover evt, ToggleVisuals target)
    {
        target.checkBox.Color = target.hoveredColor;
    }

    internal static void HandleUnhover(Gesture.OnUnhover evt, ToggleVisuals target)
    {
        target.checkBox.Color = target.defaultColor;
    }

    internal static void HandlePress(Gesture.OnPress evt, ToggleVisuals target)
    {
        target.checkBox.Color = target.pressedColor;
    }

    internal static void HandleRelease(Gesture.OnRelease evt, ToggleVisuals target)
    {
        target.checkBox.Color = target.hoveredColor;
    }
}