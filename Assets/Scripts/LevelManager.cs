using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour {
    WorldInstance m_WorldInstance;
    public string WinSceneName = "WinScene";
    public Image transitionPanel;
    public TextMeshProUGUI transitionText;
    public TextMeshProUGUI miniMapLevelText;
    public float fadeStepDuration = 0.5f;
    public float textDisplayDuration = 1f;

    int m_NextLevel;

    #region Singleton
    public static LevelManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Level Manager!");
            return;
        }
        instance = this;

        m_WorldInstance = FindObjectOfType<WorldInstance>();
    }
    #endregion

    public void ChangeLevel(int _newLevel)
    {
        Debug.Log("Change Level to: " + _newLevel);
        m_NextLevel = _newLevel;
        StartCoroutine("LevelTransition");
    }

    public void StartGame(int _newLevel)
    {
        m_NextLevel = _newLevel;
        StartCoroutine("StartGameTransition");
    }


    public void WinGame()
    {
        StartCoroutine("WinTransition");
    }

    public void EnableLevelText()
    {
        string newLevelText = "Level " + m_NextLevel;
        transitionText.SetText(newLevelText);
        transitionText.enabled = true;
        miniMapLevelText.SetText(newLevelText);
    }

    IEnumerator WinTransition()
    {
        yield return StartCoroutine("FadeOut");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(WinSceneName);
    }

    IEnumerator StartGameTransition()
    {
        transitionPanel.canvasRenderer.SetAlpha(1f);
        transitionPanel.enabled = true;
        yield return new WaitForSeconds(fadeStepDuration);
        EnableLevelText();
        yield return new WaitForSeconds(fadeStepDuration);
        yield return StartCoroutine("FadeIn");
    }

    IEnumerator LevelTransition()
    {
        yield return StartCoroutine("FadeOut");
        EnableLevelText();
        yield return StartCoroutine("SwitchActiveLevel");
        yield return StartCoroutine("FadeIn");
    }

    IEnumerator FadeOut()
    {
        transitionPanel.canvasRenderer.SetAlpha(0f);
        transitionPanel.enabled = true;
        transitionPanel.CrossFadeAlpha(1f, fadeStepDuration, false);
        //Debug.Log("Fading In");
        yield return new WaitForSeconds(fadeStepDuration);
    }


    IEnumerator FadeIn()
    {
        transitionText.enabled = false;
        transitionPanel.canvasRenderer.SetAlpha(1f);
        transitionPanel.CrossFadeAlpha(0f, fadeStepDuration, false);
        //Debug.Log("Fading Out");
        yield return new WaitForSeconds(fadeStepDuration);
        transitionPanel.enabled = false;
    }

    IEnumerator SwitchActiveLevel()
    {
        if (m_WorldInstance.levelsData.ContainsKey(m_WorldInstance.currentLevel) &&
                m_WorldInstance.levelsData.ContainsKey(m_NextLevel))
        {
            m_WorldInstance.levelsData[m_WorldInstance.currentLevel].SetActive(false);
            m_WorldInstance.levelsData[m_NextLevel].SetActive(true);
            m_WorldInstance.currentLevel = m_NextLevel;
        }

        yield return new WaitForSeconds(textDisplayDuration);
    }
}
