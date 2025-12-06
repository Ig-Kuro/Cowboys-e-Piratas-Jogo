using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    [Header("Panels")]
    public GameObject loadingPanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Fades")]
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // -------------------------
    // SHOW FUNCTIONS
    // -------------------------

    public void ShowLoading()
    {
        ShowPanel(loadingPanel);
    }

    public void ShowVictory()
    {
        ShowPanel(victoryPanel);
    }

    public void ShowDefeat()
    {
        ShowPanel(defeatPanel);
    }

    private void ShowPanel(GameObject panel)
    {
        if (panel == null) return;

        panel.SetActive(true);

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();

        StartCoroutine(Fade(cg, 0f, 1f));
    }

    // -------------------------
    // HIDE (ÚNICA PARA TUDO)
    // -------------------------

    public void Hide()
    {
        // Oculta quem estiver ativo (primeiro encontrado)
        if (loadingPanel != null && loadingPanel.activeSelf)
            StartCoroutine(HidePanel(loadingPanel));

        if (victoryPanel != null && victoryPanel.activeSelf)
            StartCoroutine(HidePanel(victoryPanel));

        if (defeatPanel != null && defeatPanel.activeSelf)
            StartCoroutine(HidePanel(defeatPanel));
    }

    private IEnumerator HidePanel(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();

        yield return StartCoroutine(Fade(cg, 1f, 0f));

        panel.SetActive(false);
    }

    // -------------------------
    // Fade genérico
    // -------------------------
    private IEnumerator Fade(CanvasGroup cg, float start, float end)
    {
        float t = 0f;

        cg.alpha = start;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        cg.alpha = end;
    }
}
