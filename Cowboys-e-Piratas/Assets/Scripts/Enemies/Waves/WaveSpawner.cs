using UnityEngine;
using System.Collections;
using Mirror;

public class WaveSpawner : NetworkBehaviour
{
    public Inimigo[] enemieTypes;
    public int enemiesPerBurst = 5;
    public float timeBetweenBursts = 2f;

    [Server]
    public void SpawnEnemies(int[] enemiesSpawnsByType)
    {
        int spawnedEnemies = 0;
        for (int i = 0; i < enemiesSpawnsByType.Length; i++)
        {
            for (int j = 0; j < enemiesSpawnsByType[i]; j++)
            {
                if (WaveManager.instance.currentEnemies < WaveManager.instance.currentWave.maxEnemies)
                {
                    if(spawnedEnemies >= enemiesPerBurst)
                    {
                        StartCoroutine(EnemySpawnerCallback());
                        return;
                    }
                    // Spawna inimigo no servidor
                    Inimigo enemyInstance = Instantiate(enemieTypes[Random.Range(0, enemieTypes.Length)], transform.position, Quaternion.identity);

                    // Spawna na rede
                    NetworkServer.Spawn(enemyInstance.gameObject);
                    spawnedEnemies++;

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

    IEnumerator EnemySpawnerCallback()
    {
        yield return new WaitForSeconds(timeBetweenBursts);
        SpawnEnemies(WaveManager.instance.currentWave.enemieSpawnsByType);
    }
}
