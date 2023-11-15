using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public UIBlock root;

    public BoolSetting boolSetting = new BoolSetting();
    public ItemView toggleItemView;

    public FloatSetting floatSetting = new FloatSetting();
    public ItemView sliderItemView;

    public MultiOptionSetting multiOptionSetting = new MultiOptionSetting();
    public ItemView dropdownItemView;

    private void Start()
    {
        //visuals
        root.AddGestureHandler<Gesture.OnHover, ToggleVisuals>(ToggleVisuals.HandleHover);
        root.AddGestureHandler<Gesture.OnUnhover, ToggleVisuals>(ToggleVisuals.HandleUnhover);
        root.AddGestureHandler<Gesture.OnPress, ToggleVisuals>(ToggleVisuals.HandlePress);
        root.AddGestureHandler<Gesture.OnRelease, ToggleVisuals>(ToggleVisuals.HandleRelease);

        root.AddGestureHandler<Gesture.OnHover, DropdownVisuals>(DropdownVisuals.HandleHover);
        root.AddGestureHandler<Gesture.OnUnhover, DropdownVisuals>(DropdownVisuals.HandleUnhover);
        root.AddGestureHandler<Gesture.OnPress, DropdownVisuals>(DropdownVisuals.HandlePress);
        root.AddGestureHandler<Gesture.OnRelease, DropdownVisuals>(DropdownVisuals.HandleRelease);

        //state changing
        root.AddGestureHandler<Gesture.OnClick, ToggleVisuals>(HandleToggleClicked);
        root.AddGestureHandler<Gesture.OnDrag, SliderVisuals>(HandleSliderDragged);
        root.AddGestureHandler<Gesture.OnClick, DropdownVisuals>(HandleDropdownClicked);

        //temporary
        BindToggle(boolSetting, toggleItemView.Visuals as ToggleVisuals);
        BindSlider(floatSetting, sliderItemView.Visuals as SliderVisuals);
        BindDropdown(multiOptionSetting, dropdownItemView.Visuals as DropdownVisuals);
    }

    private void HandleDropdownClicked(Gesture.OnClick evt, DropdownVisuals target)
    {
        if(target.IsExpanded)
        {
            target.Collapse();
        }
        else
        {
            target.Expand(multiOptionSetting);
        }
    }

    private void HandleSliderDragged(Gesture.OnDrag evt, SliderVisuals target)
    {
        Vector3 currentPointerPos = evt.PointerPositions.Current;

        float localXPos = target.sliderBackground.transform.InverseTransformPoint(currentPointerPos).x;
        float sliderWidth = target.sliderBackground.CalculatedSize.X.Value;

        float distFromLeft = localXPos + sliderWidth / 2;
        float percFromLeft = Mathf.Clamp01(distFromLeft / sliderWidth);

        floatSetting.Value = floatSetting.min + (floatSetting.max - floatSetting.min) * percFromLeft;
        target.fillBar.Size.X.Percent = percFromLeft;
        target.valueLabel.Text = floatSetting.DisplayValue;
    }

    private void HandleToggleClicked(Gesture.OnClick evt, ToggleVisuals target)
    {
        boolSetting.state = !boolSetting.state;
        target.IsChecked = boolSetting.state;
    }

    private void BindDropdown(MultiOptionSetting multiOptionSetting, DropdownVisuals visuals)
    {
        visuals.label.Text = multiOptionSetting.name;
        visuals.selectedLabel.Text = multiOptionSetting.CurrentSelection;
        visuals.Collapse();
    }

    private void BindSlider(FloatSetting floatSetting, SliderVisuals visuals)
    {
        visuals.label.Text = floatSetting.name;
        visuals.valueLabel.Text = floatSetting.DisplayValue;
        visuals.fillBar.Size.X.Percent = (floatSetting.Value - floatSetting.min) / (floatSetting.max - floatSetting.min);
    }

    private void BindToggle(BoolSetting boolSetting, ToggleVisuals visuals)
    {
        visuals.label.Text = boolSetting.name;
        visuals.IsChecked = boolSetting.state;
    }
}
