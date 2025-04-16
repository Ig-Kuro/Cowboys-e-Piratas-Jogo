using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class Wave : ScriptableObject
{
    public Wave nextWave;
    public int maxEnemies;

    //Esses valores vão para TODOS os spawners!!!
    public int[] enemieSpawnsByType;

}
