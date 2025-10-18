using UnityEngine;
using Mirror;

public class InimigoLonge : Inimigo
{
    public RangedWeapon weapon;
    public EnemyMeleeWeapon biter;

    [Header("Armas e dano")]
    public float damageTreshold = 3;
    private int damageTakenRight, damageTakenLeft;

    [Header("Referências")]
    public GameObject bracoDireito, bracoEsquerdo;

    [Server]
    public override void PerformAttack()
    {
        if (bracoEsquerdo.activeInHierarchy)
        {
            TryShoot();
        }
        else
        {
            TryBite();
        }
    }

    [Server]
    private void TryShoot()
    {
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out RaycastHit ray, attackRange))
        {
            if (ray.collider.CompareTag("Player"))
            {
                anim.SetTrigger("Lanca");
                weapon.enemyTarget = attackPoint.gameObject;
                weapon.StartCoroutine("ShootEnemyProjectile");

                if (!moveWhileAttacking)
                {
                    agent.enabled = false;
                    rb.isKinematic = false;
                    recovering = true;
                    Invoke(nameof(Recover), weapon.attackRate);
                }
            }
        }
    }

    [Server]
    private void TryBite()
    {
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out RaycastHit ray, attackRange))
        {
            if (ray.collider.CompareTag("Player"))
            {
                anim.SetTrigger("Bite");
                biter.Action();

                if (!moveWhileAttacking)
                {
                    agent.enabled = false;
                    rb.isKinematic = false;
                    recovering = true;
                    Invoke(nameof(Recover), biter.attackRate + biter.attackDelay);
                }
            }
        }
    }

    [Server]
    public override void TakeDamage(int valor)
    {
        base.TakeDamage(valor);
        CheckArm(valor);
    }

    [Server]
    private void CheckArm(int valor)
    {
        if (damage.damageDirection == DamageInfo.DamageDirection.Right)
        {
            damageTakenRight += valor;
            if (damageTakenRight > damageTreshold && bracoDireito.activeSelf)
            {
                Stun();
                bracoDireito.SetActive(false);
                GameObject blood = Instantiate(looseArmFX, bracoDireito.transform.position, transform.rotation);
                Destroy(blood, 0.2f);
            }
        }
        else if (damage.damageDirection == DamageInfo.DamageDirection.Left)
        {
            damageTakenLeft += valor;
            if (damageTakenLeft > damageTreshold && bracoEsquerdo.activeSelf)
            {
                Stun();
                bracoEsquerdo.SetActive(false);
                attackRange = 2f;
                GameObject blood = Instantiate(looseArmFX, bracoEsquerdo.transform.position, transform.rotation);
                Destroy(blood, 0.2f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
    }
}
