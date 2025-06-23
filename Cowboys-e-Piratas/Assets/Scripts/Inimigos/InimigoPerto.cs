using UnityEngine;
using Mirror;

public class InimigoPerto : Inimigo
{
    public MeleeWeapon weapon;
    public float damageTreshold = 3;
    int damageTakenRight, damageTakenLeft;


    [ServerCallback]
    void FixedUpdate()
    {
        if (target == null) return;

        if (agent.enabled)
        {
            agent.destination = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 direction = attackPoint.transform.forward;
            if (Physics.Raycast(attackPoint.position, direction, out ray, attackRange))
            {
                if (ray.collider.CompareTag("Player") && canAttack)
                {
                    if (!moveWhileAttacking)
                    {
                        agent.enabled = false;
                        rb.isKinematic = false;
                        recovering = true;
                        Invoke(nameof(Recovery), weapon.delay + weapon.attackRate);
                    }
                    weapon.Action();
                    DecideAttackAnimation();
                }
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
        if(damage.damageDirection == DamageInfo.DamageDirection.Right)
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

    [Server]
    void DecideAttackAnimation()
    {
        if (bracoDireito.activeInHierarchy== true)
        {
            if(bracoEsquerdo.activeInHierarchy == true)
            {
                int random = Random.Range(0, 3);
                if(random == 0)
                {
                    anim.SetTrigger("ClawDir");
                }

                else if (random == 1)
                {
                    anim.SetTrigger("ClawEsq");
                }
                else if (random == 3)
                {
                    anim.SetTrigger("Bite");
                }
                    Debug.Log(random);
            }
        }
        else
        {
            if(bracoEsquerdo.activeInHierarchy)
            {
                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    anim.SetTrigger("ClawEsq");
                }

                else if (random == 1)
                {
                    anim.SetTrigger("Bite");
                }
                Debug.Log(random);

            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
    }
}
