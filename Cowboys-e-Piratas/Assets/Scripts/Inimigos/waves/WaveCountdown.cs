using UnityEngine;
using TMPro;
public class WaveCountdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;

    float remainingTime;
    
    void Update()
    {
        if(remainingTime>0)
        {
            remainingTime-=Time.deltaTime;
        }
        else
        {
            remainingTime=0;
        }

        int minutes=Mathf.FloorToInt(remainingTime/60);
        int seconds=Mathf.FloorToInt(remainingTime%60);
        timer.text =("Next Wave: ")+ string.Format("{0:00}:{1:00}",minutes,seconds);
    }
    public void ResetTime()
    {
        remainingTime=WaveManager.instance.timeBetweenWaves;
    }
}
