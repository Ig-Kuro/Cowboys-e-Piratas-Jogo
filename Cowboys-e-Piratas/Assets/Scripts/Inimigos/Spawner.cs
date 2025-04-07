using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Inimigo[] spawnaveisAqui;


    public void SpawnEnemies(int maxEnemies, int enemiesPerSpawn)
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            if(SpawnManager.instance.spawnedEnemies < maxEnemies)
            {
                SpawnManager.instance.spawnedEnemies++;
                Instantiate(spawnaveisAqui[Random.Range(0, spawnaveisAqui.Length)], transform.position, Quaternion.identity);
            }
        }
    }
}
