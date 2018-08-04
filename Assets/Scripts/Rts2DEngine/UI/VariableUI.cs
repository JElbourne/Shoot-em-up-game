using UnityEngine;
using TMPro;

public class VariableUI : MonoBehaviour {

    public IntVariable currentValue;
    public IntVariable maxValue;

    TextMeshProUGUI m_HPTextField;

    // Use this for initialization
    void Start () {
        m_HPTextField = GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        if (currentValue != null && maxValue != null)
        {
            m_HPTextField.SetText(currentValue.value + " /" + maxValue.value);
        }
        else if (currentValue != null)
        {
            m_HPTextField.SetText("+ " + currentValue.value);
        }
        else if (maxValue != null)
        {
            m_HPTextField.SetText("+ " + maxValue.value);
        }

    }
}
