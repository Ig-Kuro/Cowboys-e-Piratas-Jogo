using UnityEngine;
using Mirror;

public class InimigoPerto : Inimigo
{
    public MeleeWeapon weapon;

    [Header("Dano e braços")]
    public float damageTreshold = 3;
    private int damageTakenRight, damageTakenLeft;
    public GameObject bracoDireito, bracoEsquerdo;

    [Server]
    public override void PerformAttack()
    {
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
            }
        }
        else if (damage.damageDirection == DamageInfo.DamageDirection.Left)
        {
            damageTakenLeft += valor;
            if (damageTakenLeft > damageTreshold && bracoEsquerdo.activeSelf)
            {
                Stun();
                bracoEsquerdo.SetActive(false);
            }
        }
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

        return "Bite"; // Fallback
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackRange);
    }
}
