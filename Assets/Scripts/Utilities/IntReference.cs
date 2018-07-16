using System;

[Serializable]
public class IntReference {

    public bool UseConstant = true;
    public int ConstantValue;
    public IntVariable Variable;

    public int Value
    {
        get { return UseConstant ? ConstantValue : Variable.value; }
        set {
            if (UseConstant)
            {
                ConstantValue = value;
            } else
            {
                Variable.value = value;
            }
        }
    }
}
