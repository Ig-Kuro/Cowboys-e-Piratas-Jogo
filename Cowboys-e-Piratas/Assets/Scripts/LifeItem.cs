using Mirror;
using UnityEngine;

public class LifeItem : NetworkBehaviour
{
    [SerializeField] public int healAmount = 25; 

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se é um player com o componente Personagem
        Personagem player = other.GetComponent<Personagem>();

        if (player != null && player.isLocalPlayer) // só o player local processa
        {
            // Pede ao servidor para curar o jogador
            player.CmdHealPlayer(healAmount);

            // Destroi o item no servidor (espelha para todos)
            CmdDestroySelf();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
