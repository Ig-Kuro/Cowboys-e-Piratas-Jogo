using Mirror;
using UnityEngine;

public class LifeItem : NetworkBehaviour
{
    [SerializeField] public int healAmount = 25;
    public HealSpawn healSpawn;

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        Personagem player = other.GetComponent<Personagem>();

        if (player != null && player.isLocalPlayer)
        {
            player.CmdHealPlayer(healAmount);
            healSpawn.RespawnHealItem();
            NetworkServer.Destroy(gameObject);
        }
    }
}
