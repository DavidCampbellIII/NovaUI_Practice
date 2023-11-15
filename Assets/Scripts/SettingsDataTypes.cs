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
