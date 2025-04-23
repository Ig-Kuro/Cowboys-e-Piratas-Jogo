using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Inimigo[] enemieTypes;

    public void SpawnEnemies(int[] enemiesSpawnsByType)
    {
        for(int i=0;i<enemiesSpawnsByType.Length;i++ )
        {
            for(int j=0;j<enemiesSpawnsByType[i];j++)
            {
                if(WaveManager.instance.currentenemies< WaveManager.instance.currentWave.maxEnemies)
                {
                    Instantiate(enemieTypes[i]);
                    WaveManager.instance.currentenemies++;
                }
                else
                {
                    return;
                }

            }
        }
    }
}
