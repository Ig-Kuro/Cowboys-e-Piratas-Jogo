using UnityEngine;
using Mirror;

public class InimigoPerto : Inimigo
{
    public MeleeWeapon weapon;

    [ServerCallback]
    void FixedUpdate()
    {
        if (target == null) return;

        if (agent.enabled)
        {
            agent.destination = target.position;

            Vector3 direction = attackPoint.transform.forward;
            if (Physics.Raycast(attackPoint.position, direction, out ray, attackRange))
            {
                if (ray.collider.CompareTag("Player"))
                {
                    if (!moveWhileAttacking)
                    {
                        agent.enabled = false;
                        rb.isKinematic = false;
                        recovering = true;
                        Invoke(nameof(Recovery), weapon.delay + weapon.attackRate);
                    }

                    weapon.Action();
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
    }
}
