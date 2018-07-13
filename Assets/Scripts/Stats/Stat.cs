using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField]
    int baseValue;

    private List<int> m_Modifiers = new List<int>();

    public int getValue()
    {
        int finalValue = baseValue;
        m_Modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(int _modifier)
    {
        if(_modifier != 0)
        {
            m_Modifiers.Add(_modifier);
        }
    }

    public void RemoveModifier(int _modifier)
    {
        if (_modifier != 0)
        {
            m_Modifiers.Remove(_modifier);
        }
    }

}
