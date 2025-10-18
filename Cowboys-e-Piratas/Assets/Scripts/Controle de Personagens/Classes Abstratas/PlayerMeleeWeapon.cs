using UnityEngine;
using Mirror;

public class PlayerMeleeWeapon : BaseWeapon
{
    [Header("Combo Settings")]
    public float comboResetTime = 1f;
    public int maxCombo = 3;
    public float pushForce = 5f;
    public Vector3 attackRange = new Vector3(1f, 1f, 1f);
    public GameObject swingFX;
    public GameObject bloodFX;

    private bool attacking;
    private bool bufferedInput;
    private float lastAttackTime;
    private Animator anim;
    private Pirata pirata;

    public int currentCombo = 0;
    private bool canAttack = true;

    private void Start()
    {
        pirata = GetComponentInParent<Pirata>();
        anim = pirata.anim.anim;
    }

    public override void Action()
    {
        TryAttack();
    }

    public void TryAttack()
    {
        if (!canAttack)
        {
            bufferedInput = true;
            return;
        }

        PerformCombo();
    }

    private void PerformCombo()
    {
        if (Time.time - lastAttackTime > comboResetTime)
            currentCombo = 0; // reset combo se demorou

        currentCombo++;
        if (currentCombo > maxCombo)
            currentCombo = 1;

        attacking = true;
        bufferedInput = false;
        canAttack = false;
        lastAttackTime = Time.time;

        pirata.canUseSkill1 = pirata.canUseSkill2 = pirata.canUlt = false;

        switch (currentCombo)
        {
            case 1: pirata.anim.SetAttack1Pirata(); break;
            case 2: pirata.anim.SetAttack2Pirata(); break;
            case 3: pirata.anim.SetAttack3Pirata(); break;
        }

        if (swingFX) swingFX.SetActive(true);
    }

    // chamado via Animation Event
    public void OnAttackHit()
    {
        Vector3 direction = currentCombo switch
        {
            1 => pirata.transform.right,
            2 => -pirata.transform.right,
            _ => pirata.transform.forward
        };

        CmdPerformAttack(transform.position, currentCombo, direction);
    }

    // chamado no fim da animação (Animation Event)
    public void OnAttackEnd()
    {
        attacking = false;
        canAttack = true;

        if (bufferedInput)
        {
            bufferedInput = false;
            PerformCombo();
            return;
        }

        Invoke(nameof(ResetCombo), comboResetTime);
    }

    private void ResetCombo()
    {
        currentCombo = 0;
        if (swingFX) swingFX.SetActive(false);
        pirata.canUseSkill1 = pirata.canUseSkill2 = pirata.canUlt = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdPerformAttack(Vector3 position, int combo, Vector3 direction)
    {
        int damageModifier = combo;
        Collider[] colliders = Physics.OverlapBox(position, attackRange, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out Inimigo enemy))
            {
                int dealtDamage = damage * damageModifier;
                enemy.TakeDamage(dealtDamage);

                if (enemy.canbeStaggered)
                    enemy.rb.AddForce(direction * pushForce, ForceMode.Impulse);

                if (bloodFX)
                    Instantiate(bloodFX, col.transform.position, Quaternion.identity);
            }
        }
    }
}
