using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropdownItemVisuals : ItemVisuals
{
    public TextBlock label;
    public UIBlock2D background;
    public UIBlock2D selectedIndicator;
}

[System.Serializable]
public class DropdownVisuals : ItemVisuals
{
    public TextBlock label;
    public TextBlock selectedLabel;
    public UIBlock2D background;
    public UIBlock expandedRoot;
    public ListView optionsList;

    public Color defaultColor;
    public Color hoveredColor;
    public Color pressedColor;

    public Color primaryRowColor;
    public Color secondaryRowColor;

    public bool IsExpanded => expandedRoot.gameObject.activeSelf;

    private MultiOptionSetting dataSource;
    private bool eventHandlersRegistered = false;

    #region Gesture Handlers

    internal static void HandleHover(Gesture.OnHover evt, DropdownVisuals target)
    {
        //don't change color if the pointer is over the expanded root
        if(evt.Receiver.transform.IsChildOf(target.expandedRoot.transform))
        {
            return;
        }

        target.background.Color = target.hoveredColor;
    }

    internal static void HandlePress(Gesture.OnPress evt, DropdownVisuals target)
    {
        //don't change color if the pointer is over the expanded root
        if (evt.Receiver.transform.IsChildOf(target.expandedRoot.transform))
        {
            return;
        }

        target.background.Color = target.pressedColor;
    }

    internal static void HandleRelease(Gesture.OnRelease evt, DropdownVisuals target)
    {
        //don't change color if the pointer is over the expanded root
        if (evt.Receiver.transform.IsChildOf(target.expandedRoot.transform))
        {
            return;
        }

        target.background.Color = target.hoveredColor;
    }

    internal static void HandleUnhover(Gesture.OnUnhover evt, DropdownVisuals target)
    {
        //don't change color if the pointer is over the expanded root
        if (evt.Receiver.transform.IsChildOf(target.expandedRoot.transform))
        {
            return;
        }

        target.background.Color = target.defaultColor;
    }

    #endregion

    public void Collapse()
    {
        expandedRoot.gameObject.SetActive(false);
    }

    public void Expand(MultiOptionSetting dataSource)
    {
        this.dataSource = dataSource;
        EnsureEventHandlers();

        expandedRoot.gameObject.SetActive(true);
        optionsList.SetDataSource(dataSource.options);
        optionsList.JumpToIndex(dataSource.selectedIndex);
    }

    private void EnsureEventHandlers()
    {
        if(eventHandlersRegistered)
        {
            return;
        }

        eventHandlersRegistered = true;

        optionsList.AddGestureHandler<Gesture.OnHover, DropdownItemVisuals>(HandleItemHovered);
        optionsList.AddGestureHandler<Gesture.OnUnhover, DropdownItemVisuals>(HandleItemUnhovered);
        optionsList.AddGestureHandler<Gesture.OnPress, DropdownItemVisuals>(HandleItemPressed);
        optionsList.AddGestureHandler<Gesture.OnRelease, DropdownItemVisuals>(HandleItemReleased);

        optionsList.AddGestureHandler<Gesture.OnClick, DropdownItemVisuals>(HandleItemClicked);
        optionsList.AddDataBinder<string, DropdownItemVisuals>(BindItem);
    }

    private void BindItem(Data.OnBind<string> evt, DropdownItemVisuals target, int index)
    {
        target.label.Text = evt.UserData;
        target.selectedIndicator.gameObject.SetActive(index == dataSource.selectedIndex);
        target.background.Color = index % 2 == 0 ? primaryRowColor : secondaryRowColor;
    }

    private void HandleItemClicked(Gesture.OnClick evt, DropdownItemVisuals target, int index)
    {
        dataSource.selectedIndex = index;
        selectedLabel.Text = dataSource.CurrentSelection;
        evt.Consume();
        Collapse();
    }

    private void HandleItemReleased(Gesture.OnRelease evt, DropdownItemVisuals target, int index)
    {
        target.background.Color = hoveredColor;
    }

    private void HandleItemPressed(Gesture.OnPress evt, DropdownItemVisuals target, int index)
    {
        target.background.Color = pressedColor;
    }

    private void HandleItemUnhovered(Gesture.OnUnhover evt, DropdownItemVisuals target, int index)
    {
        target.background.Color = index % 2 == 0 ? primaryRowColor : secondaryRowColor;
    }

    private void HandleItemHovered(Gesture.OnHover evt, DropdownItemVisuals target, int index)
    {
        target.background.Color = hoveredColor;
    }
}
