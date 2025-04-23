using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public GameObject countdown;

    public float timeBetweenWaves;
    public static WaveManager instance;
    public Wave currentWave;
    int maxEnemies;
    public int currentenemies=0;

    public WaveSpawner[] spawners;
    void Awake()
    {
        instance=this;
        maxEnemies=currentWave.maxEnemies;
    }
    void Start()
    {
        StartSpawning();
    }
    void StartSpawning()
    {
        foreach(WaveSpawner spawner in spawners)
        {
            spawner.SpawnEnemies(currentWave.enemieSpawnsByType);
        }
        CheckWave();
    }

    void CheckWave()
    {
        if(currentenemies<maxEnemies)
        {
            StartSpawning();
        }
        else if(currentenemies==0)
        {
            EndWave();
        }
    }
    void NextWave()
    {
        ToggleTimer();
        if(currentWave.nextWave==null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            currentWave=currentWave.nextWave;
            StartSpawning();
        }
    }
    public void EndWave()
    {
        ToggleTimer();
        Invoke("NextWave",timeBetweenWaves);
    }
    void ToggleTimer()
    {
        if(countdown.activeSelf==false)
        {
            countdown.SetActive(true);
            countdown.GetComponent<WaveCountdown>().ResetTime();
        }
        else
        {
            countdown.SetActive(false);
        }
    }
}
