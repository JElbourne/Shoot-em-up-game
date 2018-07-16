using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleUI : MonoBehaviour {

    #region Singleton
    public static ConsoleUI instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ConsoleUI Found!");
            return;
        }
        instance = this;
    }
    #endregion

    public TextMeshProUGUI consoleTextField;
    public int maxConsoleLines = 8;

    List<string> m_Log = new List<string>();
	
	void Start () {
		if (consoleTextField == null)
        {
            Debug.LogWarning("Console Text Field is not setup in ConsoleUI.");
        }
	}

    public void UpdateConsoleUI(string _message)
    {
        m_Log.Add(_message);
        string m_ConsoleText = "";
        Debug.Log("Log Count: " + m_Log.Count);
        int currentLines = Mathf.Clamp(m_Log.Count, 0, maxConsoleLines);
        for (int i = 0; i < currentLines; i++)
        {
            m_ConsoleText += m_Log[i];
            m_ConsoleText += "\n";
        }
        consoleTextField.SetText(m_ConsoleText);
    }
}
