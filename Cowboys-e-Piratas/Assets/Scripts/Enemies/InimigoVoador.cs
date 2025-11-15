using Mirror;
using UnityEngine;

public class InimigoVoador : Inimigo
{
    public EnemyRangedWeapon weapon;

    public override void PerformAttack()
    {
        if (recovering) return;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out RaycastHit ray, attackRange))
        {
            if (ray.collider.CompareTag("Player"))
            {

                anim.SetTrigger("Attack");
                weapon.Action();
                recovering = true;
                Invoke(nameof(Recover), weapon.attackRate + weapon.delay);
            }
        }
    }

    [Server]
    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        anim.SetTrigger("Damage");
    }

    [Server]
    public override void Die()
    {
        base.Die();
        ragdoll.ActivateRagdoll();
    }
}
