using UnityEngine;
using Mirror;

public class WaveSpawner : NetworkBehaviour
{
    public Inimigo[] enemieTypes;

    [Server]
    public void SpawnEnemies(int[] enemiesSpawnsByType)
    {
        for (int i = 0; i < enemiesSpawnsByType.Length; i++)
        {
            for (int j = 0; j < enemiesSpawnsByType[i]; j++)
            {
                if (WaveManager.instance.currentEnemies < WaveManager.instance.currentWave.maxEnemies)
                {
                    // Spawna inimigo no servidor
                    Inimigo enemyInstance = Instantiate(enemieTypes[i], transform.position, Quaternion.identity);

                    // Spawna na rede
                    NetworkServer.Spawn(enemyInstance.gameObject);

                    // Informa o WaveManager
                    WaveManager.instance.OnEnemySpawned();
                }
                else
                {
                    return;
                }
            }
        }
    }
}
