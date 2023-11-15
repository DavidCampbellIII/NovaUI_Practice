using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public UIBlock root;

    public List<SettingsCollection> settingsCollections;
    public ListView tabBar;
    public ListView settingsList;

    private int selectedTabIndex = -1;
    private List<Setting> currentSettings => settingsCollections[selectedTabIndex].settings;

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
        settingsList.AddGestureHandler<Gesture.OnClick, ToggleVisuals>(HandleToggleClicked);
        settingsList.AddGestureHandler<Gesture.OnDrag, SliderVisuals>(HandleSliderDragged);
        settingsList.AddGestureHandler<Gesture.OnClick, DropdownVisuals>(HandleDropdownClicked);

        //data binders
        settingsList.AddDataBinder<BoolSetting, ToggleVisuals>(BindToggle);
        settingsList.AddDataBinder<FloatSetting, SliderVisuals>(BindSlider);
        settingsList.AddDataBinder<MultiOptionSetting, DropdownVisuals>(BindDropdown);

        //tabs
        tabBar.AddDataBinder<SettingsCollection, TabButtonVisuals>(BindTab);
        tabBar.AddGestureHandler<Gesture.OnHover, TabButtonVisuals>(TabButtonVisuals.HandleHover);
        tabBar.AddGestureHandler<Gesture.OnUnhover, TabButtonVisuals>(TabButtonVisuals.HandleUnhover);
        tabBar.AddGestureHandler<Gesture.OnPress, TabButtonVisuals>(TabButtonVisuals.HandlePress);
        tabBar.AddGestureHandler<Gesture.OnRelease, TabButtonVisuals>(TabButtonVisuals.HandleRelease);
        tabBar.AddGestureHandler<Gesture.OnClick, TabButtonVisuals>(HandleTabClicked);

        //data source must be set AFTER data binder, so items can be bound correctly
        tabBar.SetDataSource(settingsCollections);

        //try to select the first tab
        if(tabBar.TryGetItemView(0, out ItemView firstTabItemView))
        {
            SelectTab(firstTabItemView.Visuals as TabButtonVisuals, 0);
        }
    }

    private void HandleTabClicked(Gesture.OnClick evt, TabButtonVisuals target, int index)
    {
        SelectTab(target, index);
    }

    private void SelectTab(TabButtonVisuals visuals, int index)
    {
        if(selectedTabIndex == index)
        {
            return;
        }

        if(selectedTabIndex != -1 && tabBar.TryGetItemView(selectedTabIndex, out ItemView oldTabItemView))
        {
            (oldTabItemView.Visuals as TabButtonVisuals).IsSelected = false;
        }

        visuals.IsSelected = true;
        selectedTabIndex = index;
        settingsList.SetDataSource(currentSettings);
    }

    private void HandleDropdownClicked(Gesture.OnClick evt, DropdownVisuals target, int index)
    {
        if(target.IsExpanded)
        {
            target.Collapse();
        }
        else
        {
            MultiOptionSetting multiOptionSetting = currentSettings[index] as MultiOptionSetting;
            target.Expand(multiOptionSetting);
        }
    }

    private void HandleSliderDragged(Gesture.OnDrag evt, SliderVisuals target, int index)
    {
        FloatSetting floatSetting = currentSettings[index] as FloatSetting;
        Vector3 currentPointerPos = evt.PointerPositions.Current;

        float localXPos = target.sliderBackground.transform.InverseTransformPoint(currentPointerPos).x;
        float sliderWidth = target.sliderBackground.CalculatedSize.X.Value;

        float distFromLeft = localXPos + sliderWidth / 2;
        float percFromLeft = Mathf.Clamp01(distFromLeft / sliderWidth);

        floatSetting.Value = floatSetting.min + (floatSetting.max - floatSetting.min) * percFromLeft;
        target.fillBar.Size.X.Percent = percFromLeft;
        target.valueLabel.Text = floatSetting.DisplayValue;
    }

    private void HandleToggleClicked(Gesture.OnClick evt, ToggleVisuals target, int index)
    {
        BoolSetting boolSetting = currentSettings[index] as BoolSetting;
        boolSetting.state = !boolSetting.state;
        target.IsChecked = boolSetting.state;
    }

    private void BindTab(Data.OnBind<SettingsCollection> evt, TabButtonVisuals target, int index)
    {
        target.label.Text = evt.UserData.category;
        target.IsSelected = false;
    }

    private void BindDropdown(Data.OnBind<MultiOptionSetting> evt, DropdownVisuals visuals, int index)
    {
        MultiOptionSetting multiOptionSetting = evt.UserData;
        visuals.label.Text = multiOptionSetting.name;
        visuals.selectedLabel.Text = multiOptionSetting.CurrentSelection;
        visuals.Collapse();
    }

    private void BindSlider(Data.OnBind<FloatSetting> evt, SliderVisuals visuals, int index)
    {
        FloatSetting floatSetting = evt.UserData;
        visuals.label.Text = floatSetting.name;
        visuals.valueLabel.Text = floatSetting.DisplayValue;
        visuals.fillBar.Size.X.Percent = (floatSetting.Value - floatSetting.min) / (floatSetting.max - floatSetting.min);
    }

    private void BindToggle(Data.OnBind<BoolSetting> evt, ToggleVisuals visuals, int index)
    {
        BoolSetting boolSetting = evt.UserData;
        visuals.label.Text = boolSetting.name;
        visuals.IsChecked = boolSetting.state;
    }
}
