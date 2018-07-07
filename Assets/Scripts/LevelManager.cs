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
    public float fadeStepDuration = 0.5f;
    public float textDisplayDuration = 1f;

    int m_NextLevel;

    private void Awake()
    {
        m_WorldInstance = FindObjectOfType<WorldInstance>();
    }

    public void ChangeLevel(int _newLevel)
    {
        m_NextLevel = _newLevel;

        //SwitchActiveLevel();
        StartCoroutine("LevelTransition");
    }

    public void WinGame()
    {
        StartCoroutine("WinTransition");
    }

    public void EnableLevelText()
    {
        transitionText.SetText("Level " + m_NextLevel);
        transitionText.enabled = true;
    }

    IEnumerator WinTransition()
    {
        yield return StartCoroutine("FadeOut");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(WinSceneName);
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
            FindObjectOfType<PlayerController>().ChangeLevel(m_NextLevel);
        }

        yield return new WaitForSeconds(textDisplayDuration);
    }
}
