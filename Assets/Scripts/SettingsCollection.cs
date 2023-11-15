using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Setting/Collection")]
public class SettingsCollection : ScriptableObject
{
    public string category;

    [SerializeReference, TypeSelector]
    public List<Setting> settings = new List<Setting>();
}

[AttributeUsage(AttributeTargets.Field)]
public class TypeSelectorAttribute : PropertyAttribute { }
