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

    private void Start()
    {
        root.AddGestureHandler<Gesture.OnHover, ToggleVisuals>(ToggleVisuals.HandleHover);
        root.AddGestureHandler<Gesture.OnUnhover, ToggleVisuals>(ToggleVisuals.HandleUnhover);
        root.AddGestureHandler<Gesture.OnPress, ToggleVisuals>(ToggleVisuals.HandlePress);
        root.AddGestureHandler<Gesture.OnRelease, ToggleVisuals>(ToggleVisuals.HandleRelease);

        root.AddGestureHandler<Gesture.OnClick, ToggleVisuals>(HandleToggleClicked);

        //temporary
        BindToggle(boolSetting, toggleItemView.Visuals as ToggleVisuals);
    }

    private void HandleToggleClicked(Gesture.OnClick evt, ToggleVisuals target)
    {
        boolSetting.state = !boolSetting.state;
        target.IsChecked = boolSetting.state;
    }

    private void BindToggle(BoolSetting boolSetting, ToggleVisuals visuals)
    {
        visuals.label.Text = boolSetting.name;
        visuals.IsChecked = boolSetting.state;
    }
}
