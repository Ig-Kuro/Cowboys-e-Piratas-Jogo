using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyMeleeWeapon : BaseWeapon
{
    [Header("Configurações de ataque")]
    public float attackRange = 1.5f;
    public float attackDelay = 0.3f;
    public LayerMask playerLayer;
    public bool canKnockBack = false;

    public bool canAttack = true;

    public override void Action()
    {
        TryAttack();
    }

    // Chamada quando o inimigo decide atacar
    [Server]
    public void TryAttack()
    {
        if (!canAttack) return;

        canAttack = false;
        StartCoroutine(PerformAttack());
        StartCoroutine(ResetAttackCooldown());
    }

    [Server]
    private IEnumerator PerformAttack()
    {
        // Aguarda um pequeno delay para sincronizar com a animação
        yield return new WaitForSeconds(attackDelay);

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        HashSet<Personagem> damagedPlayers = new();

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Personagem personagem) && !damagedPlayers.Contains(personagem))
            {
                
                personagem.TakeDamage(damage, transform.position - personagem.transform.position, canKnockBack);
                damagedPlayers.Add(personagem);
            }
        }
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
