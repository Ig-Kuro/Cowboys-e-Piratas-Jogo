using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class WaveManager : MonoBehaviour
{
    public GameObject countdown,Store;

    public float timeBetweenWaves;
    public static WaveManager instance;
    public Wave currentWave;
    int maxEnemies;
    public int currentenemies=0,spawnRange;

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

    public void CheckWave()
    {
        if(currentenemies<maxEnemies)
        {
            StartSpawning();
        }
    }

    public void CheckIfWaveEnded()
    {
        if(currentenemies == 0)
        {
            EndWave();
        }
    }
    void NextWave()
    {
        Destroy(GameObject.FindGameObjectWithTag("Store"));
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
        SpawnStore();
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
    public void SpawnStore()
    {
        GameObject player= GameObject.FindGameObjectWithTag("Player");
        Vector3 randomPoint= player.transform.position + UnityEngine.Random.insideUnitSphere * spawnRange;
        randomPoint.y=player.transform.position.y;
        Instantiate(Store,randomPoint,quaternion.identity);
    }
}
