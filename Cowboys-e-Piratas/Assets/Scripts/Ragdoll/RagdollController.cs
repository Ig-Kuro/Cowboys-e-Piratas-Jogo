using UnityEngine;
using Mirror;

public class RagdollController : MonoBehaviour
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

    [Server]
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
    public void ActivateRagdoll()
    {
        SetRagdoll(true);
        Invoke(nameof(KillEnemy), 2f); // Destr�i o inimigo ap�s 5 segundos
    }
    [Server]
    void KillEnemy()
    {
        NetworkServer.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }
}
