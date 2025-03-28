using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;
    public Spawner[] spawners;


    public float timeBetweenWaves;
    int currentWave;
    public int maxAssaults;
    int currentAssaults;
    public float timeBetweenAssaults;
    public int spawnedEnemies;
    public int maxEnemies;
    int killedEnemies;
    public int enemiesPerSpawner;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartWave();
    }
    void ActivateSpawners()
    {
        if (currentAssaults < maxAssaults)
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].SpawnEnemies(maxEnemies, enemiesPerSpawner);
            }
            currentAssaults++;
            Invoke("ActivateSpawners", timeBetweenAssaults);
        }
        else return;
    }

    void StartWave()
    {
        
        killedEnemies = 0;
        spawnedEnemies = 0;
        currentAssaults = 0;
        currentWave++;
        //Caso queira aumentar a dificuldade de acordo com a horda atual
        /*if(currentWave == 5)
        {
            maxEnemies += 3;
            maxAssaults++;
            enemiesPerSpawner += 2;
        }
        */
        maxEnemies = enemiesPerSpawner * maxAssaults * spawners.Length;
        ActivateSpawners();
        Debug.Log(currentWave);
    }

    public void CountEnemies()
    {
        killedEnemies++;
        Debug.Log(killedEnemies);
        if(killedEnemies >= maxEnemies)
        {
            Invoke("StartWave", timeBetweenWaves);
        }
    }
}
