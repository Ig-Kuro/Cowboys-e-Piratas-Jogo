using UnityEngine;
using Mirror;

public class InimigoLonge : Inimigo
{
    public Gun weapon;
    public MeleeWeapon biter;
    public float damageTreshold = 3;
    int damageTakenRight, damageTakenLeft;

    [ServerCallback]
    void FixedUpdate()
    {
        if (target == null) return;

        if (agent.enabled)
        {
            agent.destination = new Vector3(target.position.x, transform.position.y, target.position.z);
            if(bracoEsquerdo.activeInHierarchy)
            {
                AIBehaviourDistance();
            }
            else
            {
                AIBehaviourMelee();
            }
        }
    }


    [Server]
    void AIBehaviourDistance()
    {
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
                anim.SetTrigger("Lanca");
                weapon.enemyTarget = attackPoint.gameObject;
                weapon.StartCoroutine("ShootEnemyProjectile");
            }
        }
    }
    [Server]
    void AIBehaviourMelee()
    {
        if (Physics.Raycast(attackPoint.position, attackPoint.transform.forward, out ray, 1f))
        {
            if (ray.collider.CompareTag("Player"))
            {
                if (!moveWhileAttacking)
                {
                    agent.enabled = false;
                    rb.isKinematic = false;
                    recovering = true;
                    Invoke(nameof(Recovery), biter.attackRate + biter.delay);
                }
                anim.SetTrigger("Bite");
                biter.Action();
            }
        }
    }


    void CanBeStaggeredAgain()
    {
        canbeStaggered = true;
    }

    [Server]
    void CheckArm(int valor)
    {
        if (damage.damageDirection == DamageInfo.DamageDirection.Right)
        {
            damageTakenRight += valor;
            if (damageTakenRight > damageTreshold)
            {
                Stun();
                bracoDireito.SetActive(false);
                GameObject blood = Instantiate(looseArmFX, bracoDireito.transform.position, transform.rotation);
                Destroy(blood, 0.2f);
            }
        }

        if (damage.damageDirection == DamageInfo.DamageDirection.Left)
        {
            damageTakenLeft += valor;
            if (damageTakenLeft > damageTreshold)
            {
                Stun();
                bracoEsquerdo.SetActive(false);
                GameObject blood = Instantiate(looseArmFX, bracoEsquerdo.transform.position, transform.rotation);
                Destroy(blood, 0.2f);
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
