using UnityEngine;
using Mirror;

public class Spawner : NetworkBehaviour
{
    public Inimigo[] spawnaveisAqui;

    [Server]
    public void SpawnEnemies(int maxEnemies, int enemiesPerSpawn)
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            if (SpawnManager.instance.spawnedEnemies < maxEnemies)
            {
                Inimigo prefab = spawnaveisAqui[Random.Range(0, spawnaveisAqui.Length)];
                Inimigo inim = Instantiate(prefab, transform.position, Quaternion.identity);

                NetworkServer.Spawn(inim.gameObject); // importante
                SpawnManager.instance.inimigosSpawnado.Add(inim);
                SpawnManager.instance.spawnedEnemies++;
            }
        }
    }
}
