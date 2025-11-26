using Mirror;
using UnityEngine;

public class HealSpawn : NetworkBehaviour
{
    [SerializeField] LifeItem lifeItemPrefab;
    [SerializeField] float respawnTime = 10f;

    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnLifeItem();
    }

    public void RespawnHealItem()
    {
        Invoke(nameof(SpawnLifeItem), respawnTime);
    }

    void SpawnLifeItem()
    {
        LifeItem lifeItemInstance = Instantiate(lifeItemPrefab, transform.position, Quaternion.identity);
        lifeItemInstance.healSpawn = this;
        NetworkServer.Spawn(lifeItemInstance.gameObject);
    }
}
