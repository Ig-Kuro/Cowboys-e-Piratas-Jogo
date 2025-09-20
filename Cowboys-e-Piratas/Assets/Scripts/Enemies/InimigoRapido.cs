using UnityEngine;
using Mirror;

public class InimigoRapido : Inimigo
{
    public MeleeWeapon weapon;

    [Header("Esquiva")]
    public bool canDodge = true;
    public float dodgeSpeed = 10f, dodgeTimer = 2f;

    [Header("Dano e braços")]
    public float damageTreshold = 3;
    private int damageTakenRight, damageTakenLeft;
    public GameObject bracoDireito, bracoEsquerdo;

    [Server]
    public override void PerformAttack()
    {
        if (recovering) return;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out RaycastHit ray, attackRange))
        {
            if (ray.collider.CompareTag("Player"))
            {
                anim.SetTrigger(GetRandomMeleeAnim());
                weapon.Action();

                if (!moveWhileAttacking)
                {
                    agent.enabled = false;
                    rb.isKinematic = false;
                    recovering = true;
                    Invoke(nameof(Recover), weapon.attackRate + weapon.delay);
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
                GameObject blood = Instantiate(looseArmFX, bracoEsquerdo.transform.position, transform.rotation);
                Destroy(blood, 0.2f);
            }
        }
    }

    [Server]
    public void TryDodge()
    {
        if (!canDodge || recovering) return;

        Push();
        Vector3 dodgeDir = Random.Range(0, 2) == 0 ? Vector3.right : Vector3.left;
        rb.AddForce(dodgeDir * dodgeSpeed, ForceMode.Impulse);
        canDodge = false;
        weapon.canAttack = false;

        Invoke(nameof(RecoverDodge), dodgeTimer);
    }

    [Server]
    private void RecoverDodge()
    {
        canDodge = true;
        weapon.canAttack = true;
    }

    private string GetRandomMeleeAnim()
    {
        if (bracoDireito.activeSelf && bracoEsquerdo.activeSelf)
        {
            int r = Random.Range(0, 3);
            return r switch
            {
                0 => "ClawDir",
                1 => "ClawEsq",
                _ => "Bite"
            };
        }
        else if (bracoEsquerdo.activeSelf)
        {
            return Random.Range(0, 2) == 0 ? "ClawEsq" : "Bite";
        }
        else if (bracoDireito.activeSelf)
        {
            return "ClawDir";
        }

        return "Bite";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
    }
}
