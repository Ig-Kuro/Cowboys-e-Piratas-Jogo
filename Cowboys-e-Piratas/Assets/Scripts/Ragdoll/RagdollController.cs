using UnityEngine;
using Mirror;

public class RagdollController : NetworkBehaviour
{
    //Ainda to vendo como isso funciona certinho...

    private Animator animator;
    private Rigidbody[] ragdollRigidbodies;

    void Awake()
    {
        animator = GetComponent<Animator>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        // Isso aqui desliga o ragdoll no in�cio
        SetRagdoll(false);
    }

    // Aqui liga ou desliga todos os rigidbodies
    void SetRagdoll(bool active)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != null && rb.gameObject != this.gameObject) // ignora o rigidbody raiz
            {
                rb.isKinematic = !active;
                rb.detectCollisions = active;
            }
        }

        // Liga/Desliga o animator junto
        if (animator != null)
            animator.enabled = !active;
    }

    // Chamar isso quando o inimigo morrer
    [Server]
    public void ActivateRagdoll(bool instaKill = false)
    {
        // Liga o ragdoll no servidor
        RpcSetRagdoll(true);

        // E agenda a destruição
        Invoke(nameof(KillEnemy), instaKill ? 0 : 2f);
    }

    [ClientRpc]
    void RpcSetRagdoll(bool active)
    {
        // Reproduz a mudança também nos clientes
        SetRagdoll(active);
    }

    [Server]
    void KillEnemy()
    {
        NetworkServer.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }
}
