using UnityEngine;
using Mirror;

public class InimigoRapido : Inimigo
{
    public MeleeWeapon weapon;
    public bool canDodge;
    Personagem player;
    public float dodgeSpeed, dodgeTimer;

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

    [ServerCallback]
    void Update()
    {
        if (player == null) return;

        if (player.input.AttackInput())
        {
            if (canDodge && !recovering)
            {
                Dodge();
            }
        }
    }

    [Server]
    void Dodge()
    {
        Push();

        int lado = Random.Range(0, 2);
        Vector3 dodgeDirection = lado == 0 ? Vector3.right : -Vector3.right;

        rb.AddForce(dodgeDirection * dodgeSpeed, ForceMode.Impulse);
        canDodge = false;
        weapon.canAttack = false;
        Invoke(nameof(RecoverDodge), dodgeTimer);
    }

    [Server]
    void RecoverDodge()
    {
        canDodge = true;
        weapon.canAttack = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
    }
}
