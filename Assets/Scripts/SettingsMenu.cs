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

    private void Start()
    {
        root.AddGestureHandler<Gesture.OnHover, ToggleVisuals>(ToggleVisuals.HandleHover);
        root.AddGestureHandler<Gesture.OnUnhover, ToggleVisuals>(ToggleVisuals.HandleUnhover);
        root.AddGestureHandler<Gesture.OnPress, ToggleVisuals>(ToggleVisuals.HandlePress);
        root.AddGestureHandler<Gesture.OnRelease, ToggleVisuals>(ToggleVisuals.HandleRelease);

        root.AddGestureHandler<Gesture.OnClick, ToggleVisuals>(HandleToggleClicked);
        root.AddGestureHandler<Gesture.OnDrag, SliderVisuals>(HandleSliderDragged);

        //temporary
        BindToggle(boolSetting, toggleItemView.Visuals as ToggleVisuals);
        BindSlider(floatSetting, sliderItemView.Visuals as SliderVisuals);
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
