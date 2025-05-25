using UnityEngine;
using Mirror;

public class InimigoPerto : Inimigo
{
    public MeleeWeapon weapon;
    public float damageTreshold = 3;
    int damageTakenRight, damageTakenLeft;


    [Server]
    public override void TomarDano(int valor)
    {
        vida -= valor;
        if(canbeStaggered )
        {
            anim.SetBool("Dano", true);
            DecideDamageAnimation();
            canbeStaggered = false;
            Push();
            Invoke("CanBeStaggeredAgain", 2f);
        }

        CheckArm(valor);

        if (vida < 0)
        {

            if (WaveManager.instance != null)
            {
                WaveManager.instance.currentenemies--;
                WaveManager.instance.CheckIfWaveEnded();
            }
            NetworkServer.Destroy(gameObject);
        }
    }

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
    void DecideDamageAnimation()
    {
        if(damage.damageDirection == DamageInfo.DamageDirection.Right)
        {
            anim.SetTrigger("DanoDir");
        }
        else
        {
            anim.SetTrigger("DanoEsq");
        }
        anim.SetBool("Dano", false);
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
            }
        }

        if (damage.damageDirection == DamageInfo.DamageDirection.Left)
        {
            damageTakenLeft += valor;
            if (damageTakenLeft > damageTreshold)
            {
                Stun();
                bracoEsquerdo.SetActive(false);
            }
        }
    }

    [Server]
    void DecideAttackAnimation()
    {
        anim.SetBool("Ataque", true);
        anim.SetBool("Walking", false);
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

        anim.SetBool("Ataque", false);
        canAttack = false;
        anim.SetBool("Walking", true);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
    }
}
