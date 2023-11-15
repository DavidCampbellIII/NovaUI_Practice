using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Setting
{
    public string name;
}

[System.Serializable]
public class BoolSetting : Setting
{
    public bool state;
}

[System.Serializable]
public class FloatSetting : Setting
{
    [SerializeField]
    private float value;
    public float min;
    public float max;

    public string valueFormat = "{0:0.0}";

    public float Value
    {
        get => Mathf.Clamp(value, min, max);
        set => this.value = Mathf.Clamp(value, min, max);
    }

    public string DisplayValue => string.Format(valueFormat, Value);
}

[System.Serializable]
public class MultiOptionSetting : Setting
{
    private const string NOTHING_SELECTED = "None";

    public string[] options;
    public int selectedIndex;

    public string CurrentSelection => selectedIndex >= 0 ? options[selectedIndex] : NOTHING_SELECTED;
}