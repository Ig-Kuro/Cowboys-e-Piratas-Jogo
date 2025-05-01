using UnityEngine;
using Mirror;

public class InimigoLonge : Inimigo
{
    public Gun weapon;

    [ServerCallback]
    void FixedUpdate()
    {
        if (target == null) return;

        if (agent.enabled)
        {
            agent.destination = target.position;

            if (Physics.Raycast(attackPoint.position, attackPoint.transform.forward, out ray, attackRange))
            {
                if (ray.collider.CompareTag("Player"))
                {
                    if (!moveWhileAttacking)
                    {
                        agent.enabled = false;
                        rb.isKinematic = false;
                        recovering = true;
                        Invoke(nameof(Recovery), weapon.attackRate);
                    }
                    weapon.ShootEnemyProjectile(attackPoint.gameObject);
                }
            }
        }
    }

    // Gizmo só deve rodar no editor e localmente
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
    }
}
