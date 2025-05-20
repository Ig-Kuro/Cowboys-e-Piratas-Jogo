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

    [SerializeField] WaveUIManager ui;

    [SyncVar] private int maxEnemies;
    [SyncVar] public int currentEnemies = 0;

    public override void OnStartClient()
    {
        base.OnStartClient();
        instance = this;
        maxEnemies = currentWave.maxEnemies;
        spawners = FindObjectsByType<WaveSpawner>(FindObjectsSortMode.None);
        if (ui == null) ui = FindFirstObjectByType<WaveUIManager>();
        StartSpawning();
        ui.SetWaveNumber(currentWave.waveNumber);
        ui.SetEnemyCount(currentEnemies, maxEnemies);
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
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
                TargetUpdateGlobalUI(conn, true);
        }

        RpcSpawnStore();
        Invoke(nameof(NextWave), timeBetweenWaves);
    }

    [Server]
    void NextWave()
    {
        RpcDestroyStore();

        if (currentWave.nextWave == null)
        {
            NetworkManager.singleton.ServerChangeScene("Inicio");
            return;
        }

        currentWave = currentWave.nextWave;
        maxEnemies = currentWave.maxEnemies;

        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
                TargetUpdateGlobalUI(conn, false);
        }

        StartSpawning();
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

    [TargetRpc]
    public void TargetUpdateGlobalUI(NetworkConnection target, bool showCountdown)
    {
        Debug.Log(ui);
        if (ui != null)
        {
            ui.SetWaveNumber(currentWave.waveNumber);
            ui.SetEnemyCount(currentEnemies, maxEnemies);

            if (showCountdown)
                ui.StartCountdown(timeBetweenWaves);
            else
                ui.StopCountdown();
        }
    }

    [Server]
    void UpdateUIForAll()
    {
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
                TargetUpdateGlobalUI(conn, false);
        }
    }

    [Server]
    public void OnEnemyKilled()
    {
        currentEnemies--;
        UpdateUIForAll();
        CheckIfWaveEnded();
    }

    [Server]
    public void OnEnemySpawned()
    {
        currentEnemies++;
    }
}
