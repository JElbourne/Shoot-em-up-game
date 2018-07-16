using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField]
    IntReference baseValue;

    public int value
    {
        get { return baseValue.Value; }
    }

    public void AddModifier(int _modifier)
    {
        if(_modifier != 0)
        {
            baseValue.Value += _modifier;
        }
    }

    public void RemoveModifier(int _modifier)
    {
        if (_modifier != 0)
        {
            int tempValue = baseValue.Value - _modifier;
            baseValue.Value = Mathf.Clamp(tempValue, 0, baseValue.Value);
        }
    }

}
