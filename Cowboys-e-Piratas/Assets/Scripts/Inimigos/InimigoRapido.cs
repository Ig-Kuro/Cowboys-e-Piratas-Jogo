using UnityEngine;
using Mirror;

public class InimigoRapido : Inimigo
{
    public MeleeWeapon weapon;
    public bool canDodge;
    Personagem player;
    public float dodgeSpeed, dodgeTimer;
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
                    DecideAttackAnimation();
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
    public override void TomarDano(int valor)
    {
        vida -= valor;
        if (canbeStaggered)
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

    void CanBeStaggeredAgain()
    {
        canbeStaggered = true;
    }

    [Server]
    void DecideDamageAnimation()
    {
        if (damage.damageDirection == DamageInfo.DamageDirection.Right)
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
        if (damage.damageDirection == DamageInfo.DamageDirection.Right)
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


    [Server]
    void DecideAttackAnimation()
    {
        anim.SetBool("Ataque", true);
        anim.SetBool("Walking", false);
        if (bracoDireito.activeInHierarchy == true)
        {
            if (bracoEsquerdo.activeInHierarchy == true)
            {
                int random = Random.Range(0, 3);
                if (random == 0)
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
            }
        }
        else
        {
            if (bracoEsquerdo.activeInHierarchy)
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
