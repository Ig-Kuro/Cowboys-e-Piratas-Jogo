using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using Mirror;
using System.Collections;

public class WaveManager : NetworkBehaviour
{
    public static WaveManager instance;

    [Header("UI & Store")]
    public GameObject countdown;
    public GameObject storePrefab;

    [Header("Wave Settings")]
    [SerializeField] Wave[] initialWaves;
    public float timeBetweenWaves = 10f;
    [SyncVar(hook = nameof(OnWaveChanged))]public Wave currentWave;
    public WaveSpawner[] spawners;
    public int spawnRange = 5;

    [SerializeField] WaveUIManager ui;

    [SyncVar] private int maxEnemies;
    [SyncVar(hook = nameof(OnEnemyCountChanged))] public int currentEnemies = 0;

    public override void OnStartServer()
    {
        instance = this;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        instance = this;

        if(SceneManager.GetActiveScene().name == "Jogo") currentWave = initialWaves[0];
        else currentWave = initialWaves[1];
        
        maxEnemies = currentWave.maxEnemies;
        if (ui == null)
        {
            Debug.LogWarning("WaveUIManager não encontrado no cliente!");
        }
        Invoke(nameof(StartSpawning), 5f);

        // Espera um pouco para garantir que tudo foi carregado
        StartCoroutine(DelayedUIUpdate());
    }

    [Server]
    void StartSpawning()
    {
        ui = FindFirstObjectByType<WaveUIManager>();
        spawners = FindObjectsByType<WaveSpawner>(FindObjectsSortMode.None);
        foreach (WaveSpawner spawner in spawners)
        {
            spawner.SpawnEnemies(currentWave.enemieSpawnsByType);
        }
        //ui.SetEnemyCount(currentEnemies, maxEnemies);
        //CheckWave();
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
        if (currentWave.nextWave == null)
        {
            LoadingScreen.instance.ShowVictory();
            return;
        }
        
        RespawnDeadPlayers();
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
            NetworkServer.Destroy(store);
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
                //GameObject store = Instantiate(storePrefab, randomPoint, quaternion.identity);
                //NetworkServer.Spawn(store);
            }
        }
    }

    [TargetRpc]
    public void TargetUpdateGlobalUI(NetworkConnection target, bool showCountdown)
    {
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
        currentEnemies = math.max(0, currentEnemies - 1);
        UpdateUIForAll();
        CheckIfWaveEnded();
    }

    [Server]
    public void OnEnemySpawned()
    {
        currentEnemies++;
        UpdateUIForAll();
    }

    void OnEnemyCountChanged(int oldValue, int newValue)
    {
        if (ui != null)
        {
            ui.SetEnemyCount(newValue, maxEnemies);
        }
    }

    void OnWaveChanged(Wave oldWave, Wave newWave)
    {
        if (ui != null)
        {
            ui.SetWaveNumber(newWave.waveNumber);
        }
    }

    private IEnumerator DelayedUIUpdate()
    {
        // Espera um frame ou dois
        yield return new WaitForSeconds(0.1f);

        // Só o servidor pode chamar o Rpc
        if (isServer)
        {
            UpdateUIForAll();
        }
    }
    
    [Server]
    void RespawnDeadPlayers()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn != null && conn.identity != null)
            {
                var player = conn.identity.GetComponent<Personagem>();
                if (player != null && (player.dead || player.currentHp <= 0))
                {
                    Debug.Log($"[WaveManager] Respawnando {player.name}");
                    TargetRespawn(conn, player);
                }
            }
        }
    }

   [TargetRpc]
    public void TargetRespawn(NetworkConnection conn, Personagem player)
    {
        if (player == null) return;

        player.Respawn();
        Debug.Log($"[WaveManager] {player.name} foi respawnado.");
    }
}
