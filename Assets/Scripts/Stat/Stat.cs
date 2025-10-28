using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;
    public List<int> modifiers;
    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int val in modifiers)
        {
            finalValue += val;
        }
        return finalValue;
    }
    public void SetDefaultValue(int val)
    {
        baseValue = val;
    }
    public void AddModifer(int _modifier)
    {
        modifiers.Add(_modifier);
    }
    public void RemoveModifer(int _modifier)
    {
        modifiers.RemoveAt(_modifier) ;
    }
}
