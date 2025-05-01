using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager instance;
    public Spawner[] spawners;

    [HideInInspector]
    public List<Inimigo> inimigosSpawnado = new List<Inimigo>();

    public float timeBetweenWaves;
    int currentWave;
    public int maxAssaults;
    int currentAssaults;
    public float timeBetweenAssaults;
    public int spawnedEnemies;
    public int maxEnemies;
    //int killedEnemies;
    public int enemiesPerSpawner;

    private void Awake()
    {
        instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartWave();
        InvokeRepeating(nameof(CountEnemies), 5f, 5f);
    }

    [Server]
    void ActivateSpawners()
    {
        if (currentAssaults < maxAssaults)
        {
            foreach (var spawner in spawners)
            {
                spawner.SpawnEnemies(maxEnemies, enemiesPerSpawner);
            }
            currentAssaults++;
            Invoke(nameof(ActivateSpawners), timeBetweenAssaults);
        }
    }

    [Server]
    void StartWave()
    {
        //killedEnemies = 0;
        spawnedEnemies = 0;
        currentAssaults = 0;
        currentWave++;

        maxEnemies = enemiesPerSpawner * maxAssaults * spawners.Length;

        ActivateSpawners();
        Debug.Log($"Iniciando Onda: {currentWave}");
    }

    [Server]
    public void CountEnemies()
    {
        inimigosSpawnado.RemoveAll(i => i == null); // limpa mortos

        if (inimigosSpawnado.Count == 0)
        {
            Invoke(nameof(StartWave), timeBetweenWaves);
        }
    }
}
