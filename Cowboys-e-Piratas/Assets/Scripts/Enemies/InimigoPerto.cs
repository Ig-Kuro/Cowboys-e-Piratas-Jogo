using UnityEngine;
using Mirror;

public class InimigoPerto : Inimigo
{
    public EnemyMeleeWeapon weapon;

    [Header("Dano e braços")]
    public float damageTreshold = 3;
    private int damageTakenRight, damageTakenLeft;
    public GameObject bracoDireito, bracoEsquerdo;
    public bool checkArm = true;
    public bool deathAction;
    [SerializeField] GameObject deathVFX;

    [Server]
    public override void PerformAttack()
    {
        if (recovering) return;

        anim.SetTrigger(GetRandomMeleeAnim());
        weapon.Action();
        if (ataqueAudio != null)
        {
            ataqueAudio.Play();
        }

        if (!moveWhileAttacking)
        {
            agent.enabled = false;
            rb.isKinematic = false;
            recovering = true;
            Invoke(nameof(Recover), weapon.attackRate + weapon.attackDelay);
        }
    }

    [Server]
    public override void TakeDamage(int valor)
    {
        base.TakeDamage(valor);
        if (checkArm)
            CheckArm(valor);
    }

    [Server]
    public override void Die()
    {
        if (!deathAction)
        {
            base.Die();
            ragdoll.ActivateRagdoll();
        }
        else
        {
            anim.SetTrigger("Death");
        }
    }

    [Server]
    public void DeathAction()
    {
        base.Die();
        if(deathVFX != null)
        {
            GameObject vfx = Instantiate(deathVFX, attackPoint.position, Quaternion.identity);
            NetworkServer.Spawn(vfx);
            ragdoll.ActivateRagdoll();
            return;
        }
        ragdoll.ActivateRagdoll(true);
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
}
