using UnityEngine;
using TMPro;
using System.Collections;

public class WaveUIManager : MonoBehaviour
{
    [Header("Textos da HUD Global")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemyText;
    [SerializeField] private TextMeshProUGUI timerText;

    private Coroutine countdownCoroutine;

    public void SetWaveNumber(int wave)
    {
        waveText.text = $"Wave: {wave}";
    }

    public void SetEnemyCount(int current, int max)
    {
        enemyText.text = $"Inimigos: {current}/{max}";
    }

    public void StartCountdown(float duration)
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        countdownCoroutine = StartCoroutine(CountdownRoutine(duration));
    }

    public void StopCountdown()
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        timerText.text = "";
    }

    private IEnumerator CountdownRoutine(float duration)
    {
        float timeLeft = duration;

        while (timeLeft > 0)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft % 60f);
            timerText.text = $"Próxima Wave: {minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        timerText.text = "Wave Iniciada!";
    }
}
