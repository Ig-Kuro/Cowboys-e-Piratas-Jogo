using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using Mirror;

public class WaveManager : NetworkBehaviour
{
    public static WaveManager instance;

    [Header("UI & Store")]
    public GameObject countdown;
    public GameObject storePrefab;

    [Header("Wave Settings")]
    public float timeBetweenWaves = 10f;
    public Wave currentWave;
    public WaveSpawner[] spawners;
    public int spawnRange = 5;

    [SyncVar] private int maxEnemies;
    [SyncVar] public int currentEnemies = 0;

    public override void OnStartServer()
    {
        base.OnStartServer();
        instance = this;
        maxEnemies = currentWave.maxEnemies;
        StartSpawning();
    }

    [Server]
    void StartSpawning()
    {
        foreach (WaveSpawner spawner in spawners)
        {
            spawner.SpawnEnemies(currentWave.enemieSpawnsByType);
        }

        CheckWave();
    }

    [Server]
    public void CheckWave()
    {
        if (currentEnemies < maxEnemies)
        {
            StartSpawning();
        }
    }

    [Server]
    public void CheckIfWaveEnded()
    {
        if (currentEnemies <= 0)
        {
            EndWave();
        }
    }

    [Server]
    void EndWave()
    {
        RpcToggleTimer(true);
        RpcSpawnStore();
        Invoke(nameof(NextWave), timeBetweenWaves);
    }

    [Server]
    void NextWave()
    {
        RpcDestroyStore();

        RpcToggleTimer(false);

        if (currentWave.nextWave == null)
        {
            NetworkManager.singleton.ServerChangeScene("Inicio");
        }
        else
        {
            currentWave = currentWave.nextWave;
            maxEnemies = currentWave.maxEnemies;
            StartSpawning();
        }
    }

    [ClientRpc]
    void RpcToggleTimer(bool show)
    {
        if (countdown != null)
        {
            countdown.SetActive(show);
            if (show)
                countdown.GetComponent<WaveCountdown>()?.ResetTime();
        }
    }

    [ClientRpc]
    void RpcDestroyStore()
    {
        GameObject store = GameObject.FindGameObjectWithTag("Store");
        if (store != null)
        {
            Destroy(store);
        }
    }

    [ClientRpc]
    void RpcSpawnStore()
    {
        GameObject localPlayer = GameObject.FindGameObjectWithTag("Player");
        if (localPlayer != null)
        {
            Vector3 randomPoint = localPlayer.transform.position + UnityEngine.Random.insideUnitSphere * spawnRange;
            randomPoint.y = localPlayer.transform.position.y;

            if (isServer)
            {
                GameObject store = Instantiate(storePrefab, randomPoint, quaternion.identity);
                NetworkServer.Spawn(store);
            }
        }
    }

    [Server]
    public void OnEnemyKilled()
    {
        currentEnemies--;
        CheckIfWaveEnded();
    }

    [Server]
    public void OnEnemySpawned()
    {
        currentEnemies++;
    }
}
